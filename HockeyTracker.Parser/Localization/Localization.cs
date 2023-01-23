using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HockeyTracker.Localization
{
    public enum Strings
    {
        NextGame,
        Tonight,
        Today
    }

    public static class LocalizedStrings
    {
        static Dictionary<Strings, Dictionary<string, string>> StringsDictionary = new Dictionary<Strings, Dictionary<string, string>>();

        static LocalizedStrings()
        {
            StringsDictionary = new Dictionary<Strings, Dictionary<string, string>>();

            StringsDictionary.Add(Strings.NextGame, new Dictionary<string, string>() { { "en", "next game" }, { "fr", "prochain match" } });
            StringsDictionary.Add(Strings.Tonight, new Dictionary<string, string>() { { "en", "tonight" }, { "fr", "ce soir" } });
            StringsDictionary.Add(Strings.Today, new Dictionary<string, string>() { { "en", "today" }, { "fr", "aujourd'hui" } });
        }

        public static string Get(Strings id, CultureInfo cultureInfo)
        {
            return Get(id, cultureInfo.Name);
        }

        public static string Get(Strings id, string cultureName)
        {
            if (!StringsDictionary.ContainsKey(id))
            {
                return id.ToString();
            }

            var culture = cultureName.Substring(0, 2);

            switch (culture)
            {
                case "fr":
                    return StringsDictionary[id][culture];
                case "en":
                default: // other cultures
                    return StringsDictionary[id]["en"];
            }
        }
    }
}
