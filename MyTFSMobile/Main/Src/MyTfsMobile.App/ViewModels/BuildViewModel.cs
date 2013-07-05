using System;
using MyTfsMobile.App.enums;

namespace MyTfsMobile.App.ViewModels
{
    internal class BuildViewModel : ViewModelBase
    {
        private int buildId;
        public int BuildId
        {
            get { return buildId; } 
            set { if (buildId == value) return;
                buildId = value;
                NotifyPropertyChanged("BuildId");
            }
        }

        private string buildName;
        public string BuildName
        {
            get { return buildName; }
            set { 
                if (buildName == value) return;
                buildName = value;
                NotifyPropertyChanged("BuildName");
            }
        }

        private DateTime? buildDate;
        public DateTime? BuildDate
        {
            get { return buildDate; }
            set{
                if(buildDate==value) return;
                buildDate = value;
                NotifyPropertyChanged("BuildDate");
            }
        }

        private string buildMessages;
        public string BuildMessages
        {
            get { return buildMessages; }
            set { 
                if (buildMessages == value) return;
                buildMessages = value;
                NotifyPropertyChanged("BuildMessages");
            }
        }

        private BuildStatus buildStatus;
        public BuildStatus BuildStatus 
        {
            get { return buildStatus; }
            set { 
                if (buildStatus == value) return;
                buildStatus = value;
                NotifyPropertyChanged("BuildStatus");
            }
        }
    }

    
}
