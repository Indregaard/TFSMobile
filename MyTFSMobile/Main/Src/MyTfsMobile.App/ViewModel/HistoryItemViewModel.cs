using System;
using GalaSoft.MvvmLight;

namespace MyTfsMobile.App.ViewModel
{
    public class HistoryItemViewModel : ViewModelBase
    {

        private int historyId;
        public int HistoryId
        {
            get { return historyId; }
            set
            {
                if (historyId == value) return;
                historyId = value;
                RaisePropertyChanged("HistoryId");
            }
        }

        private string workType;
        public string WorkType
        {
            get { return workType; }
            set
            {
                if (workType == value) return;
                workType = value;
                RaisePropertyChanged("WorkType");
            }
        }

        private string historyItemType;
        public string HistoryItemType
        {
            get { return historyItemType; }
            set
            {
                if (historyItemType == value) return;
                historyItemType = value;
                RaisePropertyChanged("HistoryItemType");
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (description == value) return;
                description = value;
                RaisePropertyChanged("Description");
            }
        }

        private DateTime historyDate;
        public DateTime HistoryDate
        {
            get { return historyDate; }
            set
            {
                if (historyDate == value) return;
                historyDate = value;
                RaisePropertyChanged("HistoryDate");
            }
        }

        private Uri tfsItemUri;
        public Uri TfsItemUri
        {
            get { return tfsItemUri; }
            set
            {
                if (tfsItemUri == value) return;
                tfsItemUri = value;
                RaisePropertyChanged("TfsItemUri");
            }
        }

        private string areaPath;
        public string AreaPath
        {
            get { return areaPath; }
            set
            {
                if (areaPath == value) return;
                areaPath = value;
                RaisePropertyChanged("AreaPath");
            }
        }

        private string iterationPath;
        public string IterationPath
        {
            get { return iterationPath; }
            set
            {
                if (iterationPath == value) return;
                iterationPath = value;
                RaisePropertyChanged("IterationPath");
            }
        }

        private string state;
        public string State
        {
            get { return state; }
            set
            {
                if (state == value) return;
                state = value;
                RaisePropertyChanged("State");
            }
        }
    }
}
