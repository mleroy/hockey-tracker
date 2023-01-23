using HockeyTracker.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NotificationService
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the date as today, tomorrow, month/day
        /// </summary>
        public static string ToFriendlyDate(this DateTime date, int utcOffsetInMinutes, CultureInfo culture)
        {
            DateTime localDateTime = date.AddMinutes(utcOffsetInMinutes);

            if (localDateTime.Date == DateTime.Today)
            {
                if (localDateTime.Hour >= 17)
                {
                    return LocalizedStrings.Get(Strings.Tonight, culture);
                }
                else
                {
                    return LocalizedStrings.Get(Strings.Today, culture);
                }
                // Can't use this until the server sends notifications at midnight every day (midnight where?)
                // Otherwise users will get 'game tomorrow' after a game ends, and the following day (ex.: morning), it will still say 'tomorrow'
                //else if (localDateTime.Date == DateTime.Today.AddDays(1))
                //{
                //    return "tomorrow";
                //}
            }

            return localDateTime.ToString("ddd, M/d");
        }

        /// <summary>
        /// Returns a time as 7.30pm
        /// </summary>
        public static string ToFriendlyTime(this DateTime date, int utcOffsetInMinutes, CultureInfo culture)
        {
            return date.AddMinutes(utcOffsetInMinutes).ToString("h.mmtt").ToLower();
        }
    }
}
