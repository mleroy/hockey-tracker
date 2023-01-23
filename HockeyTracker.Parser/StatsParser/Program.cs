using System;
using System.Linq;
using StatsParser.DataLayerService;
using StatsParser.NotificationService;

namespace StatsParser
{
    class Program
    {
        static NotificationServiceClient notificationService;

        static void Main(string[] args)
        {
            notificationService = new NotificationServiceClient("BasicHttpBinding_NotificationService");
            StartListening();
        }

        static void StartListening()
        {
            Games gamesListener = new Games
            {
                UpdateGamesTimeout = 60000,
                GameStartedEventHandler = new Game.GameStartedEventHandler(HandleGameStarted),
                GoalEventHandler = new Game.GoalEventHandler(HandleGoal),
                GameEndedEventHandler = new Game.GameEndedEventHandler(HandleGameEnded),
                SendNextGameNotificationsEventHandler = new Games.SendNextGameEventHandler(SendNextGameNotification)
            };

            gamesListener.Go();
        }

        static void SendNextGameNotification(object sender, EventArgs e)
        {
            Game game = (Game)sender;

            try
            {
                notificationService.SendTileNotificationsForNextGame(game.Home.ShortName);
                notificationService.SendTileNotificationsForNextGame(game.Visitor.ShortName);

                Console.WriteLine("\r\n\r\nSent next game for : {0}!", game.Home.ShortName);
                Console.WriteLine("\r\n\r\nSent next game for : {0}!", game.Visitor.ShortName);
            }
            catch (Exception ex) { }
        }

        static void HandleGameStarted(object sender, EventArgs e)
        {
            Game game = (Game)sender;

            UpdateGame(game);

            string tileTitle, tileContent;

            try
            {
                game.GetLiveTileContents(game.Home.ShortName, out tileTitle, out tileContent);

                notificationService.SendTileNotifications(game.Home.ShortName, tileTitle, tileContent);
                notificationService.SendTileNotifications(game.Visitor.ShortName, tileTitle, tileContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Got exception while trying to send notifications: " + ex.ToString());
            }

            Console.WriteLine("\r\n\r\nGame {0} started!", game.Number);
        }

        static void HandleGameEnded(object sender, EventArgs e)
        {
            Game game = (Game)sender;

            UpdateGame(game);

            string toastTitleHome, toastTitleVisitor, message;
            game.GetFinalScoreToastTexts(out toastTitleHome, out toastTitleVisitor, out message);

            try
            {
                notificationService.SendToastNotifications(game.Home.ShortName, "EndScore", toastTitleHome, message);
                notificationService.SendToastNotifications(game.Visitor.ShortName, "EndScore", toastTitleVisitor, message);

                string tileTitle, tileContentHome, tileContentVisitor;

                game.GetLiveTileContents(game.Home.ShortName, out tileTitle, out tileContentHome);
                game.GetLiveTileContents(game.Visitor.ShortName, out tileTitle, out tileContentVisitor);

                notificationService.SendTileNotifications(game.Home.ShortName, tileTitle, tileContentHome);
                notificationService.SendTileNotifications(game.Visitor.ShortName, tileTitle, tileContentVisitor);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Got exception while trying to send notifications: " + ex.ToString());
            }

            Console.WriteLine("\r\n\r\nGame ended!: {0}", message);
        }

        static void HandleGoal(object sender, EventArgs e)
        {
            Game game = (Game)sender;

            UpdateGame(game);

            string toastTitleHome, toastTitleVisitor, message;

            try
            {
                game.Goals.Last().GetGoalToastTexts(game, out toastTitleHome, out toastTitleVisitor, out message);

                notificationService.SendToastNotifications(game.Home.ShortName, "EveryGoal", toastTitleHome, message);
                notificationService.SendToastNotifications(game.Visitor.ShortName, "EveryGoal", toastTitleVisitor, message);

                string tileTitle, tileContentHome, tileContentVisitor;

                game.GetLiveTileContents(game.Home.ShortName, out tileTitle, out tileContentHome);
                game.GetLiveTileContents(game.Visitor.ShortName, out tileTitle, out tileContentVisitor);

                notificationService.SendTileNotifications(game.Home.ShortName, tileTitle, tileContentHome);
                notificationService.SendTileNotifications(game.Visitor.ShortName, tileTitle, tileContentVisitor);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Got exception while trying to send notifications: " + ex.ToString());
            }

            Console.WriteLine(game.Goals.Last().ToString());
        }

        static void UpdateGame(Game game)
        {
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

            using (DataLayerServiceClient dataLayer = new DataLayerServiceClient("BasicHttpBinding_IDataLayerService"))
            {
                var attempts = 0;

                while (attempts++ < 5)
                {
                    try
                    {
                        dataLayer.UpdateGame(game.Number, game.GameTime, game.Home.ShortName, game.Visitor.ShortName, game.HomeGoals, game.VisitorGoals, sqlStartedDateTime, sqlEndedDateTime);
                        break;
                    }
                    catch { }
                }
            }
        }
    }
}
