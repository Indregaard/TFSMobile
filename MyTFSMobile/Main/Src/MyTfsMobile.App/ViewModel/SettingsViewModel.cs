using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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


                var cConn = checkTfsLogin().Wait(5000);
                return cConn;
            }
        }

        private bool TryConnectToTfs()
        {
           

            
            //var canConnect = doHent(new RequestLoginContract { TfsUri = df.TfsUri.ToString() });
            bool canConnect = false;
            checkTfsLogin().ContinueWith(s => canConnect = s.Result,
        TaskScheduler.FromCurrentSynchronizationContext());

            return canConnect;
        }

        private async Task<bool> checkTfsLogin()
        {

             var df = new RequestTfsUserDto
            {
                Username = TfsServerUsername,
                Password = TfsServerPassword,
                TfsUri = new Uri(TfsServerAdress)
            };

            var rep = new LoginRepository(df, false);
            bool canConnect = false;

            var test = rep.TryLoginAsync(new RequestLoginContract { TfsUri = df.TfsUri.ToString() }).ContinueWith(s => canConnect = s.Result,
        TaskScheduler.FromCurrentSynchronizationContext());

            await test;
            return test.Result;

        }

        //private async Task<bool> doHent(RequestLoginContract contract)
        //{
        //    //HttpClientHandler handler = new HttpClientHandler();
        //    //HttpClient client = new HttpClient(handler);
        //    //client.Timeout = TimeSpan.FromSeconds(5);
        //    //client.BaseAddress = new Uri("http://192.168.10.193/TfsMobileServices/api");
        //    //HttpResponseMessage response = await client.GetAsync("/Values");//<--Hangs
        //    //var data = await response.Content.ReadAsStringAsync();

        //    //var x = data;

        //    //return true;

        //    //using (var client = new HttpClient())
        //    //{
        //    //    client.DefaultRequestHeaders.Add("tfsuri", contract.TfsUri.ToString());


        //    //    client.DefaultRequestHeaders.Authorization =
        //    //    new AuthenticationHeaderValue(
        //    //        "Basic",
        //    //        Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", TfsServerUsername, TfsServerPassword)))
        //    //        );

        //    //    dynamic s = new ExpandoObject();
        //    //    s.comeValue = 1;
        //    //    var d = JsonConvert.SerializeObject(s);
        //    //    var requestcontent = new StringContent(d, Encoding.UTF8, "application/json");

        //    //    requestcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //    //    var requestUri = new Uri("http://192.168.10.193/TfsMobileServices/api/Login");

        //    //    var response =  client.PostAsync(requestUri, requestcontent).ContinueWith(tt =>
        //    //    {
        //    //        if (tt.Result.StatusCode == HttpStatusCode.OK)
        //    //        {
        //    //            var foo = tt.Result.Content.ReadAsStringAsync();
        //    //            return JsonConvert.DeserializeObject<bool>(foo.Result);
        //    //        }
        //    //        return false;
        //    //    });
        //    //    var resultat = await response;
        //    //    return resultat;


        //    //}

        //}


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
