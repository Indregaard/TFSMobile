using System.Windows;
using Microsoft.Phone.Shell;

namespace MyTfsMobile.App.UserControls
{
    public partial class OverLay
    {
        public OverLay()
        {
            InitializeComponent();
            LayoutRoot.Height = Application.Current.Host.Content.ActualHeight;
            LayoutRoot.Width = Application.Current.Host.Content.ActualWidth;
            SystemTray.IsVisible = false; //to hide system tray
        }
    }
}
