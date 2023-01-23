using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using HockeyTracker.HockeyTrackerService;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;

namespace HockeyTracker
{
    public partial class TeamNotifications : PhoneApplicationPage
    {
        TeamViewModel viewModel;

        public TeamNotifications()
        {
            InitializeComponent();

            viewModel = new TeamViewModel();

            this.Loaded += new RoutedEventHandler(TeamNotifications_Loaded);
            this.Unloaded += new RoutedEventHandler(TeamNotifications_Unloaded);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.Assert(NavigationContext.QueryString.ContainsKey("Team"));

            string teamShortName = NavigationContext.QueryString["Team"];

            viewModel.Team = App.Data.Teams.Single(t => t.ShortName.Equals(teamShortName, StringComparison.OrdinalIgnoreCase));

            DataContext = viewModel;
        }

        void TeamNotifications_Loaded(object sender, RoutedEventArgs e)
        {
            // Assign the ToggleSwitch'es event handlers here (instead of in the XAML code) 
            // When the UI initializes, it won't trigger the Checked event

            NotifyEndScoreLabel.Checked += new EventHandler<RoutedEventArgs>(NotifyEndScoreLabel_Checked);
            NotifyEndScoreLabel.Unchecked += new EventHandler<RoutedEventArgs>(NotifyEndScoreLabel_Unchecked);

            NotifyEveryGoalLabel.Checked += new EventHandler<RoutedEventArgs>(NotifyEveryGoalLabel_Checked);
            NotifyEveryGoalLabel.Unchecked += new EventHandler<RoutedEventArgs>(NotifyEveryGoalLabel_Unchecked);

            TeamLiveTileLabel.Checked += new EventHandler<RoutedEventArgs>(TeamLiveTileLabel_Checked);
            TeamLiveTileLabel.Unchecked += new EventHandler<RoutedEventArgs>(TeamLiveTileLabel_Unchecked);

            // Notification web service handlers
            App.HTService.SubscribeCompleted += new EventHandler<SubscribeCompletedEventArgs>(notificationService_SubscribeCompleted);
            App.HTService.UnsubscribeCompleted += new EventHandler<AsyncCompletedEventArgs>(notificationService_UnsubscribeCompleted);
            App.HTService.GetLiveTileContentsCompleted += GetLiveTileContentsCompleted;
        }

        void TeamNotifications_Unloaded(object sender, RoutedEventArgs e)
        {
            // Remove event handlers here, or they stick across the multiple instances of this page
            // Ex.: subscribing to a team calls the handler X times if you've opened the page X times before
            App.HTService.SubscribeCompleted -= new EventHandler<SubscribeCompletedEventArgs>(notificationService_SubscribeCompleted);
            App.HTService.UnsubscribeCompleted -= new EventHandler<AsyncCompletedEventArgs>(notificationService_UnsubscribeCompleted);
            App.HTService.GetLiveTileContentsCompleted -= GetLiveTileContentsCompleted;
        }

        void NotifyEndScoreLabel_Checked(object sender, RoutedEventArgs e)
        {
            Subscribe(SubscriptionType.EndScore);
        }

        void NotifyEndScoreLabel_Unchecked(object sender, RoutedEventArgs e)
        {
            Unsubscribe(SubscriptionType.EndScore);
        }

        void NotifyEveryGoalLabel_Checked(object sender, RoutedEventArgs e)
        {
            Subscribe(SubscriptionType.EveryGoal);
        }

        void NotifyEveryGoalLabel_Unchecked(object sender, RoutedEventArgs e)
        {
            Unsubscribe(SubscriptionType.EveryGoal);
        }

        void TeamLiveTileLabel_Checked(object sender, RoutedEventArgs e)
        {
            Subscribe(SubscriptionType.LiveTile);
        }

        void TeamLiveTileLabel_Unchecked(object sender, RoutedEventArgs e)
        {
            Unsubscribe(SubscriptionType.LiveTile);
        }

        /// <summary>
        /// Subscribes the current user for the given type, for this team on the server
        /// </summary>
        void Subscribe(SubscriptionType subscriptionType)
        {
            // When the UI updates from the bound property that has been changed after the Loaded event (ex.: from a web service callback),
            // We shouldn't try to subscribe. The way to detect this would be to see if the event would be a no-op
            if (App.Data.Subscriptions.Any(s => s.Team == viewModel.Team.ShortName && s.SubscriptionType == subscriptionType.ToString()))
                return;

            Subscription subscription = new Subscription
            {
                UserId = -1,
                SubscriptionType = subscriptionType.ToString(),
                Team = viewModel.Team.ShortName
            };

            if (!Tools.IsNetworkAvailable) return;

            SetToggleSwitchState(subscriptionType, false);

            App.HTService.SubscribeAsync(App.Data.AnonymousId, subscription, subscription);
        }

        /// <summary>
        /// Unsubscribes the current user for the given type, for this team on the server
        /// </summary>
        void Unsubscribe(SubscriptionType subscriptionType)
        {
            Subscription subscription = App.Data.Subscriptions.SingleOrDefault(s =>
                s.SubscriptionType == subscriptionType.ToString()
                && s.Team == viewModel.Team.ShortName);

            if (subscription != null)
            {
                if (!Tools.IsNetworkAvailable) return;

                SetToggleSwitchState(subscriptionType, false);

                List<long> unsubscribeIds = new List<long>();
                unsubscribeIds.Add(subscription.Id);

                App.HTService.UnsubscribeAsync(unsubscribeIds, subscription);
            }
        }

        void notificationService_SubscribeCompleted(object sender, SubscribeCompletedEventArgs e)
        {
            Subscription subscription = e.UserState as Subscription;
            SubscriptionType subscriptionType = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), subscription.SubscriptionType, true);

            if (e.Error == null)
            {
                subscription.Id = e.Result;

                if (e.Result > 0)
                {
                    App.Data.Subscriptions.Add(subscription);
                }

                // Create the tile when the subscription is complete only
                if (subscription.SubscriptionType == "LiveTile")
                {
                    App.HTService.GetLiveTileContentsAsync(viewModel.Team.ShortName, (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalMinutes, CultureInfo.CurrentUICulture.Name);
                }
            }
            else
            {
                // If the user enables the live tile quickly after enable another notif whose subscription is still in flight
                // this will create an error, and popup a message when the user taps 'Back'. Avoid this by not reporting errors here.
                // Tools.HandleError(e.Error);
                SetToggleSwitchValue(subscriptionType, false); // Roll-back, as the exception means we didn't commit the subscription
            }

            SetToggleSwitchState(subscriptionType, true);
        }

        void notificationService_UnsubscribeCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Subscription subscription = e.UserState as Subscription;

            if (subscription == null)
            {
                return; // When we unsubscribe on behalf of the user (because he removed the live tile), we send a list of subscription id's to remove and no user state.
                // the App's subscription list will not include the ones to remove, and thus the toggle switch won't be enabled and we don't need this method to disable it.
            }

            SubscriptionType subscriptionType = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), subscription.SubscriptionType, true);

            if (e.Error == null)
            {
                // Pas sur du scenario, mais protection au cas ou
                if (App.Data.Subscriptions.Contains(subscription))
                {
                    App.Data.Subscriptions.Remove(subscription);
                }

                if (subscription.SubscriptionType == SubscriptionType.LiveTile.ToString())
                {
                    RemoveTeamTile();
                }
            }
            else
            {
                // If the user enables the live tile quickly after enable another notif whose subscription is still in flight
                // this will create an error, and popup a message when the user taps 'Back'. Avoid this by not reporting errors here.
                // Tools.HandleError(e.Error);
                SetToggleSwitchValue(subscriptionType, true); // Roll-back, as the exception means we didn't commit the subscription
            }

            SetToggleSwitchState(subscriptionType, true);
        }

        void GetLiveTileContentsCompleted(object sender, GetLiveTileContentsCompletedEventArgs e)
        {
            // This method re-enables the ToggleSwitch on its own right before the tile is created
            if (e.Error == null)
            {
                CreateTeamTile(e.Result);
            }
        }

        void CreateTeamTile(Dictionary<string, string> nextGameTileContents)
        {
            string teamShortName = viewModel.Team.ShortName;

            ShellTile tileToFind =
                        ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("Team=" + teamShortName));

            // Create the Tile if we didn't find that it already exists.
            if (tileToFind == null)
            {
                // Get the information for the back of the tile from what we know

                Game currentGame = App.Data.Games.FirstOrDefault(g =>
                    g.HomeTeam.ShortName == teamShortName ||
                    g.VisitorTeam.ShortName == teamShortName);

                string backTitle;
                string backContent;

                if (currentGame == null && nextGameTileContents.ContainsKey("title") && nextGameTileContents.ContainsKey("content"))
                {
                    backTitle = nextGameTileContents["title"];
                    backContent = nextGameTileContents["content"];
                }
                else
                {
                    currentGame.GetLiveTileContents(teamShortName, out backTitle, out backContent);
                }

                // Now we're done computing the live tile back contents
                SetToggleSwitchState(SubscriptionType.LiveTile, true);

                // Create the Tile and pin it to Start. This will cause a navigation to Start and a deactivation of our application.
                StandardTileData teamTile = new StandardTileData
                {
                    BackgroundImage = new Uri("Images/" + teamShortName + "_173.png", UriKind.Relative),
                    BackBackgroundImage = new Uri("Images/" + teamShortName + "_173_back.png", UriKind.Relative),
                    BackTitle = backTitle,
                    BackContent = backContent
                };

                ShellTile.Create(new Uri("/TeamNotifications.xaml?Team=" + teamShortName, UriKind.Relative), teamTile);
            }
            else
            {
                // Now we're done computing the live tile back contents
                SetToggleSwitchState(SubscriptionType.LiveTile, true);
            }
        }

        void RemoveTeamTile()
        {
            // Look to see whether the Tile already exists
            ShellTile tileToFind = ShellTile.ActiveTiles.FirstOrDefault(x =>
                x.NavigationUri.ToString().Contains("TeamNotifications.xaml?Team=" + viewModel.Team.ShortName));

            // If the Tile was found, then delete it.
            if (tileToFind != null)
            {
                tileToFind.Delete();
            }
        }

        /// <summary>
        /// Controls the state of a toggle switch, i.e. enabled or disabled. Does not change the actual on/off value.
        /// </summary>
        void SetToggleSwitchState(SubscriptionType subscriptionType, bool value)
        {
            if (subscriptionType == SubscriptionType.EveryGoal)
            {
                NotifyEveryGoalLabel.IsEnabled = value;
            }
            else if (subscriptionType == SubscriptionType.EndScore)
            {
                NotifyEndScoreLabel.IsEnabled = value;
            }
            else if (subscriptionType == SubscriptionType.LiveTile)
            {
                TeamLiveTileLabel.IsEnabled = value;
            }
        }

        /// <summary>
        /// Controls the value of a toggle switch
        /// </summary>
        void SetToggleSwitchValue(SubscriptionType subscriptionType, bool value)
        {
            if (subscriptionType == SubscriptionType.EveryGoal)
            {
                NotifyEveryGoalLabel.IsChecked = value;
            }
            else if (subscriptionType == SubscriptionType.EndScore)
            {
                NotifyEndScoreLabel.IsChecked = value;
            }
            else if (subscriptionType == SubscriptionType.LiveTile)
            {
                TeamLiveTileLabel.IsChecked = value;
            }
        }
    }
}