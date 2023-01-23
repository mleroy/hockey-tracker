using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace HockeyTracker
{
    public partial class MainPage : PhoneApplicationPage
    {
        TeamsViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();

            viewModel = new TeamsViewModel();

            DataContext = viewModel;
        }

        private void TeamsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigateToTeam((ListBox)sender);
        }

        private void NavigateToTeam(ListBox listOfTeams)
        {
            if (listOfTeams.SelectedIndex == -1) return;

            Team selectedTeam = (Team)listOfTeams.SelectedItem;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/TeamNotifications.xaml?Team=" + selectedTeam.ShortName, UriKind.Relative));

            // Set the selected indices to impossible values so that tapping twice on the same item (between navigations) triggers the event each time
            listOfTeams.SelectedIndex = -1;
        }

        private void SendFeedback_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask
            {
                To = "hockeytrackerwp@hotmail.com",
                Subject = "Hockey Tracker feedback"
            };

            emailComposeTask.Show();
        }

        private void RateApp_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceTask = new MarketplaceReviewTask();
            marketplaceTask.Show();
        }
    }
}