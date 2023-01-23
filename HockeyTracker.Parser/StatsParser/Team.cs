using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StatsParser
{
    class Team
    {
        public string Name;
        public string ShortName;
        public string AltShortName;
        public string Locale;

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Locale, this.Name);
        }
    }
}
