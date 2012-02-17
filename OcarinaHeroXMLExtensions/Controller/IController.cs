using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OcarinaHeroLibrary.Controller
{
    public interface IController
    {
        ControllerInput GetInput(Time elapsedTime);
        Time ResponseRange { get; }
        Time MustReleaseRange { get; }
    }
}
