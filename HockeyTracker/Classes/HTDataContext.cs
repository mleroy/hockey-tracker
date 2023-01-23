using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace HockeyTracker
{
    [Table]
    public class ScheduleItem : INotifyPropertyChanged
    {
        // Define ID: private field, public property and database column.
        private int _number;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Number
        {
            get
            {
                return _number;
            }
            set
            {
                if (_number != value)
                {
                    _number = value;
                    NotifyPropertyChanged("Number");
                }
            }
        }

        private DateTime _gameTime;

        [Column(IsPrimaryKey = false, IsDbGenerated = false, DbType = "DateTime NOT NULL", CanBeNull = false)]
        public DateTime GameTime
        {
            get
            {
                return _gameTime;
            }
            set
            {
                if (_gameTime != value)
                {
                    _gameTime = value;
                    NotifyPropertyChanged("GameTime");
                }
            }
        }

        private string _awayTeam;

        [Column]
        public string AwayTeam
        {
            get
            {
                return _awayTeam;
            }
            set
            {
                if (_awayTeam != value)
                {
                    _awayTeam = value;
                    NotifyPropertyChanged("AwayTeam");
                }
            }
        }

        private string _homeTeam;

        [Column]
        public string HomeTeam
        {
            get
            {
                return _homeTeam;
            }
            set
            {
                if (_homeTeam != value)
                {
                    _homeTeam = value;
                    NotifyPropertyChanged("HomeTeam");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
