using GalaSoft.MvvmLight;

namespace MyTfsMobile.App.ViewModel
{
    public class BaseViewModel : ViewModelBase, IViewModel
    {
        public ViewModelLocator Locator = new ViewModelLocator();


        public virtual void LoadData()
        {
           
        }
    }

    public interface IViewModel
    {
        void LoadData();
    }
}
