using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using StatsParser.DataLayerService;

namespace StatsParser
{
    class Games
    {
        public int UpdateGamesTimeout = 60000;

        List<Game> GamesFollowed = new List<Game>();

        public Game.GameStartedEventHandler GameStartedEventHandler;
        public Game.GoalEventHandler GoalEventHandler;
        public Game.GameEndedEventHandler GameEndedEventHandler;
        public SendNextGameEventHandler SendNextGameNotificationsEventHandler;

        public delegate void SendNextGameEventHandler(object sender, EventArgs e);
        public delegate void GameStartingEventHandler(object sender, EventArgs e);

        public void Go()
        {
            while (true)
            {
                this.Parse();

                this.UpdateGames();

                this.CheckForNextGameNotifs();

                Thread.Sleep(UpdateGamesTimeout);
            }
        }

        public void UpdateGames()
        {
            Console.WriteLine("Looking for updates...");

            for (int i = GamesFollowed.Count - 1; i >= 0; i--)
            {
                GamesFollowed[i].Parse();
            }
        }

        public void CheckForNextGameNotifs()
        {
            // Need a separate list so as not to change collection while enumerating
            List<int> gamesToRemove = new List<int>();

            foreach (var game in GamesFollowed.Where(g => g.State == Game.GameState.Final))
            {
                if ((DateTime.UtcNow - game.Ended).TotalHours >= 6)
                {
                    if (SendNextGameNotificationsEventHandler != null)
                    {
                        SendNextGameNotificationsEventHandler(game, EventArgs.Empty);
                    }

                    // on enleve la game x heures apres la fin. idealement, on ne la repogne pas au prochain refresh de schedulegames (ca doit aller chercher les vieilles games < x heures passees)
                    gamesToRemove.Add(GamesFollowed.IndexOf(game));
                }
            }

            foreach (var gameToRemove in gamesToRemove.OrderByDescending(g => g))
            {
                var scheduleGame = Schedule.ScheduleGames.FirstOrDefault(g => g.Num == GamesFollowed.ElementAt(gameToRemove).Number);
                if (scheduleGame != null)
                {
                    Schedule.ScheduleGames.Remove(scheduleGame);
                }

                GamesFollowed.RemoveAt(gameToRemove);
            }
        }

        public void Parse()
        {
            Console.WriteLine("Looking for new games to follow...");

            // Trouver les games d'aujourd'hui
            foreach (var scheduleGame in Schedule.ScheduleGames.Where(game => !GamesFollowed.Any(gameFollowed => gameFollowed.Number == game.Num)))
            {
                Game game = new Game(scheduleGame);

                Console.WriteLine("Trying to add game #" + scheduleGame.Num);

                if (game.Number == 0 || game.Home == null || game.Visitor == null) // Couldn't load game - maybe html report doesn't exist yet
                    continue;

                GamesFollowed.Add(game);

                Console.WriteLine("New game!");
                Console.WriteLine(game);
                Console.WriteLine(game.GetResults());

                // Send game details to service
                DataLayerServiceClient dataLayer = new DataLayerServiceClient("BasicHttpBinding_IDataLayerService");

                // Sql can't store the "MinValue" (year=0001) as its own minvalue is (year=1753). so field is nullable over there
                DateTime? sqlStartedDateTime = game.Started;
                DateTime? sqlEndedDateTime = game.Ended;

                if (game.Started == DateTime.MinValue)
                {
                    sqlStartedDateTime = null;
                }

                if (game.Ended == DateTime.MinValue)
                {
                    sqlEndedDateTime = null;
                }

                dataLayer.UpdateGame(game.Number, game.GameTime, game.Home.ShortName, game.Visitor.ShortName, game.HomeGoals, game.VisitorGoals, sqlStartedDateTime, sqlEndedDateTime);

                dataLayer.Close();

                if (GameStartedEventHandler != null)
                {
                    game.GameStarted += GameStartedEventHandler;
                }

                if (GoalEventHandler != null)
                {
                    game.Goal += GoalEventHandler;
                }

                if (GameEndedEventHandler != null)
                {
                    game.GameEnded += GameEndedEventHandler;
                }
            }
        }
    }
}
