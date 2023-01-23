using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Info;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Net.NetworkInformation;
using System.Windows;
using Microsoft.Phone.Tasks;

namespace HockeyTracker
{
    public class Tools
    {
        public static Guid GetAnId()
        {
            string anid = UserExtendedProperties.GetValue("ANID") as string;
            anid = anid ?? string.Empty;

            string[] parts = anid.Split('&');
            IEnumerable<string[]> pairs = parts.Select(part => part.Split('='));
            string id = pairs
                .Where(pair => pair.Length == 2 && pair[0] == "A")
                .Select(pair => pair[1])
                .FirstOrDefault();

            return (id == null) ? Guid.Empty : new Guid(id);
        }

        public static bool IsNetworkAvailable
        {
            get
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
        }

        public static void HandleError(Exception e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBoxResult result =
                    MessageBox.Show("Tap OK to report this error to the developer, or Cancel to continue.", "An error occured",
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    EmailComposeTask emailComposeTask = new EmailComposeTask
                    {
                        To = "hockeytrackerwp@hotmail.com",
                        Subject = "Hockey Tracker error",
                        Body = "Error details: " + e
                    };

                    emailComposeTask.Show();
                }
            });
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Returns the date as today, tomorrow, month/day
        /// </summary>
        public static string ToFriendlyDate(this DateTime date)
        {
            DateTime localDateTime = date.ToLocalTime();

            if (localDateTime.Date == DateTime.Today)
            {
                if (localDateTime.Hour >= 17)
                    return "tonight";
                else
                    return "today";
            }

            return localDateTime.ToString("ddd, M/d");
        }

        /// <summary>
        /// Returns a time as 7.30pm
        /// </summary>
        public static string ToFriendlyTime(this DateTime date)
        {
            return date.ToLocalTime().ToString("h.mmtt").ToLower();
        }
    }
}
