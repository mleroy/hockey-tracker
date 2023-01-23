using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HockeyTracker.Data;
using System.Web.UI.HtmlControls;

namespace NotificationService
{
    public partial class Index : System.Web.UI.Page
    {
        protected int InstallationsToday { get; set; }
        protected int InstallationsYesterday { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                var now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
                var installationsTodayResult = context.GetInstallationCount(now.ToString("yyyy-MM-dd")).FirstOrDefault();
                var installationsYesterdayResult = context.GetInstallationCount(now.AddDays(-1).ToString("yyyy-MM-dd")).FirstOrDefault();

                if (installationsTodayResult != null)
                {
                    InstallationsToday = installationsTodayResult.Count.Value;
                }

                if (installationsYesterdayResult != null)
                {
                    InstallationsYesterday = installationsYesterdayResult.Count.Value;
                }

                var bob = context.Users.OrderByDescending(u => u.Id).Take(3).ToList();
                foreach (User user in bob)
                {
                    HtmlTableRow row = new HtmlTableRow();

                    row.Cells.Add(new HtmlTableCell { InnerText = user.Id.ToString() });

                    HtmlTableCell subscriptionsCell = new HtmlTableCell();

                    subscriptionsCell.InnerHtml = "<b>Goals: </b>";

                    foreach (Subscription subscription in user.Subscriptions.Where(s => s.SubscriptionType == "EveryGoal").OrderBy(s => s.Team).ToList())
                    {
                        subscriptionsCell.InnerHtml += string.Format(@"<img src=""images\{0}.png"" class=""teamImage small"" />", subscription.Team.ToLower());
                    }

                    subscriptionsCell.InnerHtml += "<br /><b>End score: </b>";

                    foreach (Subscription subscription in user.Subscriptions.Where(s => s.SubscriptionType == "EndScore").OrderBy(s => s.Team).ToList())
                    {
                        subscriptionsCell.InnerHtml += string.Format(@"<img src=""images\{0}.png"" class=""teamImage small"" />", subscription.Team);
                    }

                    subscriptionsCell.InnerHtml += "<br /><b>Live Tile: </b>";

                    foreach (Subscription subscription in user.Subscriptions.Where(s => s.SubscriptionType == "LiveTile").OrderBy(s => s.Team).ToList())
                    {
                        subscriptionsCell.InnerHtml += string.Format(@"<img src=""images\{0}.png"" class=""teamImage small"" />", subscription.Team);
                    }

                    row.Cells.Add(subscriptionsCell);
                    row.Cells.Add(new HtmlTableCell { InnerText = TimeZoneInfo.ConvertTimeFromUtc(user.LastUpdated, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")).ToString() });

                    UsersTable.Rows.Add(row);
                }

                var notifications = from n in context.NotificationsHistories.OrderByDescending(nh => nh.Id).Take(1000)
                                    group n by new { n.Title, n.Message } into g
                                    select new
                                    {
                                        Time = g.First().Time,
                                        Title = g.Key.Title,
                                        Message = g.Key.Message,
                                        Count = g.Count()
                                    };

                foreach (var n in notifications.OrderByDescending(g => g.Time).Take(20))
                {
                    HtmlTableRow historyRow = new HtmlTableRow();

                    historyRow.Cells.Add(new HtmlTableCell { InnerText = TimeZoneInfo.ConvertTimeFromUtc(n.Time, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")).ToString() });
                    historyRow.Cells.Add(new HtmlTableCell { InnerText = n.Title });
                    historyRow.Cells.Add(new HtmlTableCell { InnerText = n.Message });
                    historyRow.Cells.Add(new HtmlTableCell { InnerText = n.Count.ToString() });

                    HistoryTable.Rows.Add(historyRow);
                }

                var subscriptions = from s in context.Subscriptions
                                    group s by new { s.SubscriptionType, s.Team } into g
                                    select new { Team = g.Key.Team, SubscriptionType = g.Key.SubscriptionType, Num = g.Count() };

                var orderedSubscriptions = from o in subscriptions
                                           group o by o.Team into g
                                           select new { Team = g.Key, Num = g.Sum(s => s.Num) };

                foreach (var x in orderedSubscriptions.OrderByDescending(x => x.Num))
                {
                    HtmlTableRow row = new HtmlTableRow();

                    row.Cells.Add(new HtmlTableCell { InnerHtml = string.Format(@"<img src=""images\{0}.png"" class=""teamImage"" />", x.Team) });

                    var everyGoal = subscriptions.SingleOrDefault(s => s.SubscriptionType == "EveryGoal" && s.Team == x.Team);
                    var everyGoalCount = everyGoal == null ? 0 : everyGoal.Num;

                    var endScore = subscriptions.SingleOrDefault(s => s.SubscriptionType == "EndScore" && s.Team == x.Team);
                    var endScoreCount = endScore == null ? 0 : endScore.Num;

                    var liveTile = subscriptions.SingleOrDefault(s => s.SubscriptionType == "LiveTile" && s.Team == x.Team);
                    var liveTileCount = liveTile == null ? 0 : liveTile.Num;

                    row.Cells.Add(new HtmlTableCell { InnerHtml = everyGoalCount.ToString() });
                    row.Cells.Add(new HtmlTableCell { InnerHtml = endScoreCount.ToString() });
                    row.Cells.Add(new HtmlTableCell { InnerHtml = liveTileCount.ToString() });

                    Teams.Rows.Add(row);
                }
            }
        }
    }
}