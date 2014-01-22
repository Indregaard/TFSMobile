using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using MyTfsMobile.App.enums;
using Newtonsoft.Json;
using TfsMobile.Contracts;
using TfsMobile.Repositories.v1;

namespace MyTfsMobile.App.ViewModel
{
    internal class BuildsViewModel : BaseViewModel
    {
        private readonly BuildSection section;
        public BuildsViewModel()
            : this(BuildSection.MyBuilds)
        {
        }

        public BuildsViewModel(BuildSection section)
        {
            BuildItems = new ObservableCollection<BuildViewModel>();
            this.section = section;

        }

        public ObservableCollection<BuildViewModel> BuildItems { get; private set; }

        private string buildSectionHeader = "Builds";
        public string BuildSectionHeader
        {
            get { return buildSectionHeader; }
            set
            {
                if (buildSectionHeader == value) return;
                buildSectionHeader = value;
                RaisePropertyChanged("BuildSection");
            }
        }

        public bool IsDataLoaded { get; private set; }
        public override async void LoadData()
        {
            switch (section)
            {
                case BuildSection.MyBuilds:
                    await GetMyBuilds();
                    break;
                case BuildSection.TeamBuilds:
                    await GetTeamBuilds();
                    break;
            }
        }

        private async Task GetMyBuilds()
        {
            PrepareBuildSection("My Builds");

            var serviceAccessw = Locator.TfsAuthenticationService.CheckTfsLogin(Locator.MyTfsMobileSettings.TfsSettings);

            var access = await serviceAccessw;
            if (!access) return;


            Messenger.Default.Send(true, "ShowLoadPopup");

            var tfsUserDto = Locator.TfsAuthenticationService.CreateTfsUserDto();
            var buildsRepo = new BuildsRepository(tfsUserDto, false);
            var buildsResult = await buildsRepo.GetBuildsAsync(BuildDetailsDto.Default());
            var buildContracts = JsonConvert.DeserializeObject<List<BuildContract>>(buildsResult);
            FillBuildSection(buildContracts);
        }

        private void PrepareBuildSection(string sectionName)
        {
            IsDataLoaded = false;
            BuildSectionHeader = sectionName;
            BuildItems.Clear();
        }

        private void FillBuildSection(IEnumerable<BuildContract> buildContracts)
        {
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

            Messenger.Default.Send(true, "CloseLoadPopup");
        }

        private async Task GetTeamBuilds()
        {
            PrepareBuildSection("Team Builds");

            var serviceAccessw = Locator.TfsAuthenticationService.CheckTfsLogin(Locator.MyTfsMobileSettings.TfsSettings);

            var access = await serviceAccessw;
            if (!access) return;

            var tfsUserDto = Locator.TfsAuthenticationService.CreateTfsUserDto();
            var buildsRepo = new BuildsRepository(tfsUserDto, false);
            var buildsResult = await buildsRepo.GetTeamBuildsAsync(BuildDetailsDto.Default());
            var buildContracts = JsonConvert.DeserializeObject<List<BuildContract>>(buildsResult);
            FillBuildSection(buildContracts);
        }


        private async Task GetBuildDefinitions()
        {

            PrepareBuildSection("Build definitions");
            //var tfsUserDto = viewModelLocator.Settings.CreateTfsUserDto();
            //var buildsRepo = new BuildsRepository(tfsUserDto, false);
            //var buildsResult = await buildsRepo.GetBuildDefinitionsAsync(BuildDetailsDto.Default());
            //var buildContracts = JsonConvert.DeserializeObject<List<BuildContract>>(buildsResult);
            //FillBuildSection(buildContracts);
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
            var queueBuildDto = new QueueBuildDto { TfsProject = "Main", BuildName = build.BuildName };
            //var tfsUserDto = viewModelLocator.Settings.CreateTfsUserDto();
            //var buildsRepo = new BuildsRepository(tfsUserDto, false);
            //await buildsRepo.QueueBuild(queueBuildDto);
            
            return true;
        }
    }
}
