using System;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using MyTfsMobile.App.ViewModel;

namespace MyTfsMobile.App
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
            SetDataContext();
            RegisterNavigationRequest();
        }

        private void SetDataContext()
        {
            pageViewModel = new SettingsViewModel();
            DataContext = pageViewModel; 
        }

        private void RegisterNavigationRequest()
        {
            Messenger.Default.Register<Uri>(this, "NavigationRequest", (uri) => App.RootFrame.Navigate(uri)); 
        }

        private SettingsViewModel pageViewModel;
      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

       
    }
}