using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyTfsMobile.App.ViewModels;

namespace MyTfsMobile.App
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
            vm = new SettingsViewModel();

            DataContext = vm;

            Messenger.Default.Register<Uri>(this, "NavigationRequest", (uri) => App.RootFrame.Navigate(uri));

        }

        private SettingsViewModel vm;
      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

       
    }
}