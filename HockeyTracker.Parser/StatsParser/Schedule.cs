using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using StatsParser.DataLayerService;

namespace StatsParser
{
    class ScheduleGame
    {
        public int Num;
        public DateTime GameTime;

        public string Url
        {
            get
            { return string.Format("http://www.nhl.com/scores/htmlreports/20142015/GS0{0}.HTM", this.Num); }
        }

        public ScheduleGame(int num, DateTime gameTime)
        {
            this.Num = num;
            this.GameTime = gameTime;
        }
    }

    class Schedule
    {
        static DateTime lastUpdateTime = DateTime.MinValue;
        static int goBackInTimeInHours = 6;
        static int goForwardInTimeInMinutes = 15;

        public static List<ScheduleGame> ScheduleGames
        {
            get
            {
                if (scheduleGames == null || (DateTime.Now - lastUpdateTime).Minutes >= goForwardInTimeInMinutes)
                {
                    Refresh();
                    lastUpdateTime = DateTime.Now;
                }

                return scheduleGames;
            }
        }
        static List<ScheduleGame> scheduleGames;

        public static void Refresh()
        {
            DataLayerServiceClient dataLayer = new DataLayerServiceClient("BasicHttpBinding_IDataLayerService");

            HockeyTracker.Data.Game[] games;

            try
            {
                games = dataLayer.
                    GetGamesToParse(DateTime.UtcNow.AddHours(-goBackInTimeInHours), DateTime.UtcNow.AddMinutes(goForwardInTimeInMinutes));
            }
            catch (Exception) { games = new HockeyTracker.Data.Game[0]; } // parfois le endpoint repond pas...

            scheduleGames = (from game in games
                             select new ScheduleGame(game.GameNumber, game.GameTime)).ToList();
            
            dataLayer.Close();
            
            // garde ca ici en souvenir
            // 03 = series, 01 = round number (1 to 4), 1 = position du meilleur des 2 (+4 pour west), 1 = game number in series (1 to 7)
            //ScheduleGames.Add(new ScheduleGame("030141", "Wed Apr 13, 2011", "7:00 PM ET", "TAMPA BAY", "PITTSBURGH"));
            
            // 03 = series, 02 = round number (1 to 4), 1 = position du meilleur des 2 (+2 pour west), 1 = game number in series (1 to 7)
            //ScheduleGames.Add(new ScheduleGame("030231", "Thu Apr 28, 2011", "9:00 PM ET", "NASHVILLE", "VANCOUVER"));
        }
    }
}
