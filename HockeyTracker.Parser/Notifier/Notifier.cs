using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using HockeyTracker.Data;
using System.Threading.Tasks;
using System.Globalization;
using HockeyTracker.Localization;

namespace HockeyTracker.Notifier
{
    public class Notifier
    {
        public static void SendTileNotifications(string teamShortName, string tileTitle, string tileContent)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                var usersToNotify = context.GetUsersForNotification(teamShortName, "LiveTile");

                ServicePointManager.DefaultConnectionLimit = 30;

                foreach (var user in usersToNotify)
                {
                    NotificationsHistory n = new NotificationsHistory { UserId = user.Id, Title = tileTitle, Message = tileContent };

                    SendTileNotification(user.ChannelUri, teamShortName, tileTitle, tileContent, n);

                    context.NotificationsHistories.InsertOnSubmit(n);

                    ManageNotificationResponse(n);
                }

                context.SubmitChanges();
            }
        }

        /// <summary>
        /// The text-generation logic lives here as it requires knowledge of each user's local utc offset, and we iterate over here users here
        /// </summary>
        public static void SendTileNotificationsForNextGame(string teamShortName)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                var usersToNotify = context.GetUsersForNotification(teamShortName, "LiveTile");

                ServicePointManager.DefaultConnectionLimit = 30;

                var games =
                    from game in context.Games
                    where (game.HomeTeam == teamShortName || game.VisitorTeam == teamShortName)
                        && game.GameTime >= DateTime.UtcNow
                    orderby game.GameNumber ascending
                    select game;

                Game nextGame = games.FirstOrDefault();

                foreach (var user in usersToNotify)
                {
                    string tileTitle = string.Empty;
                    string tileContent = string.Empty;

                    var cultureInfo = new CultureInfo("en-US");

                    if (nextGame != null)
                    {
                        tileTitle = LocalizedStrings.Get(Strings.NextGame, cultureInfo);
                        tileContent = string.Format("{0} @{1}{2}{3}{4}{5}{6}",
                            nextGame.VisitorTeam,
                            Environment.NewLine,
                            nextGame.HomeTeam,
                            Environment.NewLine,
                            nextGame.GameTime.ToFriendlyDate(user.LocalUtcOffset, cultureInfo),
                            Environment.NewLine,
                            nextGame.GameTime.ToFriendlyTime(user.LocalUtcOffset, cultureInfo));
                    }

                    NotificationsHistory n = new NotificationsHistory { UserId = user.Id, Title = tileTitle, Message = tileContent };

                    SendTileNotification(user.ChannelUri, teamShortName, tileTitle, tileContent, n);

                    context.NotificationsHistories.InsertOnSubmit(n);

                    ManageNotificationResponse(n);
                }

                context.SubmitChanges();
            }
        }

        public static void SendToastNotifications(string teamShortName, string subscriptionType, string toastTitle, string toastMessage)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                var usersToNotify = context.GetUsersForNotification(teamShortName, subscriptionType);

                ServicePointManager.DefaultConnectionLimit = 30;

                //Parallel.For(0, usersToNotify.Count, delegate(int i)
                //{
                //    User user = usersToNotify[i];

                //    NotificationsHistory n = new NotificationsHistory { UserId = user.Id, Title = title, Message = message };

                //    SendNotification(user.ChannelUri, title, message, n);

                //    context.NotificationsHistories.InsertOnSubmit(n);

                //    ManageNotificationResponse(n);
                //});

                foreach (var user in usersToNotify)
                {
                    NotificationsHistory n = new NotificationsHistory { UserId = user.Id, Title = toastTitle, Message = toastMessage };

                    SendNotification(user.ChannelUri, toastTitle, toastMessage, n);

                    context.NotificationsHistories.InsertOnSubmit(n);

                    ManageNotificationResponse(n);
                }

                context.SubmitChanges();
            }
        }

        private static void ManageNotificationResponse(NotificationsHistory n)
        {
            if (n.NotificationStatus == "Dropped" && n.SubscriptionStatus == "Expired")
            {
                // Drop client
                using (DataLayerDataContext context = new DataLayerDataContext())
                {
                    context.Subscriptions.DeleteAllOnSubmit(context.Subscriptions.Where(s => s.UserId == n.UserId));
                    context.Users.Single(u => u.Id == n.UserId).Active = false;
                    context.SubmitChanges();
                }
            }
        }

        static void SendTileNotification(string channelUri, string teamShortName, string backTitle, string backContent, NotificationsHistory n)
        {
            try
            {
                HttpWebRequest sendNotificationRequest = (HttpWebRequest)WebRequest.Create(channelUri);

                // Create an HTTPWebRequest that posts the Tile notification to the Microsoft Push Notification Service.
                // HTTP POST is the only method allowed to send the notification.
                sendNotificationRequest.Method = "POST";

                // The optional custom header X-MessageID uniquely identifies a notification message. 
                // If it is present, the same value is returned in the notification response. It must be a string that contains a UUID.
                // sendNotificationRequest.Headers.Add("X-MessageID", "<UUID>");

                // Create the Tile message.
                string tileMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<wp:Notification xmlns:wp=\"WPNotification\">" +
                    "<wp:Tile ID=\"/TeamNotifications.xaml?Team=" + teamShortName + "\">" +
                      "<wp:BackgroundImage>Images\\" + teamShortName + "_173.png</wp:BackgroundImage>" +
                      "<wp:Count>0</wp:Count>" +
                      "<wp:Title></wp:Title>" +
                      "<wp:BackBackgroundImage>Images\\" + teamShortName + "_173_back.png</wp:BackBackgroundImage>" +
                      "<wp:BackTitle>" + backTitle + "</wp:BackTitle>" +
                      "<wp:BackContent>" + backContent + "</wp:BackContent>" +
                   "</wp:Tile> " +
                "</wp:Notification>";

                // Set the notification payload to send.
                byte[] notificationMessage = Encoding.Default.GetBytes(tileMessage);

                // Set the web request content length.
                sendNotificationRequest.ContentLength = notificationMessage.Length;
                sendNotificationRequest.ContentType = "text/xml";
                sendNotificationRequest.Headers.Add("X-WindowsPhone-Target", "token");
                sendNotificationRequest.Headers.Add("X-NotificationClass", "1");

                using (Stream requestStream = sendNotificationRequest.GetRequestStream())
                {
                    requestStream.Write(notificationMessage, 0, notificationMessage.Length);
                }

                // Send the notification and get the response.
                HttpWebResponse response = (HttpWebResponse)sendNotificationRequest.GetResponse();

                n.StatusCode = response.StatusCode.ToString();
                n.NotificationStatus = response.Headers["X-NotificationStatus"];
                n.SubscriptionStatus = response.Headers["X-SubscriptionStatus"];
                n.DeviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];
                n.Time = DateTime.Now;
            }
            catch (WebException e)
            {
                n.StatusCode = "Error";
                if (e.Response != null && e.Response.Headers != null)
                {
                    n.NotificationStatus = e.Response.Headers["X-NotificationStatus"];
                    n.SubscriptionStatus = e.Response.Headers["X-SubscriptionStatus"];
                    n.DeviceConnectionStatus = e.Response.Headers["X-DeviceConnectionStatus"];
                }
                n.Time = DateTime.Now;
            }
        }

        private static void SendNotification(string channelUri, string title, string message, NotificationsHistory n)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(channelUri);
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.Headers.Add("X-NotificationClass", "2");
            request.Headers.Add("X-WindowsPhone-Target", "toast");

            string notificationData =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<wp:Notification xmlns:wp=\"WPNotification\">" +
                   "<wp:Toast>" +
                      "<wp:Text1>" + title + "</wp:Text1>" +
                      "<wp:Text2>" + message + "</wp:Text2>" +
                   "</wp:Toast>" +
                "</wp:Notification>";

            byte[] contents = Encoding.Default.GetBytes(notificationData);

            request.ContentLength = contents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(contents, 0, contents.Length);
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    n.StatusCode = response.StatusCode.ToString();
                    n.NotificationStatus = response.Headers["X-NotificationStatus"];
                    n.SubscriptionStatus = response.Headers["X-SubscriptionStatus"];
                    n.DeviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];
                    n.Time = DateTime.Now;
                }
            }
            catch (WebException e)
            {
                n.StatusCode = "Error";
                if (e.Response != null && e.Response.Headers != null)
                {
                    n.NotificationStatus = e.Response.Headers["X-NotificationStatus"];
                    n.SubscriptionStatus = e.Response.Headers["X-SubscriptionStatus"];
                    n.DeviceConnectionStatus = e.Response.Headers["X-DeviceConnectionStatus"];
                }
                n.Time = DateTime.Now;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        #region For Admin page only

        public static void SendTileNotification(long userId, string teamShortName, string backTitle, string backContent)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                NotificationsHistory n = new NotificationsHistory { UserId = userId, Title = backTitle, Message = backContent };

                User user = context.Users.Single(u => u.Id == userId);

                SendTileNotification(user.ChannelUri, teamShortName, backTitle, backContent, n);

                ManageNotificationResponse(n);

                context.NotificationsHistories.InsertOnSubmit(n);
                context.SubmitChanges();
            }
        }

        public static void SendNotification(long userId, string title, string message)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                NotificationsHistory n = new NotificationsHistory { UserId = userId, Title = title, Message = message };

                User user = context.Users.Single(u => u.Id == userId);

                SendNotification(user.ChannelUri, title, message, n);

                ManageNotificationResponse(n);

                context.NotificationsHistories.InsertOnSubmit(n);
                context.SubmitChanges();
            }
        }

        #endregion
    }
}
