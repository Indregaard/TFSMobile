using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Input.Touch;
using MyTfsMobile.App.enums;
using MyTfsMobile.App.ViewModel;

namespace MyTfsMobile.App
{
    public partial class Builds : PhoneApplicationPage
    {
        private BuildsViewModel vm;
        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!vm.IsDataLoaded)
            {
                vm.LoadData();
            }
        }

        public Builds()
        {
            InitializeComponent();
            vm = new BuildsViewModel(BuildSection.MyBuilds);

            DataContext = vm;

            TouchPanel.EnabledGestures = GestureType.Flick;
        
        }

        //private void Test_OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        //{
            
        //    while (TouchPanel.IsGestureAvailable)
        //    {
        //        GestureSample gesture = TouchPanel.ReadGesture();
        //        if (gesture.GestureType == GestureType.Flick)
        //        {
        //            // determine dir
        //            var modifier = gesture.Delta.X > 0 ? -1 : 1;

        //            test.Text = "Flicked";
        //        }
        //    }
        //}

        private void MainLongListSelector_OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
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
                        ((UIElement)sender).RenderTransform = new TranslateTransform() { X = 0 };
                        
                    }
                    else
                    {
                        // slide left
                        var s = (StackPanel) sender;
                        //((UIElement) sender).RenderTransform = new TranslateTransform() {X = -50};
                        var buildImage = s.FindName("BuildImage");
                        var image = buildImage as Image;
                        if (image != null)
                        {
                            var img = image;

                            img.Visibility = Visibility.Collapsed; 
                            //img.RenderTransform = new TranslateTransform {X = 50, Y = 0};

                        }

                    }


                }
            }   
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
            NavigationService.Navigate(new Uri("/History.xaml", UriKind.Relative));
        }

        private void LoadPreviousPage()
        {
            NavigationService.Navigate(new Uri("/History.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/TfsSettings.xaml", UriKind.Relative));
        }
    }
}