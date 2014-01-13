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
    public partial class BuildsUc : UserControl
    {
        private BuildsViewModel vm;

        public BuildsUc()
        {
            InitializeComponent();
            vm = new BuildsViewModel();

            DataContext = vm;

            if (!vm.IsDataLoaded)
            {
                vm.LoadData();
            }
        }

    }
}
