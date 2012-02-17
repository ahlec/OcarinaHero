using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace OcarinaHeroLibrary.Controller
{
    public class OcarinaController : IController
    {
        protected PlayerIndex _playerIndex;
        protected ControllerInput previousCallInput;
        public static OcarinaController FromXNAKeyboard(PlayerIndex playerIndex)
        {
            OcarinaController controller = new OcarinaController();
            controller._playerIndex = playerIndex;
            return controller;
        }
        public ControllerInput GetInput(Time elapsedTime)
        {
            KeyboardState keyboardState = Keyboard.GetState(_playerIndex);
            ControllerInput input = new ControllerInput(previousCallInput, elapsedTime,
                keyboardState.IsKeyDown(Keys.Enter), keyboardState.IsKeyDown(Keys.Add),
                keyboardState.IsKeyDown(Keys.NumPad4), keyboardState.IsKeyDown(Keys.NumPad1),
                keyboardState.IsKeyDown(Keys.NumPad7), keyboardState.IsKeyDown(Keys.NumPad3),
                keyboardState.IsKeyDown(Keys.NumPad5), keyboardState.IsKeyDown(Keys.NumPad0),
                keyboardState.IsKeyDown(Keys.Decimal), keyboardState.IsKeyDown(Keys.NumPad6),
                keyboardState.IsKeyDown(Keys.NumPad2));
            previousCallInput = input;
            return input;
        }
        public Time ResponseRange { get { return new Time(0, 0, 500); } }
        public Time MustReleaseRange { get { return new Time(0, 0, 500); } }
    }
}
