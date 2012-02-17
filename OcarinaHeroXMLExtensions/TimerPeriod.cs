using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EngineLibrary
{
    public class TimerPeriod
    {
        /// <summary>
        /// The number of seconds to elapse between each time period
        /// </summary>
        public float TimeSpan { get; set; }
        private float elapsedSeconds = 0f, elapseCount = 0f;
        public float ElapsedSeconds { get { return elapsedSeconds; } }
        public float ElapseCount { get { return elapseCount; } }
        public delegate void ElapsedTime(float changeInSeconds);
        public ElapsedTime ElapseFunction;
        public bool Enabled { get; set; }
        public void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;
            elapsedSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedSeconds >= TimeSpan)
            {
                ElapseFunction((float)gameTime.ElapsedGameTime.TotalSeconds);
                elapsedSeconds -= TimeSpan;
                elapseCount++;
            }
        }
    }
}
