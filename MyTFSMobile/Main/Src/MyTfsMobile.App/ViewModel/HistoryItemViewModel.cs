using System;
using GalaSoft.MvvmLight;
using TfsMobile.Repositories.v1.Dtos;

namespace MyTfsMobile.App.ViewModel
{
    public class HistoryItemViewModel : ViewModelBase
    {
        public HistoryItemDto Item { get; set; }
    
        public int HistoryId
        {
            get { return Item.Id; }
            set
            {
                if (Item.Id == value) return;
                Item.Id = value;
                RaisePropertyChanged("HistoryId");
            }
        }


        public string WorkType
        {
            get { return Item.WorkType; }
            set
            {
                if (Item.WorkType == value) return;
                Item.WorkType = value;
                RaisePropertyChanged("WorkType");
            }
        }


        public string HistoryItemType
        {
            get { return Item.Title; }
            set
            {
                if (Item.Title == value) return;
                Item.Title = value;
                RaisePropertyChanged("HistoryItemType");
            }
        }

        public string Description
        {
            get { return Item.Description; }
            set
            {
                if (Item.Description == value) return;
                Item.Description = value;
                RaisePropertyChanged("Description");
            }
        }

  
        public DateTime HistoryDate
        {
            get { return Item.HistoryDate; }
            set
            {
                if (Item.HistoryDate == value) return;
                Item.HistoryDate = value;
                RaisePropertyChanged("HistoryDate");
            }
        }

        public Uri TfsItemUri
        {
            get { return Item.TfsItemUri; }
            set
            {
                if (Item.TfsItemUri == value) return;
                Item.TfsItemUri = value;
                RaisePropertyChanged("TfsItemUri");
            }
        }

     
        public string AreaPath
        {
            get { return Item.AreaPath; }
            set
            {
                if (Item.AreaPath == value) return;
                Item.AreaPath = value;
                RaisePropertyChanged("AreaPath");
            }
        }

        
        public string IterationPath
        {
            get { return Item.IterationPath; }
            set
            {
                if (Item.IterationPath == value) return;
                Item.IterationPath = value;
                RaisePropertyChanged("IterationPath");
            }
        }

        public HistoryItemViewModel(HistoryItemDto historyItem)
        {
            Item = historyItem;
        }

        public string State
        {
            get { return Item.State; }
            set
            {
                if (Item.State == value) return;
                Item.State = value;
                RaisePropertyChanged("State");
            }
        }
    }
}
