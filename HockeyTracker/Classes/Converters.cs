using System;
using System.Linq;
using System.Windows.Data;
using System.Collections.Generic;
using System.Windows;

namespace HockeyTracker
{
    public class TeamImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string shortName = (string)value;

            string image = "Images/none.png";

            if (!string.IsNullOrEmpty(shortName))
            {
                image = "Images/" + shortName + ".png";
            }

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TeamDivisionVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string teamShortName = (string)value;

            Team team = App.Data.Teams.Single(t => t.ShortName == teamShortName);

            // Division string visible only if it's the first team in a division
            return
                App.Data.Teams.Where(t => t.Division == team.Division).OrderBy(t => t.Locale).First().ShortName == team.ShortName
                ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TeamDivisionTextTranslator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Division division = (Division)Enum.Parse(typeof(Division), value.ToString(), true);

            switch (division)
            {
                case Division.Atlantic:
                    return AppResources.DivisionAtlantic;
                case Division.Metropolitan:
                    return AppResources.DivisionMetropolitan;
                case Division.Central:
                    return AppResources.DivisionCentral;
                case Division.Pacific:
                default:
                    return AppResources.DivisionPacific;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public class SampleFinalScoreNotificationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Team team = (Team)value;

            return string.Format(AppResources.SampleFinalScore, team.Name, team.ShortName, team.ShortName == "TOR" ? "BOS" : "TOR");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GameStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TeamLiveGame game = (TeamLiveGame)value;

            string line = (string)parameter;

            string winLose = string.Empty;

            if (game != null && !string.IsNullOrEmpty(line))
            {
                if (line == "Line1")
                {
                    if (game.State == GameState.NotYetStarted && DateTime.UtcNow < game.GameTime)
                    {
                        return string.Format(AppResources.MinutesUntilGame, -(int)(DateTime.UtcNow - game.GameTime).TotalMinutes);
                    }

                    if (game.MyScore > game.OtherScore)
                    {
                        winLose = game.State == GameState.InProgress ? AppResources.GameWinning : AppResources.GameWon;
                    }
                    else if (game.MyScore < game.OtherScore)
                    {
                        winLose = game.State == GameState.InProgress ? AppResources.GameLosing : AppResources.GameLost;
                    }
                    else
                    {
                        winLose = AppResources.GameTied;
                    }

                    return string.Format(winLose, game.MyScore, game.OtherScore);
                }
                else if (line == "Line2")
                {
                    return string.Format(AppResources.GameAgainstTeam, game.OtherTeam.ShortName);
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
