using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace MyTfsMobile.App.ViewModel
{

    public class SettingsViewModel : BaseViewModel
    {
        [PreferredConstructor]
        public SettingsViewModel()
        {
            LoadTfsUserSettings();
        }

        public string TfsServerAdress
        {
            get { return tfsSettings.TfsServer; }
            set
            {
                if (tfsSettings.TfsServer == value) return;
                tfsSettings.TfsServer = value;
                RaisePropertyChanged("TfsServerAdress");
            }
        }


        public string TfsServerUsername
        {
            get { return tfsSettings.TfsUsername; }
            set
            {
                if (tfsSettings.TfsUsername == value) return;
                tfsSettings.TfsUsername = value;
                RaisePropertyChanged("TfsServerUsername");
            }
        }

        public string TfsServerPassword
        {
            get { return tfsSettings.TfsPassword; }
            set
            {
                if (tfsSettings.TfsPassword == value) return;
                tfsSettings.TfsPassword = value;
                RaisePropertyChanged("TfsServerPassword");
            }
        }

        private TfsUserSettings tfsSettings;
        private void LoadTfsUserSettings()
        {
            tfsSettings = Locator.MyTfsMobileSettings.TfsSettings;
        }


        private async Task<bool> SaveSettings()
        {
            var settingsSaved = await Locator.MyTfsMobileSettings.SaveTfsSettings(tfsSettings);
            return settingsSaved;
        }



        private RelayCommand<SettingsViewModel> saveCommand;
        public RelayCommand<SettingsViewModel> SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand<SettingsViewModel>(
                    async retval =>
                          {
                              var saved = await SaveSettings();
                              var canLogIn = await Locator.TfsAuthenticationService.CheckTfsLogin(tfsSettings);
                              if (saved && canLogIn)
                                 CloseSettings();
                    }));
            }
        }

        public void ShowSettings()
        {
            Messenger.Default.Send(true, "ShowSettingsPopup");
        }

        private static void CloseSettings()
        {
            Messenger.Default.Send(false, "CloseSettingsPopup");
        }
    }

    public class TfsUserSettings
    {
        public string TfsServer { get; set; }
        public string TfsUsername { get; set; }
        public string TfsPassword { get; set; }
        public int RefreshInterval { get; set; }
    }

    public class TfsSettings
    {
        public Uri TfsServer { get; set; }
        public string TfsUsername { get; set; }
        public string TfsPassword { get; set; }
        
    }
}
