using System;
using System.ComponentModel;

namespace HockeyTracker
{
    public enum Conference { East, West };

    public enum Division { Atlantic, Metropolitan, Pacific, Central };

    public class Team : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string shortName;
        public string ShortName
        {
            get
            {
                return shortName;
            }
            set
            {
                if (value != shortName)
                {
                    shortName = value;
                    NotifyPropertyChanged("ShortName");
                }
            }
        }

        private string locale;
        public string Locale
        {
            get
            {
                return locale;
            }
            set
            {
                if (value != locale)
                {
                    locale = value;
                    NotifyPropertyChanged("Locale");
                }
            }
        }

        private Conference conference;
        public Conference Conference
        {
            get
            {
                return conference;
            }
            set
            {
                if (value != conference)
                {
                    conference = value;
                    NotifyPropertyChanged("Conference");
                }
            }
        }

        private Division division;
        public Division Division
        {
            get
            {
                return division;
            }
            set
            {
                if (value != division)
                {
                    division = value;
                    NotifyPropertyChanged("Division");
                }
            }
        }

        private TeamLiveGame _currentGame;
        public TeamLiveGame CurrentGame
        {
            get
            {
                return _currentGame;
            }
            set
            {
                if (value != _currentGame)
                {
                    _currentGame = value;
                    NotifyPropertyChanged("CurrentGame");
                }
            }
        }

        public override string ToString()
        {
            return CapitalizedTitle.ToLower();
        }

        public string CapitalizedTitle
        {
            get
            {
                return string.Format("{0} {1}", this.Locale, this.Name);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}