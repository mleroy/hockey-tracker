using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace StatsParser
{
    class Player
    {
        public int Number;
        public Name FullName;
        public int Points;

        public class Name
        {
            public string First;
            public string Last;
        }

        public static Player Parse(string innerText)
        {
            Match playerMatch = Regex.Match(innerText, @"(?<Number>[0-9]{1,2})\s(?<FirstName>[A-Z])\.(?<LastName>[^\(\)]+)(\((?<Points>[0-9]+)\))?");

            if (playerMatch.Success)
            {
                string lastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(playerMatch.Groups["LastName"].Value.ToLower());

                return new Player
                {
                    Number = int.Parse(playerMatch.Groups["Number"].Value),
                    FullName = new Name { First = playerMatch.Groups["FirstName"].Value, Last = lastName},
                    Points = playerMatch.Groups["Points"].Success ? int.Parse(playerMatch.Groups["Points"].Value) : 0
                };
            }
            else return null;
        }
    }
}
