using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace OcarinaHeroLibrary.Instruments
{
    public interface IInstrument
    {
        InstrumentNote this[string index] { get; }
    }
}
