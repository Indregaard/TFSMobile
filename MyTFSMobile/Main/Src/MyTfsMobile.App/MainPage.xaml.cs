using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using MyTfsMobile.App.UserControls;
using MyTfsMobile.App.ViewModel;

namespace MyTfsMobile.App
{
    public partial class MainPage
    {
        private Popup loadingPopup;
        private Popup settingsPopup;
        public MainPage()
        {
            InitializeComponent();
            CreateLoadPopup();
            CreateSettingsPopup();

            RegisterMessengerEvents();


            var builds = (BuildsViewModel)BuildUserControl.DataContext;
            if (!builds.IsDataLoaded)
            {
                builds.LoadData();
            }
        }

        private void CreateLoadPopup()
        {
            loadingPopup = new Popup();
            var ovr = new OverLay();
            loadingPopup.Child = ovr;
        }

        private void CreateSettingsPopup()
        {
            settingsPopup = new Popup();
            var settingsUc = new SettingsUc();
            settingsPopup.Child = settingsUc;
        }

        private void RegisterMessengerEvents()
        {
            Messenger.Default.Register<bool>(this, "ShowSettingsPopup", (action) => ShowSettingsPopup());
            Messenger.Default.Register<bool>(this, "CloseSettingsPopup", (action) => CloseSettingsPopup());
            Messenger.Default.Register<bool>(this, "ShowLoadPopup", (action) => ShowLoadPopup());
            Messenger.Default.Register<bool>(this, "CloseLoadPopup", (action) => CloseLoadPopup()); 
        }

        public void ShowLoadPopup()
        {
            LayoutRoot.Opacity = 0.2;
            loadingPopup.IsOpen = true;
            ApplicationBar.IsVisible = false;
        }

        public void ShowSettingsPopup()
        {
            settingsPopup.IsOpen = true;
            ApplicationBar.IsVisible = false;
        }

        public void CloseSettingsPopup()
        {
            settingsPopup.IsOpen = false;
            ApplicationBar.IsVisible = true;
        }

        public void CloseLoadPopup()
        {
            loadingPopup.IsOpen = false;
            LayoutRoot.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
        }
    }
}