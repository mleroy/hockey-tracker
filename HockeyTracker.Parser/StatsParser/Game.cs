using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using StatsParser.DataLayerService;

namespace StatsParser
{
    class Game
    {
        public enum GameState { NotYetStarted, InProgress, Final };

        public int Number;

        public DateTime GameTime;
        public DateTime Started;
        public DateTime Ended;
        public GameState State;
        public List<Goal> Goals;

        public Team Visitor;
        public Team Home;

        public string Url;

        public delegate void GameStartedEventHandler(object sender, EventArgs e);
        public event GameStartedEventHandler GameStarted;

        public delegate void GoalEventHandler(object sender, EventArgs e);
        public event GoalEventHandler Goal;

        public delegate void GameEndedEventHandler(object sender, EventArgs e);
        public event GameEndedEventHandler GameEnded;

        public Game(ScheduleGame scheduleGame)
        {
            this.Number = scheduleGame.Num;
            this.Url = scheduleGame.Url;
            this.GameTime = scheduleGame.GameTime;

            this.Goals = new List<Goal>();

            Parse();
        }

        public void Parse()
        {
            if (this.State == GameState.Final)
                return;

            string html = Helpers.GetHtml(this.Url);

            if (html == null)
                return;

            HtmlDocument document = new HtmlDocument();

            document.LoadHtml(html);

            #region Visitor vs Home

            if (this.Visitor == null || this.Home == null)
            {
                var visitorNode = document.DocumentNode.SelectSingleNode("//table[@id=\"Visitor\"]/tr[3]/td");

                string visitor = visitorNode.InnerHtml.Substring(0, visitorNode.InnerHtml.IndexOf("<br>")); // Cell is like "edmonton oilers<br>Game ..."

                this.Visitor = Teams.Parse(visitor);

                var homeNode = document.DocumentNode.SelectSingleNode("//table[@id=\"Home\"]/tr[3]/td");

                string home = homeNode.InnerHtml.Substring(0, homeNode.InnerHtml.IndexOf("<br>")); // Cell is like "edmonton oilers<br>Game ..."

                this.Home = Teams.Parse(home);
            }

            #endregion

            #region Goals - Assists

            // Take all rows except the first one which is the header
            var goalsNodes = document.DocumentNode.SelectNodes("//table[@id=\"MainTable\"]/tr[4]/td/table/tr[position()>" + (1 + this.Goals.Count) + "]");

            if (goalsNodes != null)
            {
                for (int i = 0; i < goalsNodes.Count; i++)
                {
                    var lineElements = goalsNodes[i].SelectNodes("./td");

                    int period;
                    if (!int.TryParse(lineElements[1].InnerText, out period))
                    {
                        if (lineElements[1].InnerText == "OT")
                            period = 4;
                        else if (lineElements[1].InnerText == "SO")
                            period = 5;
                    }

                    TimeSpan timeSpan;
                    if (!TimeSpan.TryParse("00:" + lineElements[2].InnerText, out timeSpan))
                    {
                        timeSpan = TimeSpan.Zero; // Can happen for SO
                    }

                    int goalNumber;
                    int.TryParse(lineElements[0].InnerText, out goalNumber);

                    if (goalNumber > 0) // Unsuccessful penalty shots have a goal number of '-'
                    {
                        Goal goal = new Goal
                        {
                            Number = goalNumber,
                            Period = period,
                            Time = timeSpan,
                            Team = Teams.Parse(lineElements[4].InnerText),
                            Scorer = Player.Parse(lineElements[5].InnerText)
                        };

                        // Sometimes the NHL website will have ".(0)" as a placeholder for the scorer name for a while
                        // In that case, Player.Parse returns null, and we should wait until reporting this goal
                        if (goal.Scorer != null)
                        {
                            goal.Assists = new List<Player>();

                            Player assist1 = Player.Parse(lineElements[6].InnerText);
                            Player assist2 = Player.Parse(lineElements[7].InnerText);

                            if (assist1 != null)
                            {
                                goal.Assists.Add(assist1);

                                if (assist2 != null)
                                {
                                    goal.Assists.Add(assist2);
                                }
                            }

                            if (assist1 != null || lineElements[6].InnerText.Equals("unassisted") || lineElements[6].InnerText.Equals("Penalty Shot") || period == 5)
                            {
                                // No player name in the Assist1 column was found
                                // This may be because it's "unassisted", or because the NHL website hasn't reported who assisted yet
                                // in which case we should wait before saving this goal
                                // In SO, there's no assist and the assit column is left blank

                                this.Goals.Add(goal);

                                if (Goal != null)
                                {
                                    Goal(this, EventArgs.Empty);
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            // Analyze game info table after we checked for goals, in order to send goal notifications before game ended (for SO/OT situations)
            #region GameInfo table: start, end, game state

            var gameInfoNodes = document.DocumentNode.SelectNodes("//table[@id=\"GameInfo\"]/tr/td");

            if (gameInfoNodes != null)
            {
                DateTime gameDay = DateTime.Parse(gameInfoNodes[3].InnerText);

                Match gameTimeMatch = Regex.Match(HttpUtility.HtmlDecode(gameInfoNodes[5].InnerText),
                    "(Début/)?Start\\s(?<StartedHour>[0-9]{1,2}):(?<StartedMinutes>[0-9]{2})\\s(?<StartedTZ>[A-Z]{3})(?<EndBlock>;\\s" +
                    "(Fin/)?End\\s(?<EndedHour>[0-9]{1,2}):(?<EndedMinutes>[0-9]{2})\\s(?<EndedTZ>[A-Z]{3}))?");

                if (gameTimeMatch.Success)
                {
                    int startedHour = int.Parse(gameTimeMatch.Groups["StartedHour"].Value);
                    int startedMinute = int.Parse(gameTimeMatch.Groups["StartedMinutes"].Value);

                    int endedHour = 0;
                    int endedMinute = 0;

                    if (gameTimeMatch.Groups["EndBlock"].Success)
                    {
                        endedHour = int.Parse(gameTimeMatch.Groups["EndedHour"].Value);
                        endedMinute = int.Parse(gameTimeMatch.Groups["EndedMinutes"].Value);
                    }

                    TimeSpan timeZone;

                    switch (gameTimeMatch.Groups["StartedTZ"].Value)
                    {
                        case "EET":
                            timeZone = new TimeSpan(2, 0, 0);
                            break;
                        case "EDT":
                            timeZone = new TimeSpan(-4, 0, 0);
                            break;
                        case "EST":
                            timeZone = new TimeSpan(-5, 0, 0);
                            break;
                        case "CDT":
                            timeZone = new TimeSpan(-5, 0, 0);
                            break;
                        case "CST":
                            timeZone = new TimeSpan(-6, 0, 0);
                            break;
                        case "MDT":
                            timeZone = new TimeSpan(-6, 0, 0);
                            break;
                        case "MST":
                            timeZone = new TimeSpan(-7, 0, 0);
                            break;
                        case "PDT":
                            timeZone = new TimeSpan(-7, 0, 0);
                            break;
                        case "PST":
                            timeZone = new TimeSpan(-8, 0, 0);
                            break;
                        default:
                            timeZone = new TimeSpan(0, 0, 0);
                            break;
                    }

                    // Convert to PM -- this doesn't work for AM games
                    if (startedHour < 12)
                    {
                        startedHour += 12;
                    }

                    if (endedHour < 12)
                    {
                        endedHour += 12;
                    }

                    if (this.Started == DateTime.MinValue)
                    {
                        this.Started = new DateTime(gameDay.Year, gameDay.Month, gameDay.Day, startedHour, startedMinute, 0);
                        this.Started = this.Started.AddHours(-timeZone.Hours);

                        this.State = GameState.InProgress;

                        if (GameStarted != null)
                        {
                            GameStarted(this, EventArgs.Empty);
                        }
                    }

                    if (gameTimeMatch.Groups["EndBlock"].Success)
                    {
                        this.Ended = new DateTime(gameDay.Year, gameDay.Month, gameDay.Day, endedHour, endedMinute, 0);
                        this.Ended = this.Ended.AddHours(-timeZone.Hours);
                    }
                }

                // Parsing the game number is tricky and not necessary:
                // First, because we build the URL based on the game number, so it must match what we already know
                // Also because they give out the last 4 digits, but not 1-2-3 to identify preseason-season-series games
                //Match gameNumberMatch = Regex.Match(gameInfoNodes[6].InnerText, "(Match/)?Game (?<GameNumber>[0-9]{4})");

                //if (gameNumberMatch.Success)
                //{
                //    this.Number = int.Parse("03" + gameNumberMatch.Groups["GameNumber"].Value);
                //}

                // Update to "Final" only if the web page shows an end time as well and we could parse it
                // Also, avoid updating to Final if the score is still equal; this means the NHL hasn't fully updated their page yet and it's missing the OT/SO goal
                if (gameInfoNodes[7].InnerText == "Final" && this.Ended != DateTime.MinValue && this.VisitorGoals != this.HomeGoals)
                {
                    this.State = GameState.Final;

                    if (GameEnded != null)
                    {
                        GameEnded(this, EventArgs.Empty);
                    }
                }
                else if (this.Started == DateTime.MinValue)
                {
                    this.State = GameState.NotYetStarted;
                }
                else
                {
                    this.State = GameState.InProgress;
                }
            }

            #endregion
        }

        public int HomeGoals
        {
            get
            {
                return this.Goals.Count(g => g.Team == this.Home);
            }
        }

        public int VisitorGoals
        {
            get
            {
                return this.Goals.Count(g => g.Team == this.Visitor);
            }
        }

        public string GetResults()
        {
            int homeGoals = HomeGoals;
            int visitorGoals = VisitorGoals;

            string status = State == GameState.Final ? "won" : "is winning";

            if (homeGoals == visitorGoals)
            {
                status = "is tied";
            }

            if (homeGoals > visitorGoals)
            {
                return string.Format("{0} {1} {2}-{3} against {4}", Home.Locale, status, homeGoals, visitorGoals, Visitor.Locale);
            }
            else
            {
                return string.Format("{0} {1} {2}-{3} against {4}", Visitor.Locale, status, visitorGoals, homeGoals, Home.Locale);
            }
        }

        public void GetFinalScoreToastTexts(out string titleHome, out string titleVisitor, out string message)
        {
            if (this.State != GameState.Final)
            {
                throw new Exception("Game is not over yet!");
            }

            int homeGoals = HomeGoals;
            int visitorGoals = VisitorGoals;

            string winExtraText = string.Empty;
            string loseExtraText = string.Empty;

            switch (this.Goals.Last().Period)
            {
                case 4:
                    winExtraText = " IN OT";
                    loseExtraText = " in OT";
                    break;
                case 5:
                    winExtraText = " IN SO";
                    loseExtraText = " in SO";
                    break;
            }

            if (homeGoals > visitorGoals)
            {
                titleHome = string.Format("{0} WIN{1}!", Home.Name, winExtraText);
                titleVisitor = string.Format("{0} lose{1}", Visitor.Name, loseExtraText);
            }
            else
            {
                titleHome = string.Format("{0} lose{1}", Home.Name, loseExtraText);
                titleVisitor = string.Format("{0} WIN{1}!", Visitor.Name, winExtraText);
            }

            message = string.Format("{0} {1} - {2} {3}", Home.ShortName, homeGoals, Visitor.ShortName, visitorGoals);
        }
        
        /// <summary>
        /// Builds the strings for the back of the live tile for the team in the context of this game
        /// </summary>
        public void GetLiveTileContents(string teamShortName, out string backTitle, out string backContent)
        {
            string smileyText = ":-(";

            if (this.VisitorGoals == this.HomeGoals)
            {
                smileyText = ":-o";
            }
            else if (
                (this.HomeGoals > this.VisitorGoals && teamShortName == this.Home.ShortName) ||
                (this.HomeGoals < this.VisitorGoals && teamShortName == this.Visitor.ShortName))
            {
                smileyText = ":-)";
            }

            if (this.State == GameState.InProgress)
            {
                backTitle = "in progress";
                backContent =
                    string.Format("{0} {1} @{2}{3} {4}{5}{6}",
                        this.Visitor.ShortName,
                        this.VisitorGoals,
                        Environment.NewLine,
                        this.Home.ShortName,
                        this.HomeGoals,
                        Environment.NewLine,
                        smileyText);
            }
            else if (this.State == GameState.Final)
            {
                backTitle = "game ended";
                backContent =
                    string.Format("{0} {1} @{2}{3} {4}{5}{6}",
                        this.Visitor.ShortName,
                        this.VisitorGoals,
                        Environment.NewLine,
                        this.Home.ShortName,
                        this.HomeGoals,
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

        public override string ToString()
        {
            string output = string.Format(
                "Game #{0}: {1} vs {2}" + Environment.NewLine +
                "Started: {3} Ended: {4}" + Environment.NewLine +
                "State: {5}", Number, Visitor, Home,
                Started == DateTime.MinValue ? "Not yet" : Started.ToString(),
                Ended == DateTime.MinValue ? "Not yet" : Ended.ToString(), State);

            output += Environment.NewLine + Environment.NewLine;

            output += "Goals:" + Environment.NewLine;

            foreach (Goal goal in this.Goals)
            {
                output += "\t" + goal.ToString() + Environment.NewLine;
            }

            return output;
        }
    }
}
