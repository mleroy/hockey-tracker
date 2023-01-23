using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using HockeyTracker.Data;
using System.Globalization;
using HockeyTracker.Localization;

namespace NotificationService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class HockeyTrackerService
    {
        [OperationContract]
        public long Subscribe(Guid anonymousId, Subscription subscription)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                User user = context.Users.FirstOrDefault(d => d.AnId == anonymousId);

                // If a user subscribes before RegisterUser could commit his anonymousId, we won't have a UserId for him
                // We could create one here, but then potentially create a concurrency issue with RegisterUser
                // Since this should be rare, we don't handle this case and the user won't get registered
                if (user != null)
                {
                    subscription.UserId = user.Id;

                    if (!context.Subscriptions.Any(s => s.UserId == subscription.UserId && s.SubscriptionType == subscription.SubscriptionType && s.Team == subscription.Team))
                    {
                        context.Subscriptions.InsertOnSubmit(subscription);
                        context.SubmitChanges();

                        return subscription.Id;
                    }
                }

                return 0;
            }
        }

        [OperationContract]
        public void Unsubscribe(List<long> subscriptionIds)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                foreach (long subscriptionId in subscriptionIds)
                {
                    Subscription subscriptionToDelete =
                        context.Subscriptions.SingleOrDefault(s => s.Id == subscriptionId);

                    if (subscriptionToDelete != null)
                    {
                        context.Subscriptions.DeleteOnSubmit(subscriptionToDelete);
                        context.SubmitChanges();
                    }
                }
            }
        }

        [OperationContract]
        public long RegisterUser(Guid anonymousId, int utcOffsetInMinutes, string channelUri, string culture)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                User user = context.Users.FirstOrDefault(d => d.AnId == anonymousId);

                if (user == null)
                {
                    user = new User
                    {
                        AnId = anonymousId,
                        FirstUpdated = DateTime.Now
                    };

                    context.Users.InsertOnSubmit(user);
                }

                // Update new or existing user if necessary
                if (user.ChannelUri != channelUri)
                {
                    user.ChannelUri = channelUri;
                }

                user.Culture = "en-US"; // starting from version 1.7, the culture is sent as NULL. haven't looked at why yet.

                user.LocalUtcOffset = utcOffsetInMinutes;
                user.LastUpdated = DateTime.Now;
                user.Active = true;

                context.SubmitChanges();

                return user.Id;
            }
        }

        /// <summary>
        /// Retrieves the subcriptions for a given unique device id
        /// </summary>
        [OperationContract]
        public List<Subscription> GetSubscriptions(Guid anId)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                List<Subscription> subscriptions = new List<Subscription>();

                User user = context.Users.SingleOrDefault(u => u.AnId == anId);

                // User can be null if it's a new user, and if the call to RegisterUser hasn't reached the device before it made this request
                if (user != null)
                {
                    subscriptions = context.Subscriptions.Where(s => s.UserId == user.Id).ToList();
                }

                return subscriptions;
            }
        }

        [OperationContract]
        public List<Game> GetLiveGames()
        {
            // GameTime is stored as UTC
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                // this will return future games too, if the db ever contains them.
                List<Game> liveGames = (from game in context.Games
                                        let timeDifference = (DateTime.UtcNow - game.GameTime).TotalHours
                                        where timeDifference <= 17 && timeDifference >= -2
                                        select game).ToList();

                return liveGames;
            }
        }

        [OperationContract]
        public Dictionary<string, string> GetLiveTileContents(string teamShortName, int localUtcOffset, string culture)
        {
            var tileContents = new Dictionary<string, string>();

            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                var games =
                    from game in context.Games
                    where (game.HomeTeam == teamShortName || game.VisitorTeam == teamShortName)
                        && game.GameTime >= DateTime.UtcNow
                    orderby game.GameNumber ascending
                    select game;

                Game nextGame = games.FirstOrDefault();

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
                        nextGame.GameTime.ToFriendlyDate(localUtcOffset, cultureInfo),
                        Environment.NewLine,
                        nextGame.GameTime.ToFriendlyTime(localUtcOffset, cultureInfo));

                    tileContents.Add("title", tileTitle);
                    tileContents.Add("content", tileContent);
                }
            }

            return tileContents;
        }
    }
}
