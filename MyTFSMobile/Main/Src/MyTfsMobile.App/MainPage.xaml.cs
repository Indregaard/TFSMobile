using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml.Linq;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
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
            RegisterMessengerEvents();
            InitializeComponent();

            CreateLoadPopup();
            CreateSettingsPopup();

            GetProgramVersion();
        }

        private void GetProgramVersion()
        {
            var xElement = XDocument.Load("WMAppManifest.xml").Root;
            if (xElement == null) _versionText.Text = "Unknown";
            if (xElement != null && xElement.Element("App") != null && xElement.Element("App").Attribute("Version") != null)
            {
                var version = xElement.Element("App").Attribute("Version").Value;
                _versionText.Text = version;
                    
            }
        }

        private void GetCurrentPanoPage()
        {
            
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
            Messenger.Default.Register<bool>(this, "RefreshButtonClick", (action) => RefreshButtonClick());
        }

        private void RefreshButtonClick()
        {
            var selectedpanoItem = panoPage.SelectedItem;
            if (selectedpanoItem == MyBuilds)
                ((BuildsViewModel)MyBuildsUserControl.DataContext).LoadData();
            else if (selectedpanoItem == TeamBuilds)
                ((BuildsViewModel)TeamBuildUserControl.DataContext).LoadData();
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

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            var s = ((ButtonBase)sender).Tag as string;

            switch (s)
            {
                case "Review":
                    var task = new MarketplaceReviewTask();
                    task.Show();
                    break;
            }
        }

        private void Panorama_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // få tak i uc, last data..
            //var selectedpanoItem = panoPage.SelectedItem;
            //if (selectedpanoItem == MyBuilds)
            //    ((BuildsViewModel) MyBuildsUserControl.DataContext).LoadData();
            //else if (selectedpanoItem == TeamBuilds)
            //    ((BuildsViewModel)TeamBuildUserControl.DataContext).LoadData();
        }

    }
}