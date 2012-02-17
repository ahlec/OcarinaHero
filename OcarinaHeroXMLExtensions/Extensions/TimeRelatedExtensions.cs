using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OcarinaHeroLibrary
{
    public static class TimeRelatedExtensions
    {
        /*public static TimeSpan operator *(TimeSpan timeSpan, float number)
        {
            return TimeSpan.FromMilliseconds(timeSpan.TotalMilliseconds * number);
        }*/
        public static TimeSpan Multiply(this TimeSpan timeSpan, float number)
        {
            return TimeSpan.FromMilliseconds(timeSpan.TotalMilliseconds * number);
        }
    }
}
