using System;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Input.Touch;
using MyTfsMobile.App.ViewModel;

namespace MyTfsMobile.App
{
    public partial class History : PhoneApplicationPage
    {
        private HistoryViewModel vm;
        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!vm.IsDataLoaded)
            {
                vm.LoadData();
            }
        }

        public History()
        {
            InitializeComponent();
            vm = new HistoryViewModel();

            DataContext = vm;

            TouchPanel.EnabledGestures = GestureType.Flick;
        
        }
     

        private void Builds_OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                if (gesture.GestureType == GestureType.Flick)
                {
                    // determine dir
                    var modifier = gesture.Delta.X > 0 ? -1 : 1;

                    if (modifier < 0)
                    {
                        LoadNextPage();

                    }
                    else
                    {
                        LoadPreviousPage();
                    }
                }
            }
        }

        private void LoadNextPage()
        {
            NavigationService.Navigate(new Uri("/Builds.xaml", UriKind.Relative));
        }

        private void LoadPreviousPage()
        {
            NavigationService.Navigate(new Uri("/Builds.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/TfsSettings.xaml", UriKind.Relative));
        }
    }
}