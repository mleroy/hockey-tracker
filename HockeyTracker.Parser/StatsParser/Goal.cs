using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatsParser
{
    class Goal
    {
        public int Number;
        public int Period;
        public TimeSpan Time;
        public Player Scorer;
        public Team Team;
        public List<Player> Assists;

        public void GetGoalToastTexts(Game game, out string titleHome, out string titleVisitor, out string message)
        {
            if (this.Team == game.Home)
            {
                titleHome = string.Format("{0} GOAL!", this.Team.Name);
                titleVisitor = string.Format("{0} goal", this.Team.Name);
            }
            else
            {
                titleHome = string.Format("{0} goal", this.Team.Name);
                titleVisitor = string.Format("{0} GOAL!", this.Team.Name);
            }

            message = this.ToString();
        }

        public override string ToString()
        {
            string periodText = string.Empty;
            string timeText = string.Empty;

            switch (Period)
            {
                case 1:
                    periodText = " in 1st";
                    break;
                case 2:
                    periodText = " in 2nd";
                    break;
                case 3:
                    periodText = " in 3rd";
                    break;
                case 4:
                    periodText = " in OT"; // Dubinsky (xx:xx in OT)
                    break;
                case 5:
                    periodText = "SO"; // Dubinsky (SO)
                    break;
            }

            // Avoid showing 0:00 for shoot-out goals
            if (Period < 5)
            {
                timeText = string.Format("{0:00}:{1:00}", Time.Minutes, Time.Seconds);
            }

            string output = string.Format("{0}, {1}. ({2}{3})", Scorer.FullName.Last, Scorer.FullName.First.Substring(0, 1), timeText, periodText);

            //if (Assists.Count > 0)
            //{
            //    output += " (" + Assists[0].Name;

            //    if (Assists.Count > 1)
            //    {
            //        output += ", " + Assists[1].Name;
            //    }

            //    output += ")";
            //}

            return output;
        }
    }
}
