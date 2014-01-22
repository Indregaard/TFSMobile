using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace MyTfsMobile.App.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            
        }

        private RelayCommand<MainViewModel> refreshCommand;
        public RelayCommand<MainViewModel> RefreshCommand
        {
            get
            {
                return refreshCommand ?? (refreshCommand = new RelayCommand<MainViewModel>(
                   (data) => Messenger.Default.Send(true, "RefreshButtonClick")));
            }
        }

        private RelayCommand<MainViewModel> settingsCommand;
        public RelayCommand<MainViewModel> SettingsCommand
        {
            get
            {
                return settingsCommand ?? (settingsCommand = new RelayCommand<MainViewModel>(
                   (data) => Messenger.Default.Send(true, "ShowSettingsPopup")));
            }
        }
    }
}