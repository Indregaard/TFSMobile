using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace MyTfsMobile.App.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {
        public ViewModelLocator Locator = new ViewModelLocator();
    }
}
