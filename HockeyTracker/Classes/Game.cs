using System;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;

namespace HockeyTracker
{
    public enum GameState { NotYetStarted, InProgress, Final };

    public class Game
    {
        public Team HomeTeam { get; set; }
        public Team VisitorTeam { get; set; }

        public int HomeScore { get; set; }
        public int VisitorScore { get; set; }

        public DateTime GameTime { get; set; }

        public DateTime? Started { get; set; }
        public DateTime? Ended { get; set; }

        public GameState State
        {
            get
            {
                if (this.Ended != null)
                {
                    return GameState.Final;
                }

                if (this.Started != null)
                {
                    return GameState.InProgress;
                }

                return GameState.NotYetStarted;
            }
        }

        public static Game FromNotificationServiceGame(HockeyTrackerService.Game game)
        {
            return new Game
            {
                Started = game.Started,
                Ended = game.Ended,
                HomeTeam = App.Data.Teams.Single(t => t.ShortName == game.HomeTeam),
                VisitorTeam = App.Data.Teams.Single(t => t.ShortName == game.VisitorTeam),
                HomeScore = game.HomeGoals,
                VisitorScore = game.VisitorGoals,
                GameTime = game.GameTime
            };
        }

        /// <summary>
        /// Builds the strings for the back of the live tile for the team in the context of this game
        /// </summary>
        public void GetLiveTileContents(string teamShortName, out string backTitle, out string backContent)
        {
            string smileyText = ":-(";

            if (this.VisitorScore == this.HomeScore)
            {
                smileyText = ":-o";
            }
            else if (
                (this.HomeScore > this.VisitorScore && teamShortName == this.HomeTeam.ShortName) ||
                (this.HomeScore < this.VisitorScore && teamShortName == this.VisitorTeam.ShortName))
            {
                smileyText = ":-)";
            }

            if (this.State == GameState.NotYetStarted)
            {
                backTitle = "next game";
                backContent = string.Format("{0} @{1}{2}{3}{4}{5}{6}",
                    this.VisitorTeam.ShortName,
                    Environment.NewLine,
                    this.HomeTeam.ShortName,
                    Environment.NewLine,
                    this.GameTime.ToFriendlyDate(),
                    Environment.NewLine,
                    this.GameTime.ToFriendlyTime());
            }
            else if (this.State == GameState.InProgress)
            {
                backTitle = "in progress";
                backContent =
                    string.Format("{0} {1} @{2}{3} {4}{5}{6}",
                        this.VisitorTeam.ShortName,
                        this.VisitorScore,
                        Environment.NewLine,
                        this.HomeTeam.ShortName,
                        this.HomeScore,
                        Environment.NewLine,
                        smileyText);
            }
            else if (this.State == GameState.Final)
            {
                backTitle = "game ended";
                backContent =
                    string.Format("{0} {1} @{2}{3} {4}{5}{6}",
                        this.VisitorTeam.ShortName,
                        this.VisitorScore,
                        Environment.NewLine,
                        this.HomeTeam.ShortName,
                        this.HomeScore,
                        Environment.NewLine,
                        smileyText);
            }
            else
            {
                // Should never get here
                backTitle = string.Empty;
                backContent = string.Empty;
            }
        }
    }

    /// <summary>
    /// This is a hack for as long as we can't bind a field to a ConverterParameter
    /// Problem is that a field is bound to a 'Game' object, but the object has no context of which team it's currently
    /// being displayed for. And since we can't bind the team object as a parameter to the binding, we need a custom game object
    /// that explains the score from the perspective of a team.
    /// </summary>
    public class TeamLiveGame
    {
        public DateTime GameTime;

        public Team OtherTeam;

        public int MyScore;
        public int OtherScore;

        public DateTime? Started;
        public DateTime? Ended;

        public GameState State
        {
            get
            {
                if (this.Ended != null)
                {
                    return GameState.Final;
                }

                if (this.Started != null)
                {
                    return GameState.InProgress;
                }

                return GameState.NotYetStarted;
            }
        }
    }
}
