using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HockeyTracker.Notifier;

namespace NotificationService
{
    public partial class Notifications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SendTileNotif.Click += new EventHandler(SendTileNotif_Click);
        }

        void SendTileNotif_Click(object sender, EventArgs e)
        {
            Notifier.SendTileNotificationsForNextGame(TileNotif_TeamShortName.Value);
        }
    }
}