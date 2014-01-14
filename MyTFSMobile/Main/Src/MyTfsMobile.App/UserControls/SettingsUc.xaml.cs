using System.Windows;
using Microsoft.Phone.Shell;
using MyTfsMobile.App.ViewModel;

namespace MyTfsMobile.App.UserControls
{
    public partial class SettingsUc
    {
        private SettingsViewModel pageViewModel;

        public SettingsUc()
        {
            InitializeComponent();
            LayoutRoot.Height = Application.Current.Host.Content.ActualHeight;
            LayoutRoot.Width = Application.Current.Host.Content.ActualWidth;
            //SystemTray.IsVisible = false; 
            SetDataContext();
        }

        private void SetDataContext()
        {
            pageViewModel = new SettingsViewModel();
            DataContext = pageViewModel;
        }
    }
}
