using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using MyTfsMobile.App.ViewModel;
using TfsMobile.Contracts;
using TfsMobile.Repositories.v1;
using TfsMobile.Repositories.v1.Interfaces;

namespace MyTfsMobile.App
{
    public interface ITfsAuthenticationService
    {
        Task<bool> CheckTfsLogin(TfsUserSettings settings);
        RequestTfsUserDto CreateTfsUserDto();
    }

    public class TfsAuthenticationService : ITfsAuthenticationService
    {
        public ViewModelLocator Locator = new ViewModelLocator();
        private ILoginRepository loginRepository;
        private TfsUserSettings TfsSettings;

        [PreferredConstructor]
        public TfsAuthenticationService()
        {
        }

        public TfsAuthenticationService(ILoginRepository loginRepo)
        {
            loginRepository = loginRepo;
        }

        public async Task<bool> CheckTfsLogin(TfsUserSettings settings)
        {
            Messenger.Default.Send(true, "ShowLoadPopup");
            var canLogIn = !string.IsNullOrEmpty(settings.TfsServer) && !string.IsNullOrEmpty(settings.TfsUsername) && !string.IsNullOrEmpty(settings.TfsPassword);

            TfsSettings = settings;

            if(canLogIn){
                var tfsUserDto = CreateTfsUserDto();
                CreateLoginRepository(tfsUserDto);
                var canAccessTfs = await loginRepository.TryLoginAsync(new RequestLoginContract { TfsUri = tfsUserDto.TfsUri.ToString() });
                canLogIn = canAccessTfs;
            }


            if (!canLogIn)
            {
                Locator.Settings.ShowSettings();
            }

            Messenger.Default.Send(true, "CloseLoadPopup");

            return canLogIn;
        }

        public RequestTfsUserDto CreateTfsUserDto()
        {
            if (TfsSettings == null) return null;

            return new RequestTfsUserDto
            {
                Username = TfsSettings.TfsUsername,
                Password = TfsSettings.TfsPassword,
                TfsUri = Uri.IsWellFormedUriString(TfsSettings.TfsServer, UriKind.Absolute) ? new Uri(TfsSettings.TfsServer) : null,
                TfsMobileApiUri = new Uri("http://192.168.1.17/TfsMobileServices/api/")
            };
        }

        private void CreateLoginRepository(RequestTfsUserDto tfsUserDto)
        {
            loginRepository = new LoginRepository(tfsUserDto, false);
        }
    }
}
