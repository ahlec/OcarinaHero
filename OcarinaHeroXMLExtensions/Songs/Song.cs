using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using OcarinaHeroLibrary.Instruments;

namespace OcarinaHeroLibrary.Songs
{
    public class BaseSong
    {
        protected Song _songFile;
        protected string _title;
        protected InstrumentTypes _instrumentType;
        protected SongDifficulty _difficulty;
        protected Time _duration;
        protected SongAct[] _acts;
        protected float _speed;
        protected int maxScore;

        public Song SongFile { get { return _songFile; } }
        public string Title { get { return _title; } }
        public InstrumentTypes Instrument { get { return _instrumentType; } }
        public SongDifficulty Difficulty { get { return _difficulty; } }
        public Time Duration { get { return _duration; } }
        public SongAct[] Acts { get { return _acts; } }
        public float Speed { get { return _speed; } }
        public int MaxScore { get { return maxScore; } }

        protected virtual void Initialize()
        {
            for (int actIndex = 0; actIndex < _acts.Length; actIndex++)
            {
                _acts[actIndex].SongActIndex = actIndex;
                if (_acts[actIndex] is SongActNote)
                {
                    ((SongActNote)_acts[actIndex]).NoteMissed += new SongActNote.OnNoteMissedHandler(NoteHasBeenMissed);
                    ((SongActNote)_acts[actIndex]).NoteHit += new SongActNote.OnNoteHitHandler(NoteHasBeenHit);
                }
            }
            _duration = new Time(SongFile.Duration);
            maxScore = (int)(GlobalConstants.NotePointBase * _acts.Length);
//            maxScore = (int)(GlobalConstants.NotePointBase / 2) * (_acts.Length - 1) * _acts.Length;
        }

        protected virtual void NoteHasBeenHit(SongActNote noteAct)
        {
            if (NoteHit != null)
                NoteHit.Invoke(noteAct);
        }

        protected virtual void NoteHasBeenMissed(SongActNote noteAct)
        {
            if (NoteMissed != null)
                NoteMissed.Invoke(noteAct);
        }

        protected int[] actsActiveIndeces;
        public void SetCurrentSongTime(Time time, Time bufferTimeOnScreen, Time responseRange)
        {
            if (time > _duration)
            {
                if (Ended != null)
                    Ended.Invoke();
                return;
            }
            List<int> activeActsIndeces = new List<int>();
            foreach (SongAct act in _acts)
            {
                if (act is SongActNote)
                {
                    SongActNote noteAct = (SongActNote)act;
                    if (time > noteAct.Time + noteAct.Length)
                    {
                        if (!noteAct.Passed)
                        {
                            noteAct.Active = false;
                            noteAct.Passed = true;
                            if (!noteAct.Hit)
                                noteAct.Missed = true;
                            noteAct.InFeedbackZone = false;
                        }
                    }
                    else if (time + bufferTimeOnScreen >= noteAct.Time)
                    {
                        noteAct.Active = true;
                        noteAct.InFeedbackZone = (time >= noteAct.Time - responseRange &&
                            time <= noteAct.Time + responseRange);
                        activeActsIndeces.Add(noteAct.SongActIndex);
                    }
                    else if (time + bufferTimeOnScreen < noteAct.Time)
                    {
                        noteAct.Active = false;
                    }
                }
            }
            actsActiveIndeces = activeActsIndeces.ToArray();
        }
        public delegate void NoteInteractedHandler(SongActNote note);
        public event NoteInteractedHandler NoteMissed;
        public event NoteInteractedHandler NoteHit;

        public int[] GetActiveActsIndeces()
        {
            return actsActiveIndeces;
        }

        public SongAct this[int index]
        {
            get { return _acts[index]; }
            set { _acts[index] = value; }
        }

        public SongAct[] GetActs(Time fromTime)
        {
            return GetActs(fromTime, null);
        }
        public SongAct[] GetActs(Time fromTime, Time length)
        {
            List<SongAct> acts = new List<SongAct>();
            foreach (SongAct act in _acts)
            {
                if (act.Time >= fromTime && act.Length == null)
                    acts.Add(act);
                else if (act.Length != null && act.Time >= fromTime && act.Time <= fromTime + length)
                    acts.Add(act);
                else if (act.Length != null && act.Time + act.Length >= fromTime && act.Time + act.Length <= fromTime + length)
                    acts.Add(act);
            }
            return acts.ToArray();
        }
        public void RespondTo(SongAct act, bool hit)
        {
            _acts[act.SongActIndex].RespondedTo = true;
        }
        public static BaseSong FromXML(string filepath, ContentManager content, ApplicationSkin skin)
        {
            BaseSong song = new BaseSong();
            System.IO.Stream stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open);
            XDocument xmlFile = XDocument.Load(System.Xml.XmlReader.Create(stream), LoadOptions.None);
            XElement xmlSong = xmlFile.Element("Song");
            song._title = xmlSong.Element("Title").Value;
            song._speed = float.Parse(xmlSong.Element("Speed").Value);
            song._difficulty = (SongDifficulty)Enum.Parse(typeof(SongDifficulty),
                xmlSong.Element("Difficulty").Value);
            song._songFile = content.Load<Song>(xmlSong.Element("SongFile").Value);

            List<SongAct> acts = new List<SongAct>();
            foreach (XElement act in xmlSong.Element("Acts").Elements())
            {
                SongAct songAct;
                if (act.Name.LocalName.Equals("A") || act.Name.LocalName.Equals("B") ||
                    act.Name.LocalName.Equals("C") || act.Name.LocalName.Equals("D") ||
                    act.Name.LocalName.Equals("E"))
                {
                    songAct = new SongActNote(Time.FromString(act.Value),
                        act.Name.LocalName);
                    ((SongActNote)songAct).Model = skin.GetNoteModel(((SongActNote)songAct).Note);
                }
                else
                    songAct = new SongAct(Time.FromString(act.Element("Time").Value));
                acts.Add(songAct);
            }
            song._acts = acts.ToArray();
            song.Initialize();

            return song;
        }
        public delegate void SongEndedHandler();
        public event SongEndedHandler Ended;
    }
}
