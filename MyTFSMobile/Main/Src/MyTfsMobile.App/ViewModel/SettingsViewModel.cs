using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Tasks;
using TfsMobile.Contracts;
using TfsMobile.Repositories.v1;

namespace MyTfsMobile.App.ViewModels
{

    public class SettingsViewModel : ViewModelBase
    {

        private const string SettingsFile = "myTfsMobileSettings.mtm";
        private static readonly IsolatedStorageSettings AppSettings = IsolatedStorageSettings.ApplicationSettings;

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

        private NetworkCredential NetCredentials { get; set; }

        public bool IsTfsAuthenticated
        {
            get
            {
                if (string.IsNullOrEmpty(TfsServerAdress)) return false;


                var cConn = TryConnectToTfs();
                return cConn.Result;
            }
        }

        private async Task<bool> TryConnectToTfs()
        {
            var df = new RequestTfsUserDto
            {
                Username = TfsServerUsername,
                Password = TfsServerPassword,
                TfsUri = new Uri(TfsServerAdress)
            };

            var rep = new TfsAccountRepository(df, false);
            var canConnect = await rep.CanConnectToTfs();
            return canConnect;
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

        static async private Task<bool> SaveData(Action errorCallback)
        {
            var settingsSaved = false;
            try
            {
                AppSettings[SettingsFile] = TfsSettings;
                var saved = await SaveAppsettings();
                if (saved){
                    settingsSaved = true; ;
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
            AppSettings.Save();
            return true;
        }


        private RelayCommand<SettingsViewModel> saveCommand;
        public RelayCommand<SettingsViewModel> SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand<SettingsViewModel>(
                    async (retval) =>
                          {
                              var saved = await SaveData(SaveCommandError);
                              if(saved && IsTfsAuthenticated)
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
