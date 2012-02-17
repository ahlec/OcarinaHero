using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using OcarinaHeroLibrary.Instruments;

namespace OcarinaHeroLibrary.Controller
{
    public class ControllerInput
    {
        public ControllerInput(ControllerInput previousInput, Time elapsedTime, bool aDown, bool bDown, bool cDown,
            bool dDown, bool eDown, bool downDown, bool upDown, bool enterDown, bool pauseDown, bool rightDown,
            bool leftDown)
        {
            _aNoteDown = aDown;
            _bNoteDown = bDown;
            _cNoteDown = cDown;
            _dNoteDown = dDown;
            _eNoteDown = eDown;
            _downDirectionDown = downDown;
            _upDirectionDown = upDown;
            _enter = enterDown;
            _pause = pauseDown;
            _upDownTime = elapsedTime;
            left = leftDown;
            right = rightDown;
            if (previousInput != null)
            {
                _pauseReleased = (previousInput.Pause != _pause);
                _downReleased = (previousInput.Down != _downDirectionDown);
                _upReleased = (previousInput.Up != _upDirectionDown);
                _upDownTime += previousInput.UpStateDuration;
                _downDownTime += previousInput.DownStateDuration;
            }
            if (_upReleased)
                _upDownTime = Time.Zero;
            if (_downReleased)
                _downDownTime = Time.Zero;
        }
        private bool _aNoteDown, _bNoteDown, _cNoteDown, _dNoteDown, _eNoteDown, _downDirectionDown,
            _upDirectionDown, _enter, _pause, _pauseReleased, _downReleased, _upReleased, left, right;
        private Time _upDownTime, _downDownTime;
        public bool NoteA { get { return _aNoteDown; } }
        public bool NoteB { get { return _bNoteDown; } }
        public bool NoteC { get { return _cNoteDown; } }
        public bool NoteD { get { return _dNoteDown; } }
        public bool NoteE { get { return _eNoteDown; } }
        public bool Down { get { return _downDirectionDown; } }
        public bool Up { get { return _upDirectionDown; } }
        public bool Enter { get { return _enter; } }
        public bool Pause { get { return _pause; } }
        public bool PauseReleased { get { return _pauseReleased; } }
        public bool DownReleased { get { return _downReleased; } }
        public bool UpReleased { get { return _upReleased; } }
        public bool Left { get { return left; } }
        public bool Right { get { return right; } }
        public Time UpStateDuration { get { return _upDownTime; } }
        public Time DownStateDuration { get { return _downDownTime; } }

        public bool GetNoteIsPressed(InstrumentNote note)
        {
            if (note == InstrumentNote.A)
                return NoteA;
            else if (note == InstrumentNote.B)
                return NoteB;
            else if (note == InstrumentNote.C)
                return NoteC;
            else if (note == InstrumentNote.D)
                return NoteD;
            else if (note == InstrumentNote.E)
                return NoteE;
            throw new ArgumentException();
        }
    }
}
