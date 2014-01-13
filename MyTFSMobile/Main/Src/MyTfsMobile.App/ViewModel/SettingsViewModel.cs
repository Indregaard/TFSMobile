using System;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using TfsMobile.Contracts;
using TfsMobile.Repositories.v1;
using TfsMobile.Repositories.v1.Interfaces;

namespace MyTfsMobile.App.ViewModel
{

    public class SettingsViewModel : ViewModelBase
    {

        private const string SettingsFile = "myTfsMobileSettings.mtm";
        private static IsolatedStorageSettings _appSettings;

        [PreferredConstructor]
        public SettingsViewModel()
        {
            if (!System.ComponentModel.DesignerProperties.IsInDesignTool)
            {
                _appSettings = IsolatedStorageSettings.ApplicationSettings;
            }
        }

        public string TfsServerAdress
        {
            get { return TfsSettings.TfsServer; }
            set
            {
                if (TfsSettings.TfsServer == value) return;
                TfsSettings.TfsServer = value;
                RaisePropertyChanged("TfsServerAdress");
            }
        }

        public string TfsServerUsername
        {
            get { return TfsSettings.TfsUsername; }
            set
            {
                if (TfsSettings.TfsUsername == value) return;
                TfsSettings.TfsUsername = value;
                RaisePropertyChanged("TfsServerUsername");
            }
        }

        public string TfsServerPassword
        {
            get { return TfsSettings.TfsPassword; }
            set
            {
                if (TfsSettings.TfsPassword == value) return;
                TfsSettings.TfsPassword = value;
                RaisePropertyChanged("TfsServerPassword");
            }
        }

        

        

        

        private static TfsUserSettings tfsSettings;
        public static TfsUserSettings TfsSettings
        {
            get
            {
                if (tfsSettings == null)
                {
                    tfsSettings = new TfsUserSettings();
                    if (_appSettings.Contains(SettingsFile))
                    {
                        tfsSettings = (TfsUserSettings)_appSettings[SettingsFile];
                    }
                }
                return tfsSettings;
            }
            set
            {
                tfsSettings = value;
                NotifyTfsSettingsUpdated();
            }
        }

        public static event EventHandler TfsSettingsUpdated;
        private static void NotifyTfsSettingsUpdated()
        {
            var handler = TfsSettingsUpdated;
            if (handler != null) handler(null, null);
        }

        private static async Task<bool> SaveData(Action errorCallback)
        {
            var settingsSaved = false;
            try
            {
                _appSettings[SettingsFile] = TfsSettings;
                var saved = await SaveAppsettings();
                if (saved){
                    settingsSaved = true;
                }
            }
            catch (IsolatedStorageException)
            {
                settingsSaved = false;
                errorCallback();
                
            }
            return settingsSaved;
        }

        private static void CloseSettings()
        {
            Messenger.Default.Send(false, "CloseSettingsPopup");
        }

        private static async Task<bool> SaveAppsettings()
        {
            await TaskEx.Run(() => _appSettings.Save());
            return true;
        }


        private RelayCommand<SettingsViewModel> saveCommand;
        public RelayCommand<SettingsViewModel> SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand<SettingsViewModel>(
                    async retval =>
                          {
                              var saved = await SaveData(SaveCommandError);
                              var canLogIn = await SimpleIoc.Default.GetInstance<ITfsAuthenticationService>().CheckTfsLogin(TfsSettings);
                              //if (saved && canLogIn)
                                 
                              CloseSettings();
                    }));
            }
        }

        private void SaveCommandError()
        {
            // cannot save data..
            
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
