using System.Linq;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace HockeyTracker
{
    public class TeamViewModel : INotifyPropertyChanged
    {
        public Team Team { get; set; }

        public TeamViewModel()
        {
            App.Data.Subscriptions.CollectionChanged += new NotifyCollectionChangedEventHandler(Subscriptions_CollectionChanged);
            App.Data.ComponentLoaded += new AppData.ComponentLoadedEventHandler(Data_ComponentLoaded);
        }
        
        public bool UserSubscribedToEveryGoal
        {
            get
            {
                return App.Data.Subscriptions.Any(s => s.Team == Team.ShortName && s.SubscriptionType == "EveryGoal");
            }
        }

        public bool UserSubscribedToEndScore
        {
            get
            {
                return App.Data.Subscriptions.Any(s => s.Team == Team.ShortName && s.SubscriptionType == "EndScore");
            }
        }

        public bool TeamLiveTileEnabled
        {
            get
            {
                return App.Data.Subscriptions.Any(s => s.Team == Team.ShortName && s.SubscriptionType == "LiveTile");
            }
        }

        public bool IsDataLoading
        {
            get
            {
                return !App.Data.IsDataLoaded;
            }
        }

        public bool IsDataLoaded
        {
            get
            {
                return App.Data.IsDataLoaded;
            }
        }

        public Visibility ProgressBarVisibility
        {
            get
            {
                return IsDataLoading ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        
        void Data_ComponentLoaded(object sender, AppData.ComponentLoadedEventArgs e)
        {
            NotifyPropertyChanged("IsDataLoading");
            NotifyPropertyChanged("IsDataLoaded");
            NotifyPropertyChanged("ProgressBarVisibility");
        }

        void Subscriptions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("UserSubscribedToEveryGoal");
            NotifyPropertyChanged("UserSubscribedToEndScore");
            NotifyPropertyChanged("TeamLiveTileEnabled");
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            foreach (var name in propertyNames)
            {
                NotifyPropertyChanged(name);
            }
        }
        #endregion
    }
}