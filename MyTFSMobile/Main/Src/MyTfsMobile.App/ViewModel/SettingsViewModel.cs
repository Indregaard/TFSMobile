using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

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

        private Uri tfsServerAdress;
        public string TfsServerAdress
        {
            get { return tfsServerAdress.AbsoluteUri; }
            set
            {
                if (tfsServerAdress.AbsoluteUri == value) return;
                tfsServerAdress = new Uri(value);
                RaisePropertyChanged("TfsServerAdress");
            }
        }

        private string tfsServerUsername;
        public string TfsServerUsername
        {
            get { return tfsServerUsername; }
            set
            {
                if (tfsServerUsername == value) return;
                tfsServerUsername = value;
                RaisePropertyChanged("TfsServerUsername");
            }
        }

        private string tfsServerPassword;
        public string TfsServerPassword
        {
            get { return tfsServerPassword; }
            set
            {
                if (tfsServerPassword == value) return;
                tfsServerPassword = value;
                RaisePropertyChanged("TfsServerPassword");
            }
        }

        public bool IsTfsAuthenticated
        {
            get { return false; }
        }


        private static TfsSettings tfsSettings;

        public static TfsSettings TfsSettings
        {
            get
            {
                if (tfsSettings == null)
                {
                    tfsSettings = new TfsSettings();
                    if (AppSettings[SettingsFile] != null)
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
                await new Task(SaveAppsettings);
                //SaveCarPhoto(CAR_PHOTO_FILE_NAME, Car.Picture, errorCallback);
            }
            catch (IsolatedStorageException)
            {
                errorCallback();
            }
            return false;
        }

        private static void SaveAppsettings()
        {
            AppSettings.Save();
        }


        private RelayCommand<SettingsViewModel> saveCommand;
        public RelayCommand<SettingsViewModel> SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand<SettingsViewModel>(
                    async (retval) =>
                          {
                              await SaveData(SaveCommandError);
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
