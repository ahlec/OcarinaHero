using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace OcarinaHeroLibrary.Controller
{
    public class KeyboardController : IController
    {
        protected PlayerIndex _playerIndex;
        protected ControllerInput previousCallInput;
        public static KeyboardController FromXNAKeyboard(PlayerIndex playerIndex)
        {
            KeyboardController controller = new KeyboardController();
            controller._playerIndex = playerIndex;
            return controller;
        }
        public ControllerInput GetInput(Time elapsedTime)
        {
            KeyboardState keyboardState = Keyboard.GetState(_playerIndex);
            ControllerInput input = new ControllerInput(previousCallInput, elapsedTime,
                keyboardState.IsKeyDown(Keys.A),
                keyboardState.IsKeyDown(Keys.S), keyboardState.IsKeyDown(Keys.D), keyboardState.IsKeyDown(Keys.F),
                keyboardState.IsKeyDown(Keys.G), keyboardState.IsKeyDown(Keys.Down), keyboardState.IsKeyDown(Keys.Up),
                keyboardState.IsKeyDown(Keys.Space), keyboardState.IsKeyDown(Keys.Enter), keyboardState.IsKeyDown(Keys.Right),
                keyboardState.IsKeyDown(Keys.Left));
            previousCallInput = input;
            return input;
        }
        public Time ResponseRange { get { return new Time(0, 0, 500); } }
        public Time MustReleaseRange { get { return new Time(0, 0, 500); } }
    }
}
