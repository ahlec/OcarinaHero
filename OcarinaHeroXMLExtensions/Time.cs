using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OcarinaHeroLibrary
{
    public class Time
    {
        private TimeSpan _baseTimeSpan;
        public Time(int minutes, int seconds)
        {
            _minutes = minutes;
            _seconds = seconds;
            _milliseconds = 0;
            _baseTimeSpan = new TimeSpan(0, 0, minutes, seconds);
        }
        public Time(float minutes, float seconds)
        {
            _minutes = minutes;
            _seconds = seconds;
            _milliseconds = 0;
            _baseTimeSpan = new TimeSpan(0, 0, (int)minutes, (int)seconds);
        }
        public Time(int minutes, int seconds, int milliseconds)
        {
            _minutes = minutes;
            _seconds = seconds;
            _milliseconds = milliseconds;
            _baseTimeSpan = new TimeSpan(0, 0, (int)minutes, (int)seconds, (int)milliseconds);
        }
        public Time(float minutes, float seconds, float milliseconds)
        {
            _minutes = minutes;
            _seconds = seconds;
            _milliseconds = milliseconds;
            _baseTimeSpan = new TimeSpan(0, 0, (int)minutes, (int)seconds, (int)milliseconds);
        }
        public Time(TimeSpan timeSpan)
        {
            _baseTimeSpan = timeSpan;
        }
        protected float _minutes, _seconds, _milliseconds;
        public float Minutes { get { return _baseTimeSpan.Minutes; } }
        public float Seconds { get { return _baseTimeSpan.Seconds; } }
        public float Milliseconds { get { return _baseTimeSpan.Milliseconds; } }
        public float TotalMilliseconds { get { return (float)_baseTimeSpan.TotalMilliseconds; } }
        public float TotalSeconds { get { return (float)_baseTimeSpan.TotalSeconds; } }
        public static Time FromString(string str)
        {
            int minutes = Int32.Parse(str.Substring(0, 2));
            int seconds = Int32.Parse(str.Substring(3, 2));
            int milliseconds = 0;
            if (str.Length > 5)
                milliseconds = Int32.Parse(str.Substring(6, 2));
            return new Time(minutes, seconds, milliseconds);
        }
        public static Time FromGameTime(GameTime gameTime)
        {
            return new Time(gameTime.ElapsedGameTime);
        }
        public override string ToString()
        {
            return "{" + Minutes.ToString().PadLeft(2, '0') + ":" + Seconds.ToString().PadLeft(2, '0') + 
                ":" + Milliseconds.ToString().PadLeft(4, '0') + "}";
        }
        public static Time operator +(Time timeA, Time timeB)
        {
            if (timeB == null)
                return timeA;
            if (timeA == null)
                return timeB;
            return new Time(timeA._baseTimeSpan.Add(timeB._baseTimeSpan));
        }
        public static Time operator -(Time timeA, Time timeB)
        {
            if (timeB == null)
                return timeA;
            if (timeA == null)
                return new Time(0, 0, 0) - timeB;
            return new Time(timeA._baseTimeSpan.Subtract(timeB._baseTimeSpan));
        }
        public static implicit operator Time(TimeSpan timeSpan)
        {
            return new Time(timeSpan);
        }
        public static implicit operator Time(GameTime gameTime)
        {
            return Time.FromGameTime(gameTime);
        }
        public static bool operator >=(Time timeA, Time timeB)
        {
            return timeA._baseTimeSpan.TotalMilliseconds >= timeB._baseTimeSpan.TotalMilliseconds;
        }
        public static bool operator <=(Time timeA, Time timeB)
        {
            return timeA._baseTimeSpan.TotalMilliseconds <= timeB._baseTimeSpan.TotalMilliseconds;
        }
        public static bool operator <(Time timeA, Time timeB)
        {
            return timeA._baseTimeSpan.TotalMilliseconds < timeB._baseTimeSpan.TotalMilliseconds;
        }
        public static bool operator >(Time timeA, Time timeB)
        {
            return timeA._baseTimeSpan.TotalMilliseconds > timeB._baseTimeSpan.TotalMilliseconds;
        }
        public static Time Zero = new Time(0, 0, 0);
    }
}
