using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Tasks;

namespace MyTfsMobile.App.ViewModels
{

    public class SettingsViewModel : ViewModelBase
    {

        private const string SettingsFile = "myTfsMobileSettings.mtm";
        private static readonly IsolatedStorageSettings AppSettings = IsolatedStorageSettings.ApplicationSettings;
        // tfsserver
        // username
        //password
        //isAuthenticated?
        //isloggedin?
        //andre??

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

        private static bool Auth { get; set; }
        public bool IsTfsAuthenticated
        {
            get { return Auth; }
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
            try
            {
                AppSettings[SettingsFile] = TfsSettings;
                var saved = await SaveAppsettings();
                if (saved){
                    Auth = true;;
                }
            }
            catch (IsolatedStorageException)
            {
                Auth = false;
                errorCallback();
                
            }
            return Auth;
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
                              if(saved)
                                  NavigateToBuilds();
                    }));
            }
        }

        private void SaveCommandError()
        {
            // error prosessering?
        }


    }

    public class TfsSettings
    {
        public Uri TfsServer { get; set; }
        public string TfsUsername { get; set; }
        public string TfsPassword { get; set; }
        
    }
}
