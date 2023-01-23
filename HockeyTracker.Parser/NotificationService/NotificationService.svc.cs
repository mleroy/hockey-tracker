using System.ServiceModel;
using System.ServiceModel.Activation;
using HockeyTracker.Notifier;

namespace NotificationService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class NotificationService
    {
        [OperationContract]
        public void SendToastNotifications(string teamShortName, string subscriptionType, string toastTitle, string toastMessage)
        {
            Notifier.SendToastNotifications(teamShortName, subscriptionType, toastTitle, toastMessage);
        }

        [OperationContract]
        public void SendTileNotifications(string teamShortName, string tileTitle, string tileContent)
        {
            Notifier.SendTileNotifications(teamShortName, tileTitle, tileContent);
        }

        [OperationContract]
        public void SendTileNotificationsForNextGame(string teamShortName)
        {
            Notifier.SendTileNotificationsForNextGame(teamShortName);
        }
    }
}
