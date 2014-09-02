using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace MyTfsMobile.App.ViewModel
{
    public class BaseViewModel : ViewModelBase, IViewModel
    {
        public ViewModelLocator Locator = new ViewModelLocator();


        public virtual void LoadData()
        {
           
        }

        protected async Task<bool> UserAuthenticatedAgainstTfs()
        {
            return await Locator.TfsAuthenticationService.CheckTfsLogin(Locator.MyTfsMobileSettings.TfsSettings);
        }
    }

    public interface IViewModel
    {
        void LoadData();
    }
}
