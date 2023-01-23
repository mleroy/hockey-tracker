using System;
using System.Collections.ObjectModel;
using System.Linq;
using HockeyTracker.HockeyTrackerService;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;
using System.Collections.Generic;

namespace HockeyTracker
{
    public class AppData
    {
        public ObservableCollection<Game> Games { get; private set; }
        public ObservableCollection<Team> Teams { get; private set; }

        public ObservableCollection<Subscription> Subscriptions { get; private set; }

        public Guid AnonymousId
        {
            get
            {
                if (anonymousId == Guid.Empty)
                {
                    anonymousId = Tools.GetAnId();
                }
                return anonymousId;
            }
        }
        Guid anonymousId;

        [Flags]
        public enum ComponentToLoad
        {
            None = 0,
            Games = 1,
            Subscriptions = 2,
            All = Games + Subscriptions
        }

        /// <summary>
        /// Holds the current set of loaded components
        /// </summary>
        public ComponentToLoad ComponentsLoaded;

        /// <summary>
        /// Event fired each time a component is loaded
        /// </summary>
        public event ComponentLoadedEventHandler ComponentLoaded;

        public delegate void ComponentLoadedEventHandler(object sender, ComponentLoadedEventArgs eventArgs);

        public class ComponentLoadedEventArgs : EventArgs
        {
            public ComponentToLoad ComponentToLoad;
            public ComponentLoadedEventArgs(ComponentToLoad componentToLoad) { this.ComponentToLoad = componentToLoad; }
        }

        public bool IsDataLoaded
        {
            get
            {
                return ComponentsLoaded == ComponentToLoad.All;
            }
        }

        public AppData()
        {
            this.Games = new ObservableCollection<Game>();
            this.Teams = new ObservableCollection<Team>();
            this.Subscriptions = new ObservableCollection<Subscription>();

            ComponentLoaded += new ComponentLoadedEventHandler(AppData_ComponentLoaded);

            App.HTService.GetLiveGamesCompleted += new EventHandler<GetLiveGamesCompletedEventArgs>(NotificationService_GetLiveGamesCompleted);
            App.HTService.GetSubscriptionsCompleted += new EventHandler<GetSubscriptionsCompletedEventArgs>(NotificationService_GetSubscriptionsCompleted);
        }

        void AppData_ComponentLoaded(object sender, AppData.ComponentLoadedEventArgs eventArgs)
        {
            if (eventArgs.ComponentToLoad == ComponentToLoad.None)
            {
                ComponentsLoaded = ComponentToLoad.None;
            }
            else
            {
                ComponentsLoaded |= eventArgs.ComponentToLoad;
            }
        }

        public void LoadData()
        {
            LoadOnDemandData();
            LoadTeams();
        }

        void LoadOnDemandData()
        {
            if (!Tools.IsNetworkAvailable) return;

            ComponentLoaded(this, new ComponentLoadedEventArgs(ComponentToLoad.None));

            LoadSubscriptions();
            LoadGames();
        }

        void LoadGames()
        {
            App.HTService.GetLiveGamesAsync();
        }

        void LoadSubscriptions()
        {
            App.HTService.GetSubscriptionsAsync(this.AnonymousId);
        }

        void NotificationService_GetLiveGamesCompleted(object sender, GetLiveGamesCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.Games.Clear();

                foreach (HockeyTrackerService.Game game in e.Result)
                {
                    this.Games.Add(Game.FromNotificationServiceGame(game));

                    TeamLiveGame currentGameForHomeTeam = new TeamLiveGame
                    {
                        GameTime = game.GameTime,
                        OtherTeam = this.Teams.Single(t => t.ShortName == game.VisitorTeam),
                        MyScore = game.HomeGoals,
                        OtherScore = game.VisitorGoals,
                        Started = game.Started,
                        Ended = game.Ended
                    };

                    TeamLiveGame currentGameForVisitorTeam = new TeamLiveGame
                    {
                        GameTime = game.GameTime,
                        OtherTeam = this.Teams.Single(t => t.ShortName == game.HomeTeam),
                        MyScore = game.VisitorGoals,
                        OtherScore = game.HomeGoals,
                        Started = game.Started,
                        Ended = game.Ended
                    };

                    this.Teams.Single(t => t.ShortName == game.HomeTeam).CurrentGame = currentGameForHomeTeam;
                    this.Teams.Single(t => t.ShortName == game.VisitorTeam).CurrentGame = currentGameForVisitorTeam;
                }
            }
            else { Tools.HandleError(e.Error); }

            ComponentLoaded(this, new ComponentLoadedEventArgs(ComponentToLoad.Games));
        }

        void NotificationService_GetSubscriptionsCompleted(object sender, GetSubscriptionsCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.Subscriptions.Clear();

                // If user unpinned tiles (vs turning off the Live Tile notifications), the server won't know about it
                // Here, for each known subscription, double-check that the tile still exists. If it doesn't, unsubscribe on behalf of the user
                List<long> subscriptionsToRemove = new List<long>();

                foreach (Subscription subscription in e.Result)
                {
                    if (subscription.SubscriptionType == "LiveTile" &&
                        !ShellTile.ActiveTiles.Any(x => x.NavigationUri.ToString().Contains("Team=" + subscription.Team)))
                    {
                        subscriptionsToRemove.Add(subscription.Id);   
                    }
                    else
                    {
                        this.Subscriptions.Add(subscription);
                    }
                }

                if (subscriptionsToRemove.Count > 0)
                {
                    App.HTService.UnsubscribeAsync(subscriptionsToRemove);
                }
            }
            else { Tools.HandleError(e.Error); }

            ComponentLoaded(this, new ComponentLoadedEventArgs(ComponentToLoad.Subscriptions));
        }

        void LoadTeams()
        {
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Pacific, Locale = "Anaheim", Name = "Ducks", ShortName = "ANA" });
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Pacific, Locale = "Calgary", Name = "Flames", ShortName = "CGY" });
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Pacific, Locale = "Edmonton", Name = "Oilers", ShortName = "EDM" });
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Pacific, Locale = "Los Angeles", Name = "Kings", ShortName = "LAK" });
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Pacific, Locale = "Arizona", Name = "Coyotes", ShortName = "ARI" });
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Pacific, Locale = "San Jose", Name = "Sharks", ShortName = "SJS" });
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Pacific, Locale = "Vancouver", Name = "Canucks", ShortName = "VAN" });

            Teams.Add(new Team { Conference = Conference.West, Division = Division.Central, Locale = "Chicago", Name = "Blackhawks", ShortName = "CHI" });
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Central, Locale = "Colorado", Name = "Avalanche", ShortName = "COL" });
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Central, Locale = "Dallas", Name = "Stars", ShortName = "DAL" });
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Central, Locale = "Minnesota", Name = "Wild", ShortName = "MIN" });
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Central, Locale = "Nashville", Name = "Predators", ShortName = "NSH" });
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Central, Locale = "St. Louis", Name = "Blues", ShortName = "STL" });
            Teams.Add(new Team { Conference = Conference.West, Division = Division.Central, Locale = "Winnipeg", Name = "Jets", ShortName = "WPG" });

            Teams.Add(new Team { Conference = Conference.East, Division = Division.Atlantic, Locale = "Boston", Name = "Bruins", ShortName = "BOS" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Atlantic, Locale = "Buffalo", Name = "Sabres", ShortName = "BUF" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Atlantic, Locale = "Detroit", Name = "Red Wings", ShortName = "DET" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Atlantic, Locale = "Florida", Name = "Panthers", ShortName = "FLA" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Atlantic, Locale = "Montreal", Name = "Canadiens", ShortName = "MTL" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Atlantic, Locale = "Ottawa", Name = "Senators", ShortName = "OTT" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Atlantic, Locale = "Tampa Bay", Name = "Lightning", ShortName = "TBL" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Atlantic, Locale = "Toronto", Name = "Maple Leafs", ShortName = "TOR" });

            Teams.Add(new Team { Conference = Conference.East, Division = Division.Metropolitan, Locale = "Carolina", Name = "Hurricanes", ShortName = "CAR" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Metropolitan, Locale = "Columbus", Name = "Blue Jackets", ShortName = "CBJ" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Metropolitan, Locale = "New Jersey", Name = "Devils", ShortName = "NJD" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Metropolitan, Locale = "New York", Name = "Islanders", ShortName = "NYI" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Metropolitan, Locale = "New York", Name = "Rangers", ShortName = "NYR" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Metropolitan, Locale = "Philadelphia", Name = "Flyers", ShortName = "PHI" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Metropolitan, Locale = "Pittsburgh", Name = "Penguins", ShortName = "PIT" });
            Teams.Add(new Team { Conference = Conference.East, Division = Division.Metropolitan, Locale = "Washington", Name = "Capitals", ShortName = "WSH" });
        }
    }
}