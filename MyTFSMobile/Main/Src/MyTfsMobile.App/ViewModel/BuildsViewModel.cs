using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MyTfsMobile.App.enums;
using MyTfsMobile.App.ViewModel;
using Newtonsoft.Json;
using TfsMobile.Contracts;
using TfsMobile.Repositories.v1;

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
            set
            {
                if (buildSection == value) return;
                buildSection = value;
                RaisePropertyChanged("BuildSection");
            }
        }

        public bool IsDataLoaded { get; private set; }
        private static ViewModelLocator viewModelLocator = new ViewModelLocator();
        public async void LoadData()
        {
            await GetMyBuilds();

        }

        private async Task GetMyBuilds()
        {
            BuildSection = "My Builds";
            BuildItems.Clear();
            var tfsUserDto = viewModelLocator.Settings.CreateTfsUserDto();
            var buildsRepo = new BuildsRepository(tfsUserDto, false);
            var buildsResult = await buildsRepo.GetBuildsAsync(BuildDetailsDto.Default());
            var buildContracts = JsonConvert.DeserializeObject<List<BuildContract>>(buildsResult);
            foreach (var build in buildContracts)
            {
                BuildItems.Add(new BuildViewModel
                {
                    BuildId = 1,
                    BuildName = build.Name,
                    BuildDate = build.FinishTime,
                    BuildStatus = BuildStatusConverter.GetFromString(build.Status),
                    BuildMessages = @""
                });
            }
            IsDataLoaded = true;
        }

        private async Task GetTeamBuilds()
        {

            BuildSection = "Team Builds";
            BuildItems.Clear();
            var tfsUserDto = viewModelLocator.Settings.CreateTfsUserDto();
            var buildsRepo = new BuildsRepository(tfsUserDto, false);
            var buildsResult = await buildsRepo.GetTeamBuildsAsync(BuildDetailsDto.Default());
            var buildContracts = JsonConvert.DeserializeObject<List<BuildContract>>(buildsResult);
            foreach (var build in buildContracts)
            {
                BuildItems.Add(new BuildViewModel
                {
                    BuildId = 1,
                    BuildName = build.Name,
                    BuildDate = build.FinishTime,
                    BuildStatus = BuildStatusConverter.GetFromString(build.Status),
                    BuildMessages = @""
                });
            }
            IsDataLoaded = true;
        }


        private async Task GetBuildDefinitions()
        {

            BuildSection = "Build definitions";
            BuildItems.Clear();
            var tfsUserDto = viewModelLocator.Settings.CreateTfsUserDto();
            var buildsRepo = new BuildsRepository(tfsUserDto, false);
            var buildsResult = await buildsRepo.GetBuildDefinitionsAsync(BuildDetailsDto.Default());
            var buildContracts = JsonConvert.DeserializeObject<List<BuildContract>>(buildsResult);
            foreach (var build in buildContracts)
            {
                BuildItems.Add(new BuildViewModel
                {
                    BuildId = 1,
                    BuildName = build.Name,
                    BuildDate = build.FinishTime,
                    BuildStatus = BuildStatusConverter.GetFromString(build.Status),
                    BuildMessages = @""
                });
            }
            IsDataLoaded = true;
        }

        private RelayCommand<BuildViewModel> queueBuildCommand;
        public RelayCommand<BuildViewModel> QueueBuildCommand
        {
            get
            {
                return queueBuildCommand ?? (queueBuildCommand = new RelayCommand<BuildViewModel>(
                    async (build) =>
                    {
                        await QueueBuild(build);
                    }));
            }
        }

        private RelayCommand<BuildViewModel> myBuildsCommand;
        public RelayCommand<BuildViewModel> MyBuildsCommand
        {
            get
            {
                return myBuildsCommand ?? (myBuildsCommand = new RelayCommand<BuildViewModel>(
                   async (build) =>
                   {
                       await GetMyBuilds();
                   }));
            }
        }

        private RelayCommand<BuildViewModel> teamBuildsCommand;
        public RelayCommand<BuildViewModel> TeamBuildsCommand
        {
            get
            {
                return teamBuildsCommand ?? (teamBuildsCommand = new RelayCommand<BuildViewModel>(
                   async (build) =>
                   {
                       await GetTeamBuilds();
                   }));
            }
        }

        private RelayCommand<BuildViewModel> buildDefintionsCommand;
        public RelayCommand<BuildViewModel> BuildDefintionsCommand
        {
            get
            {
                return buildDefintionsCommand ?? (buildDefintionsCommand = new RelayCommand<BuildViewModel>(
                   async (build) =>
                   {
                       await GetBuildDefinitions();
                   }));
            }
        }


        static async private Task<bool> QueueBuild(BuildViewModel build)
        {
            var queueBuildDto = new QueueBuildDto { TfsProject = "Byggtjeneste - Projects", BuildName = build.BuildName };
            var tfsUserDto = viewModelLocator.Settings.CreateTfsUserDto();
            var buildsRepo = new BuildsRepository(tfsUserDto, false);
            await buildsRepo.QueueBuild(queueBuildDto);
            
            return true;
        }
    }
}
