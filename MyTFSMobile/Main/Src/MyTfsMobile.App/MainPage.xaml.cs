using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using MyTfsMobile.App.UserControls;

namespace MyTfsMobile.App
{
    public partial class MainPage
    {
        private Popup popup;
        public MainPage()
        {
            InitializeComponent();

            this.popup = new Popup();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            this.LayoutRoot.Opacity = 0.2;
            OverLay ovr = new OverLay();
            this.popup.Child = ovr;
            this.popup.IsOpen = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, a) =>
            {
                Thread.Sleep(5000);
            };
            worker.RunWorkerCompleted += (s, a) =>
            {
                popup.IsOpen = false;
                this.LayoutRoot.Opacity = 1.0;
            };
            worker.RunWorkerAsync();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            this.popup.IsOpen = false;
        }
    }
}