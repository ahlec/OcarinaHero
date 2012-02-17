using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using OcarinaHeroLibrary.Controller;
using OcarinaHeroLibrary.Instruments;
using OcarinaHeroLibrary.Songs;
using OcarinaHeroLibrary.Graphics;

//http://guitarhero.wikia.com/wiki/Multiplier

namespace OcarinaHeroLibrary.Game
{
    public class GameLevel
    {
        public GameLevel(BaseSong song, IController controller, ApplicationSkin skin, ContentManager content, GraphicsDeviceManager graphics)
        {
            _song = song;
            _song.NoteMissed += new BaseSong.NoteInteractedHandler(NoteMissed);
            _song.NoteHit += new BaseSong.NoteInteractedHandler(NoteHit);
            _song.Ended += new BaseSong.SongEndedHandler(SongEnded);
            _controller = controller;
            font = content.Load<SpriteFont>("SpriteFont1");
            Paused += new EventHandler(OnPaused);
            Overstrum += new OverstrumHandler(OverstrumCount);
            _levelTime = new Time(0, 0);
            _camera = new EngineCamera(graphics.GraphicsDevice.Viewport);
            _camera.RotationX += MathHelper.Pi / 20;
            _camera.Y += 10;
            _effect = new BasicEffect(graphics.GraphicsDevice, new EffectPool());

            _skin = skin;
        }
        public EngineCamera Camera { get { return _camera; } }
        void  OverstrumCount(int degreeOverstrum)
        {
            TimesOverstrummed += degreeOverstrum;
            consecutiveSuccessfulNotes = 0;
        }

        protected virtual void SongEnded()
        {
            TogglePause();
            if (LevelEnded != null)
                LevelEnded.Invoke(true); // We haven't failed thus far
        }

        protected virtual void NoteHit(SongActNote note)
        {
            consecutiveSuccessfulNotes++;
            consecutiveNotesMissed = 0;
            _score += GlobalConstants.NotePointBase * CurrentMultiplier;
//            _score += consecutiveSuccessfulNotes * GlobalConstants.NotePointBase;
        }

        protected virtual void NoteMissed(SongActNote note)
        {
            NotesMissed++;
            consecutiveSuccessfulNotes = 0;
            consecutiveNotesMissed++;
//            if (NotesMissed >= notesMissedBecomesFailure)
//                LevelEnded.Invoke(false);
        }

        protected ApplicationSkin _skin;

        protected EngineModel[] _noteBases;

        private BaseSong _song;
        private IController _controller;
        private IInstrument _instrument;
        private bool _paused, _hasBegun, countingDown;
        private Time _levelTime;
        private SpriteFont font;

        private const int notesMissedBecomesFailure = 5;

        protected float _score;
        protected EngineCamera _camera;
        protected int consecutiveSuccessfulNotes = 0;
        protected int consecutiveNotesMissed = 0;

        private TimeSpan startCountdown;
        private TimeSpan startCountdownLength = new TimeSpan(0, 0, 0, 0, 500);

        public BaseSong Song { get { return _song; } }
        public IController Controller { get { return _controller; } }
        public IInstrument Instrument { get { return _instrument; } }
        public Time LevelTime { get { return _levelTime; } }
        public bool HasBegun { get { return _hasBegun; } }
        public float CurrentScore { get { return _score; } }
        public int CurrentMultiplier
        {
            get
            {
                int multiplier = (int)(Math.Floor(consecutiveSuccessfulNotes / 10f));
                return (multiplier == 0 ? 1 : multiplier);
            }
        }
        public bool IsPaused
        {
            get { return _paused; }
            set
            {
                if (!HasBegun)
                    return;
                _paused = value;
                if (Paused != null)
                    Paused.Invoke(this, new EventArgs());
            }
        }
        public void TogglePause() { IsPaused = !IsPaused; }
        public event EventHandler Paused;
        protected virtual void OnPaused(object sender, EventArgs e)
        {
            if (!HasBegun)
                return;

            if (IsPaused)
                MediaPlayer.Pause();
            else
                MediaPlayer.Resume();
        }

        public void Begin()
        {
            MediaPlayer.Play(_song.SongFile);
            _hasBegun = true;
            _paused = false;
            countingDown = true;
            startCountdown = new TimeSpan(startCountdownLength.Ticks);
        }
        public void End()
        {
            if (!IsPaused)
                MediaPlayer.Stop();
            _paused = false;
            _hasBegun = false;
        }

        public int NotesMissed { get; set; }
        public int TimesOverstrummed { get; set; }

        private TimeSpan previousMediaPlayTime;
        private Time onScreenDuration = new Time(0, 1, 500);

        protected List<LevelDisplayedAct> _actsOnScreen = new List<LevelDisplayedAct>();
        private SongAct[] pastUpdateActiveActs = new SongAct[0];

        private int _pauseMenuIndex = 0;
        public string[] pauseMenuItems = new string[] { "Settings", "Return" };

        private EngineModel[] noteModels = new EngineModel[0];
        protected Dictionary<InstrumentNote, Time> inputRidingOnPriorSuccess = new Dictionary<InstrumentNote, Time>();

        public void Update(GameTime gameTime)
        {
            if (!HasBegun)
                return;

            if (countingDown)
            {
                startCountdown -= gameTime.ElapsedGameTime;
                if (startCountdown.TotalMilliseconds <= 0)
                    countingDown = false;
                return;
            }

            ControllerInput controllerInput = _controller.GetInput(gameTime.ElapsedGameTime);

            if (IsPaused)
            {
                if (controllerInput.Down)
                    _pauseMenuIndex++;
                if (controllerInput.Up)
                    _pauseMenuIndex--;
                if (_pauseMenuIndex < 0)
                    _pauseMenuIndex = pauseMenuItems.Length - 1;
                if (_pauseMenuIndex > pauseMenuItems.Length - 1)
                    _pauseMenuIndex = 0;
                if (controllerInput.Pause)
                    IsPaused = false;
                else if (controllerInput.Enter)
                {
                    if (pauseMenuItems[_pauseMenuIndex].Equals("Return"))
                        IsPaused = false;
                }
                return;
            }
            else if (controllerInput.PauseReleased)
                IsPaused = true;

            if (controllerInput.Down)
                _camera.Z++;
            if (controllerInput.Up)
                _camera.Z--;

            _levelTime = MediaPlayer.PlayPosition;
            _song.SetCurrentSongTime(_levelTime, onScreenDuration, _controller.ResponseRange);
            previousMediaPlayTime = MediaPlayer.PlayPosition;

            List<EngineModel> noteModelsList = new List<EngineModel>();
            int[] activeActIndeces = _song.GetActiveActsIndeces();
            foreach (int index in activeActIndeces)
            {
                if (_song[index] is SongActNote)
                {
                    SongActNote noteAct = (SongActNote)_song[index];
                    if (!noteAct.Hit && !noteAct.Missed)
                    {
                        if (noteAct.InFeedbackZone &&
                            !inputRidingOnPriorSuccess.ContainsKey(noteAct.Note) &&
                            controllerInput.GetNoteIsPressed(noteAct.Note))
                        {
                            ((SongActNote)_song[index]).Hit = true;
                            inputRidingOnPriorSuccess.Add(noteAct.Note, _levelTime);
                        }
                        else if (noteAct.Active)
                            this.ToString();
                        if (noteAct.InFeedbackZone)
                            noteAct.Model.AmbientColor = Color.Yellow;
                    }
                    float distPerc = ((noteAct.Time - _levelTime).TotalMilliseconds / onScreenDuration.TotalMilliseconds);
                    float distToEnd = (Math.Abs(_skin.TrackStartZ) - Math.Abs(_skin.TrackEndZ));
                    float distTraveled = distToEnd * (1 - distPerc);
                    noteAct.Model.Z = _skin.TrackStartZ + Math.Abs(distTraveled);
                    noteModelsList.Add(noteAct.Model);
                }
            }
            noteModels = noteModelsList.ToArray();

            Dictionary<InstrumentNote, bool> inputClearance = new Dictionary<InstrumentNote,bool>();
            // Ultimately, false values in this array mean that note's input has been cleared. True means we have
            // "rogue" input, dry firing
            inputClearance.Add(InstrumentNote.A, controllerInput.NoteA);
            inputClearance.Add(InstrumentNote.B, controllerInput.NoteB);
            inputClearance.Add(InstrumentNote.C, controllerInput.NoteC);
            inputClearance.Add(InstrumentNote.D, controllerInput.NoteD);
            inputClearance.Add(InstrumentNote.E, controllerInput.NoteE);
            List<InstrumentNote> notesToRemoveFromPriorSuccessDictionary = new List<InstrumentNote>();
            foreach (InstrumentNote inputNote in inputRidingOnPriorSuccess.Keys)
            {
                Time originalInputTime = inputRidingOnPriorSuccess[inputNote];
                if (inputRidingOnPriorSuccess.Keys.Contains(inputNote) &&
                    !controllerInput.GetNoteIsPressed(inputNote))
                {
                    notesToRemoveFromPriorSuccessDictionary.Add(inputNote);
                    inputClearance[inputNote] = false;
                    continue;
                }
                if (originalInputTime + _controller.MustReleaseRange >= _levelTime)
                    inputClearance[inputNote] = false;
                else
                    notesToRemoveFromPriorSuccessDictionary.Add(inputNote);
            }
            foreach (InstrumentNote note in notesToRemoveFromPriorSuccessDictionary)
                inputRidingOnPriorSuccess.Remove(note);

            int dryFiredInput = 0;
            foreach (bool isRogue in inputClearance.Values)
                if (isRogue)
                    dryFiredInput++;
            if (Overstrum != null)
                Overstrum.Invoke(dryFiredInput);
        }

        BasicEffect _effect;
        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            graphics.GraphicsDevice.Clear(Color.LightGray);

            if (!HasBegun)
                return;

            _effect.View = _camera.ViewMatrix;
            _effect.Projection = _camera.ProjectionMatrix;
            _effect.Begin();
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                _skin.GameTrack.Draw(graphics.GraphicsDevice, _camera);
                _skin.CollisionArea.Draw(graphics.GraphicsDevice, _camera);
                _skin.TrackA.Draw(graphics.GraphicsDevice, _camera);
                _skin.TrackB.Draw(graphics.GraphicsDevice, _camera);
                _skin.TrackC.Draw(graphics.GraphicsDevice, _camera);
                _skin.TrackD.Draw(graphics.GraphicsDevice, _camera);
                _skin.TrackE.Draw(graphics.GraphicsDevice, _camera);
                if (!countingDown)
                    foreach (EngineModel model in noteModels)
                       model.Draw(graphics.GraphicsDevice, _camera);
                pass.End();
            }
            _effect.End();

            if (countingDown)
            {
                spriteBatch.Begin();
                float opacity = (float)(startCountdown.TotalMilliseconds / startCountdownLength.TotalMilliseconds);
                spriteBatch.FillRectangle(0, 0, graphics.GraphicsDevice.Viewport.Width,
                    graphics.GraphicsDevice.Viewport.Height, new Color(Color.Gray, opacity));
                Vector2 numberSize = _skin.ScoreFont.MeasureString((startCountdown.Seconds > 0 ?
                    startCountdown.Seconds.ToString() : "0"));
                Vector2 initialPosition = new Vector2((graphics.GraphicsDevice.Viewport.Width - numberSize.X) / 2,
                            (graphics.GraphicsDevice.Viewport.Height - numberSize.Y) / 3);
                Vector2 position;
                if (startCountdown.Milliseconds > 700)
                    position = initialPosition + new Vector2(0, 50f * ((startCountdown.Milliseconds - 700f) / 300f));
                else
                    position = initialPosition + new Vector2(0, 50 -
                        100f * ((startCountdown.Milliseconds - 300f) / 700f));
                spriteBatch.DrawString(_skin.ScoreFont, (startCountdown.Seconds > 0 ?
                    startCountdown.Seconds.ToString() : "0"), position,
                    Color.White, (float)((startCountdown.Milliseconds / 500f)));
                spriteBatch.End();
                return;
            }
            
            spriteBatch.Begin();

            if (IsPaused)
            {
                spriteBatch.FillRectangle(0, 0, graphics.GraphicsDevice.Viewport.Width,
                    graphics.GraphicsDevice.Viewport.Height, new Color(Color.Gray, 125));
                Vector2 pauseMenuLocation = new Vector2((graphics.GraphicsDevice.Viewport.Width - 300) / 2, 0);
                spriteBatch.FillRectangle(pauseMenuLocation.X,
                    pauseMenuLocation.Y, 300, graphics.GraphicsDevice.Viewport.Height, Color.White);
                spriteBatch.DrawLine(pauseMenuLocation.X, 0, pauseMenuLocation.X,
                    graphics.GraphicsDevice.Viewport.Height, Color.Black);
                spriteBatch.DrawLine(pauseMenuLocation.X + 300, 0, pauseMenuLocation.X + 300,
                    graphics.GraphicsDevice.Viewport.Height, Color.Black);
                spriteBatch.DrawString(font, "Paused", new Vector2(pauseMenuLocation.X +
                    (int)((300 - font.MeasureString("Paused").X) / 2), 75), Color.Black);
                for (int drawMenuIndex = 0; drawMenuIndex < pauseMenuItems.Length; drawMenuIndex++)
                {
                    Vector2 itemSize = font.MeasureString(pauseMenuItems[drawMenuIndex]);
                    Vector2 itemLocation = new Vector2(pauseMenuLocation.X + (int)((300 -
                            itemSize.X) / 2), 120 + 20 * drawMenuIndex);
                    spriteBatch.DrawString(font, pauseMenuItems[drawMenuIndex], itemLocation, Color.Black);
                    if (drawMenuIndex == _pauseMenuIndex)
                    {
                        spriteBatch.DrawRectangle(pauseMenuLocation + new Vector2(10, 120 + 20 * drawMenuIndex),
                            new Vector2(280, 20), Color.Black);
                    }
                }
            }

            /* Current Score */
            string scoreString = _score.ToString().PadLeft(6, '0');
            spriteBatch.DrawString(_skin.ScoreFont, scoreString, scorePosition, Color.White);

            spriteBatch.End();
        }

        private Vector2 scorePosition = new Vector2(10, 10);

        public delegate void OnLevelEndedHandler(bool passedLevel);
        public event OnLevelEndedHandler LevelEnded;
        public delegate void OverstrumHandler(int degreeOverstrum);
        public event OverstrumHandler Overstrum;
    }
}