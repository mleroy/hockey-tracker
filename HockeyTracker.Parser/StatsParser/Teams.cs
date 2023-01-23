using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StatsParser
{
    class Teams
    {
        public static List<Team> NhlTeams
        {
            get
            {
                if (_NhlTeams == null)
                {
                    _NhlTeams = PopulateTeams();
                }

                return _NhlTeams;
            }
        }

        static List<Team> _NhlTeams;

        static List<Team> PopulateTeams()
        {
            List<Team> t = new List<Team>();

            t.Add(new Team { ShortName = "ANA", Name = "Ducks", Locale = "Anaheim" });
            t.Add(new Team { ShortName = "BOS", Name = "Bruins", Locale = "Boston" });
            t.Add(new Team { ShortName = "BUF", Name = "Sabres", Locale = "Buffalo" });
            t.Add(new Team { ShortName = "CAR", Name = "Hurricanes", Locale = "Carolina" });
            t.Add(new Team { ShortName = "CBJ", Name = "Blue Jackets", Locale = "Columbus" });
            t.Add(new Team { ShortName = "CGY", Name = "Flames", Locale = "Calgary" });
            t.Add(new Team { ShortName = "CHI", Name = "Blackhawks", Locale = "Chicago" });
            t.Add(new Team { ShortName = "COL", Name = "Avalanche", Locale = "Colorado" });
            t.Add(new Team { ShortName = "DAL", Name = "Stars", Locale = "Dallas" });
            t.Add(new Team { ShortName = "DET", Name = "Red Wings", Locale = "Detroit" });
            t.Add(new Team { ShortName = "EDM", Name = "Oilers", Locale = "Edmonton" });
            t.Add(new Team { ShortName = "FLA", Name = "Panthers", Locale = "Florida" });
            t.Add(new Team { ShortName = "LAK", Name = "Kings", Locale = "Los Angeles", AltShortName = "L.A" });
            t.Add(new Team { ShortName = "MIN", Name = "Wild", Locale = "Minnesota" });
            t.Add(new Team { ShortName = "MTL", Name = "Canadiens", Locale = "Montreal" });
            t.Add(new Team { ShortName = "NJD", Name = "Devils", Locale = "New Jersey", AltShortName = "N.J" });
            t.Add(new Team { ShortName = "NSH", Name = "Predators", Locale = "Nashville" });
            t.Add(new Team { ShortName = "NYI", Name = "Islanders", Locale = "New York" });
            t.Add(new Team { ShortName = "NYR", Name = "Rangers", Locale = "New York" });
            t.Add(new Team { ShortName = "OTT", Name = "Senators", Locale = "Ottawa" });
            t.Add(new Team { ShortName = "PHI", Name = "Flyers", Locale = "Philadelphia" });
            t.Add(new Team { ShortName = "ARI", Name = "Coyotes", Locale = "Arizona" });
            t.Add(new Team { ShortName = "PIT", Name = "Penguins", Locale = "Pittsburgh" });
            t.Add(new Team { ShortName = "SJS", Name = "Sharks", Locale = "San Jose", AltShortName = "S.J" });
            t.Add(new Team { ShortName = "STL", Name = "Blues", Locale = "St. Louis" });
            t.Add(new Team { ShortName = "TBL", Name = "Lightning", Locale = "Tampa Bay", AltShortName = "T.B" });
            t.Add(new Team { ShortName = "TOR", Name = "Maple Leafs", Locale = "Toronto" });
            t.Add(new Team { ShortName = "VAN", Name = "Canucks", Locale = "Vancouver" });
            t.Add(new Team { ShortName = "WPG", Name = "Jets", Locale = "Winnipeg" });
            t.Add(new Team { ShortName = "WSH", Name = "Capitals", Locale = "Washington" });

            return t;
        }

        public static Team Parse(string name)
        {
            if (name.Length == 3)
            {
                return NhlTeams.FirstOrDefault(t => t.ShortName == name) ?? NhlTeams.First(t => t.AltShortName == name);
            }
            else
            {
                return NhlTeams.Single(t => t.ToString().Equals(name, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}