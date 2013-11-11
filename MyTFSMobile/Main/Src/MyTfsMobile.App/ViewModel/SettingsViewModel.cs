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
        private static readonly IsolatedStorageSettings AppSettings = IsolatedStorageSettings.ApplicationSettings;
        private ILoginRepository loginRepository;

        [PreferredConstructor]
        public SettingsViewModel(){}

        public SettingsViewModel(ILoginRepository loginRepo)
        {
            loginRepository = loginRepo;
        }

        public string TfsServerAdress
        {
            get { return TfsSettings.TfsServer != null ? TfsSettings.TfsServer.AbsoluteUri : ""; }
            set
            {
                if ((TfsSettings.TfsServer != null && TfsSettings.TfsServer.AbsoluteUri == value) || value == null) return;
                TfsSettings.TfsServer = new Uri(value);
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

        public async Task<bool> CheckTfsLogin()
        {
            if (string.IsNullOrEmpty(TfsServerAdress) || string.IsNullOrEmpty(TfsServerUsername) ||
                string.IsNullOrEmpty(TfsServerPassword))
                return false;

            var tfsUserDto = CreateTfsUserDto();
            CreateLoginRepository(tfsUserDto);
            var canAccessTfs = await loginRepository.TryLoginAsync(new RequestLoginContract { TfsUri = tfsUserDto.TfsUri.ToString() });

            return canAccessTfs;
        }

        public RequestTfsUserDto CreateTfsUserDto()
        {
            return new RequestTfsUserDto
            {
                Username = TfsServerUsername,
                Password = TfsServerPassword,
                TfsUri = new Uri(TfsServerAdress)
            };
        }

        private void CreateLoginRepository(RequestTfsUserDto tfsUserDto)
        {
            loginRepository = new LoginRepository(tfsUserDto, false);
        }

        private static TfsSettings tfsSettings;
        public static TfsSettings TfsSettings
        {
            get
            {
                if (tfsSettings == null)
                {
                    tfsSettings = new TfsSettings();
                    if (AppSettings.Contains(SettingsFile))
                    {
                        tfsSettings = (TfsSettings)AppSettings[SettingsFile];
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
                AppSettings[SettingsFile] = TfsSettings;
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

        private static void NavigateToBuilds()
        {
            Messenger.Default.Send(new Uri("/Builds.xaml", UriKind.Relative), "NavigationRequest");
        }

        private static async Task<bool> SaveAppsettings()
        {
            await TaskEx.Run(() => AppSettings.Save());
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
                              var canLogIn = await CheckTfsLogin();
                              if (saved && canLogIn)
                                  NavigateToBuilds();
                    }));
            }
        }

        private void SaveCommandError()
        {
            // cannot save data..
            
        }


    }

    public class TfsSettings
    {
        public Uri TfsServer { get; set; }
        public string TfsUsername { get; set; }
        public string TfsPassword { get; set; }
        
    }
}
