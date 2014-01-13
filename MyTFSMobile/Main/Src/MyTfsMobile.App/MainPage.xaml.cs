using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using MyTfsMobile.App.UserControls;

namespace MyTfsMobile.App
{
    public partial class MainPage
    {
        private readonly Popup loadingPopup;
        private Popup settingsPopup;
        public MainPage()
        {
            InitializeComponent();

            loadingPopup = new Popup();
            CreateSetttingsPopup();

            Messenger.Default.Register<bool>(this, "ShowSettingsPopup", (action) => ShowSettingsPopup());
            Messenger.Default.Register<bool>(this, "CloseSettingsPopup", (action) => CloseSettingsPopup());
        }

        private void CreateSetttingsPopup()
        {
            settingsPopup = new Popup();
            var settingsUc = new SettingsUc();
            settingsPopup.Child = settingsUc;
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
       

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {

            Messenger.Default.Send<bool>(false, "ShowSettingsPopup");
            //LayoutRoot.Opacity = 0.2;
            //var ovr = new OverLay();
            //loadingPopup.Child = ovr;
            //loadingPopup.IsOpen = true;
            //var worker = new BackgroundWorker();
            //worker.DoWork += (s, a) =>
            //{
            //    Thread.Sleep(5000);
            //};
            //worker.RunWorkerCompleted += (s, a) =>
            //{
            //    loadingPopup.IsOpen = false;
            //    LayoutRoot.Opacity = 1.0;
            //};
            //worker.RunWorkerAsync();
        }



        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            loadingPopup.IsOpen = false;
        }
    }

    public class ShowSettingsMessage
    {
        public string Message { get; set; }
    }
}