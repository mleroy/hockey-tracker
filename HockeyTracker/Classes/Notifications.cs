using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Phone.Notification;
using Microsoft.Phone.Tasks;
using HockeyTracker.HockeyTrackerService;
using System.Globalization;

namespace HockeyTracker
{
    public class NotificationsEngine
    {
        public HttpNotificationChannel myChannel;

        public void CreateNotificationChannel(string channelName)
        {
            myChannel = HttpNotificationChannel.Find(channelName);

            if (myChannel == null)
            {
                // Only one notification channel name is supported per application.
                myChannel = new HttpNotificationChannel(channelName);

                SetUpDelegates();

                // After myChannel.Open() is called, the notification channel URI will be sent to the application through the ChannelUriUpdated delegate.
                // If your application requires a timeout for setting up a notification channel, start it after the myChannel.Open() call. 
                myChannel.Open();
            }
            else // Found an existing notification channel.
            {
                SetUpDelegates();

                if (myChannel.ChannelUri == null)
                {
                    // The notification channel URI has not been sent to the client. Wait for the ChannelUriUpdated delegate to fire.
                    // If your application requires a timeout for setting up a notification channel, start it here.
                }
            }

            // An application is expected to send its notification channel URI to its corresponding web service each time it launches.
            // The notification channel URI is not guaranteed to be the same as the last time the application ran. 
            if (myChannel.ChannelUri != null)
            {
                AfterChannelSetup(myChannel.ChannelUri);
            }
        }

        public void SetUpDelegates()
        {
            myChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(myChannel_ChannelUriUpdated);
            myChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(myChannel_HttpNotificationReceived);
            myChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(myChannel_ShellToastNotificationReceived);
            myChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(myChannel_ErrorOccurred);
        }

        void myChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            AfterChannelSetup(e.ChannelUri);
        }

        void AfterChannelSetup(Uri channelUri)
        {
            BindingANotificationsChannelToAToastNotification();
            BindingANotificationsChannelToATileNotification();

            int utcOffsetInMinutes = (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalMinutes;

            string cultureName = CultureInfo.CurrentUICulture.Name;

            App.HTService.RegisterUserAsync(Tools.GetAnId(), utcOffsetInMinutes, channelUri.ToString(), cultureName);
            
            App.HTService.RegisterUserCompleted += new EventHandler<RegisterUserCompletedEventArgs>((s, e) =>
            {
                if (e.Error != null)
                {
                    Tools.HandleError(e.Error);
                }
            });
        }

        void myChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            switch (e.ErrorType)
            {
                case ChannelErrorType.ChannelOpenFailed:
                    // ...
                    break;
                case ChannelErrorType.MessageBadContent:
                    // ...
                    break;
                case ChannelErrorType.NotificationRateTooHigh:
                    // ...
                    break;
                case ChannelErrorType.PayloadFormatError:
                    // ...
                    break;
                case ChannelErrorType.PowerLevelChanged:
                    // ...
                    break;
            }
        }

        // Receiving a toast notification. 
        // Toast notifications are only delivered to the device when the application is not running in the foreground. 
        // If the application is running in the foreground, the toast notification is instead routed to the application.
        void myChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            if (e.Collection != null)
            {
                Dictionary<string, string> collection = (Dictionary<string, string>)e.Collection;
                System.Text.StringBuilder messageBuilder = new System.Text.StringBuilder();

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(collection["wp:Text2"], collection["wp:Text1"], MessageBoxButton.OK);
                });
            }
        }

        // Receiving a raw notification. 
        // Raw notifications are only delivered to the application when it is running in the foreground. 
        // If the application is not running in the foreground, the raw notification message 
        // is dropped on the Push Notification Service and is not delivered to the device.
        void myChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            if (e.Notification.Body != null && e.Notification.Headers != null)
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(e.Notification.Body);
            }
        }



        // If you do not want to use remote resources for your Tile's background image, use the following code to bind a notification channel to a local resource. 
        // A Tile notification is always delivered to the Tile, regardless of whether the application is running in the foreground.  
        private void BindingANotificationsChannelToATileNotification()
        {
            if (!myChannel.IsShellTileBound)
            {
                myChannel.BindToShellTile();
            }
        }


        // If you want to use remote or local resources for your Tile's background image, 
        // use the following code to bind a notification channel to either a remote resource that is in the approved list, or to a local resource. 
        // A Tile notification is always delivered to the Tile, regardless of whether the application is running in the foreground.  
        private void BindingANotificationsChannelToALiveTileNotification()
        {
            if (!myChannel.IsShellTileBound)
            {
                myChannel.BindToShellTile();
            }

            // The approved list of URIs that will be verified on every push notification that contains a URI reference.
            Collection<Uri> ListOfAllowedDomains = new Collection<Uri> { new Uri("www.contoso.com") };
            myChannel.BindToShellTile(ListOfAllowedDomains);
        }

        // Binding a notification channel to a toast notification.
        private void BindingANotificationsChannelToAToastNotification()
        {
            if (!myChannel.IsShellToastBound)
            {
                myChannel.BindToShellToast();
            }
        }
    }
}
