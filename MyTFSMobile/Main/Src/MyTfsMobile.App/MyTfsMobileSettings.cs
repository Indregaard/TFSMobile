using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using MyTfsMobile.App.ViewModel;

namespace MyTfsMobile.App
{
    public class MyTfsMobileSettings
    {

        private const string SettingsFile = "myTfsMobileSettings.mtm";
        private IsolatedStorageSettings _appSettings;

        [PreferredConstructor]
        public MyTfsMobileSettings()
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
                TfsSettings.TfsServer = value;
            }
        }

        public string TfsServerUsername
        {
            get { return TfsSettings.TfsUsername; }
            set
            {
                TfsSettings.TfsUsername = value;
            }
        }

        public string TfsServerPassword
        {
            get { return TfsSettings.TfsPassword; }
            set
            {
                TfsSettings.TfsPassword = value;
            }
        }

        private TfsUserSettings tfsSettings;
        public TfsUserSettings TfsSettings
        {
            get
            {
                if (tfsSettings == null)
                {
                    tfsSettings = LoadTfsUserSettings();
                }
                return tfsSettings;
            }
            set
            {
                tfsSettings = value;
            }
        }

        private TfsUserSettings LoadTfsUserSettings()
        {
            tfsSettings = new TfsUserSettings();
            if (_appSettings.Contains(SettingsFile))
            {
                tfsSettings = (TfsUserSettings)_appSettings[SettingsFile];
            }
            return tfsSettings;
        }

        public async Task<bool> SaveData(Action errorCallback)
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

        private async Task<bool> SaveAppsettings()
        {
            await TaskEx.Run(() => _appSettings.Save());
            return true;
        }

        public async Task<bool> SaveTfsSettings(TfsUserSettings settings)
        {
            TfsSettings = settings;
            var saved = await SaveData(SaveCommandError);
            return saved;
        }


        private void SaveCommandError()
        {
            // cannot save data..
            
        }

    }
}
