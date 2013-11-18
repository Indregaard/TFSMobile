using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using MyTfsMobile.App.ViewModels;
using Newtonsoft.Json;
using TfsMobile.Contracts;
using TfsMobile.Repositories.v1;

namespace MyTfsMobile.App.ViewModel
{
    public class HistoryViewModel : ViewModelBase
    {

        public HistoryViewModel()
        {
            HistoryItems = new ObservableCollection<HistoryItemViewModel>();
        }

        public ObservableCollection<HistoryItemViewModel> HistoryItems { get; private set; }

        public bool IsDataLoaded { get; private set; }
        private static readonly ViewModelLocator viewModelLocator = new ViewModelLocator();
        public async void LoadData()
        {
            await GetMyHistory();

        }

        private async Task GetMyHistory()
        {
            HistoryItems.Clear();

            var tfsUserDto = viewModelLocator.Settings.CreateTfsUserDto();
            var buildsRepo = new HistoryRepository(tfsUserDto, false);
            var buildsResult = await buildsRepo.GetHistoryAsync(new RequestHistoryDto() { FromDays = "7", TfsProject = "Byggtjeneste - Projects" });

            foreach (var historyItem in buildsResult)
            {
                HistoryItems.Add(new HistoryItemViewModel
                {
                    HistoryId = historyItem.Id,
                    HistoryItemType = historyItem.HistoryItemType,
                    HistoryDate = historyItem.HistoryDate,
                    AreaPath = historyItem.AreaPath,
                    Description = historyItem.Description,
                    IterationPath = historyItem.IterationPath,
                    State = historyItem.State,
                    TfsItemUri = historyItem.TfsItemUri,
                    WorkType = historyItem.WorkType
                });
            }

            IsDataLoaded = true;
        }
    }
}
