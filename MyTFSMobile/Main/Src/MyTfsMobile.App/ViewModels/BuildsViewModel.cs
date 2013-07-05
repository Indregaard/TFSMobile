using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTfsMobile.App.enums;

namespace MyTfsMobile.App.ViewModels
{
    internal class BuildsViewModel : ViewModelBase
    {
        public BuildsViewModel()
        {
            BuildItems = new ObservableCollection<BuildViewModel>();
        }

        public ObservableCollection<BuildViewModel> BuildItems { get; private set; }


        // my/favourite/all team buils
        private string buildSection = "Builds";
        public string BuildSection
        {
            get { return buildSection; }
            set{ 
                if(buildSection == value) return;
                buildSection = value;
                NotifyPropertyChanged("BuildSection");
            }
        }

        public bool IsDataLoaded { get; private set; }

        public void LoadData()
        {
            BuildItems.Add(new BuildViewModel { BuildId = 1001, BuildName = "MyTfsMobile.ci", BuildDate = new DateTime(2013, 7, 5, 22, 22, 45), BuildStatus = BuildStatus.Failed, BuildMessages = @"No such folder c:\Nofolder"});
            BuildItems.Add(new BuildViewModel { BuildId = 1002, BuildName = "MyTfsMobile.ci", BuildDate = new DateTime(2013, 7, 5, 22, 54, 12), BuildStatus = BuildStatus.Partial, BuildMessages = @"0/2 unit tests passed...blablabla" });
            BuildItems.Add(new BuildViewModel { BuildId = 1003, BuildName = "MyTfsMobile.ci", BuildDate = new DateTime(2013, 7, 5, 23, 02, 33), BuildStatus = BuildStatus.Ok, BuildMessages = @"" });
            IsDataLoaded = true;
        }
    }
}
