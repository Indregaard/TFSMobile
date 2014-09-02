using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using TfsMobile.Contracts;
using TfsMobile.Repositories.v1;

namespace MyTfsMobile.App.ViewModel
{
    public class HistoryViewModel : BaseViewModel
    {

        public HistoryViewModel()
        {
            HistoryItems = new ObservableCollection<HistoryItemViewModel>();
        }

        public ObservableCollection<HistoryItemViewModel> HistoryItems { get; private set; }

        public bool IsDataLoaded { get; private set; }
        public override async void LoadData()
        {
            await GetMyHistory();

        }

        private async Task GetMyHistory()
        {
            return;
            HistoryItems.Clear();
            if (!await UserAuthenticatedAgainstTfs()) return;

            var tfsUserDto = SimpleIoc.Default.GetInstance<ITfsAuthenticationService>().CreateTfsUserDto();
            var historyRepository = new HistoryRepository(tfsUserDto);
            var historyResult = await historyRepository.GetHistoryAsync(new RequestHistoryDto() { FromDays = "20", TfsProject = "Main" });

            foreach (var historyItem in historyResult)
            {
                HistoryItems.Add(new HistoryItemViewModel(historyItem));
            }

            IsDataLoaded = true;
        }
    }
}
