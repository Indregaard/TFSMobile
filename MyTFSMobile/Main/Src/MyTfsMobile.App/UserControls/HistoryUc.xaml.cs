using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyTfsMobile.App.ViewModel;

namespace MyTfsMobile.App.UserControls
{
    public partial class HistoryUc : UserControl
    {
        private HistoryViewModel vm;
        public HistoryUc()
        {
            InitializeComponent();
        }

        private void HistoryUc_OnLoaded(object sender, RoutedEventArgs e)
        {
            vm = new HistoryViewModel();

            DataContext = vm;
            if (!vm.IsDataLoaded)
                vm.LoadData();
        }
    }
}
