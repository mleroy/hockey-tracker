using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Windows;

namespace HockeyTracker
{
    public class TeamsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Team> EasternTeams { get; private set; }
        public ObservableCollection<Team> WesternTeams { get; private set; }
        
        public TeamsViewModel()
        {
            App.Data.ComponentLoaded += new AppData.ComponentLoadedEventHandler(Data_ComponentLoaded);

            this.EasternTeams = new ObservableCollection<Team>();
            this.WesternTeams = new ObservableCollection<Team>();

            foreach (Team team in App.Data.Teams)
            {
                if (team.Conference == Conference.East)
                {
                    this.EasternTeams.Add(team);
                }
                else
                {
                    this.WesternTeams.Add(team);
                }
            }
        }

        void Data_ComponentLoaded(object sender, AppData.ComponentLoadedEventArgs e)
        {
            NotifyPropertyChanged("IsDataLoading");
            NotifyPropertyChanged("ProgressBarVisibility");
        }

        public bool IsDataLoading
        {
            get
            {
                return !App.Data.IsDataLoaded;
            }
        }

        public Visibility ProgressBarVisibility
        {
            get
            {
                return IsDataLoading ? Visibility.Visible : Visibility.Collapsed;
            }
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