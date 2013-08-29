using System;
using GalaSoft.MvvmLight;
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
                RaisePropertyChanged("BuildId");
            }
        }

        private string buildName;
        public string BuildName
        {
            get { return buildName; }
            set { 
                if (buildName == value) return;
                buildName = value;
                RaisePropertyChanged("BuildName");
            }
        }

        private DateTime? buildDate;
        public DateTime? BuildDate
        {
            get { return buildDate; }
            set{
                if(buildDate==value) return;
                buildDate = value;
                RaisePropertyChanged("BuildDate");
            }
        }

        private string buildMessages;
        public string BuildMessages
        {
            get { return buildMessages; }
            set { 
                if (buildMessages == value) return;
                buildMessages = value;
                RaisePropertyChanged("BuildMessages");
            }
        }

        private BuildStatus buildStatus;
        public BuildStatus BuildStatus 
        {
            get { return buildStatus; }
            set { 
                if (buildStatus == value) return;
                buildStatus = value;
                RaisePropertyChanged("BuildStatus");
            }
        }
    }

    
}
