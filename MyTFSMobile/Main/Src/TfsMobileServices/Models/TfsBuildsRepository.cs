using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using TfsMobile.Contracts;

namespace TfsMobileServices.Models
{
    public class TfsBuildsRepository
    {
        public IEnumerable<BuildContract> GetMyBuilds(TfsService tf, string project, int fromDays)
        {
            using (var instance = tf.Connect())
            {
                var buildServer = (IBuildServer) instance.GetService(typeof (IBuildServer));
                var buildSpec = GetBuildDetailSpec(buildServer, project, fromDays);

                buildSpec.RequestedFor = (tf.UseLocalAccount)
                    ? instance.AuthorizedIdentity.DisplayName
                    : tf.NetCredentials.UserName;

                IBuildQueryResult details = buildServer.QueryBuilds(buildSpec);
                IEnumerable<BuildContract> result = CreateBuildResults(details);
                return result;
            }
        }

        public IEnumerable<BuildContract> GetBuildDefinitions(TfsService tf, string project)
        {
            using (var instance = tf.Connect())
            {
                var buildServer = (IBuildServer)instance.GetService(typeof(IBuildServer));
                var buildSpec = GetBuildDetailSpec(buildServer, project, 365);
                IBuildQueryResult details = buildServer.QueryBuilds(buildSpec);
                IEnumerable<BuildContract> result = CreateBuildResults(details);
                return result;
            }
        }

        public IEnumerable<BuildContract> GetTeamBuilds(TfsService tf, string project, int fromDays)
        {
            using (var instance = tf.Connect())
            {
                var buildServer = (IBuildServer)instance.GetService(typeof(IBuildServer));
                var buildSpec = GetBuildDetailSpec(buildServer, project, fromDays);
                IBuildQueryResult details = buildServer.QueryBuilds(buildSpec);
                IEnumerable<BuildContract> result = CreateBuildResults(details);
                return result;
            }
        }

        private static IEnumerable<BuildContract> CreateBuildResults(IBuildQueryResult details)
        {
            if (details != null && details.Builds != null)
            {
                IBuildDetail[] buildDetails = BuildUtility.FilterMontoredBuildsOnly(details);
                var builds = buildDetails.Where(bd => bd.BuildDefinition.Enabled).Select(BuildFactory.CreateBuild);
                return BuildUtility.GetUniqueBuilds(builds);
            }
            return new List<BuildContract>();
        }

        private IBuildDetailSpec GetBuildDetailSpec(IBuildServer buildServer, string project, int fromDays)
        {
            IBuildDetailSpec buildSpec;
            buildSpec = buildServer.CreateBuildDetailSpec(project);
            buildSpec.Status = BuildStatus.Failed | BuildStatus.InProgress | BuildStatus.PartiallySucceeded | BuildStatus.Succeeded;
            buildSpec.MinFinishTime = DateTime.Now.AddDays(-fromDays); //DateTime.Now.AddHours(-10);
            buildSpec.InformationTypes = null; // for speed improvement
            buildSpec.MaxBuildsPerDefinition = 5; //get only one build per build definintion
            buildSpec.QueryOrder = BuildQueryOrder.FinishTimeDescending; //get the latest build only
            buildSpec.QueryOptions = QueryOptions.All;
      
            return buildSpec;
        }

        //private static IEnumerable<BuildContract> CreateBuildContractsFromBuildResults(IBuildQueryResult builds)
        //{
        //    return builds.Builds.Select(
        //        b =>
        //            new BuildContract
        //            {
        //                FinishTime = b.FinishTime,
        //                Name = b.BuildDefinition.Name,
        //                Status = b.Status.ToString(),
        //            }).ToList();
        //}

        //private static IEnumerable<BuildContract> CreateBuildContractsFromBuildResults(IEnumerable<IBuildDefinition> builds)
        //{
        //    return builds.OrderBy(c=>c.Name).Select(
        //        b =>
        //            new BuildContract
        //            {
        //                FinishTime = b.DateCreated,
        //                Name = b.Name,
        //                Status = "BuildDefinition",

        //            }).ToList();
        //}
                
        public void QueueBuild(TfsService tf, string teamProject, string buildName)
        {
            using (var tfsInstance = tf.Connect())
            {
                var buildServer = (IBuildServer)tfsInstance.GetService(typeof(IBuildServer));
                var buildDef = buildServer.GetBuildDefinition(teamProject, buildName);
                buildServer.QueueBuild(buildDef);
            }
            
        }
    }

    public static class BuildFactory
    {
        public static BuildContract CreateBuild(IBuildDetail bd)
        {
            var contract = new BuildContract()
            {
                Name = BuildUtility.GetBuildNameWithBranchName(bd),
                TeamProject = bd.BuildDefinition.TeamProject,
                Status = GetStatus(bd.Status),
                StartTime = bd.StartTime,
                RequestedBy = bd.RequestedBy,
                RequestedFor = bd.RequestedFor,
                FinishTime = bd.BuildFinished ? bd.FinishTime : (DateTime?) null
            };
            return contract;
        }

        private static string GetStatus(BuildStatus status)
        {
            switch (status)
            {
                case BuildStatus.Failed:
                    return "Failed";
                case BuildStatus.PartiallySucceeded:
                    return "PartiallySucceeded";
                case BuildStatus.InProgress:
                    return "InProgress";
                case BuildStatus.Succeeded:
                    return "Succeeded";
                default:
                    return "Succeeded";
            }
        }
    }

    public static class BuildUtility
    {
        private const string BuildMustContainThisToBeMonitored = "refs/heads";
        private const string TfsVsStartsWithThis = "C";
        public static string GetBuildNameWithBranchName(IBuildDetail bd)
        {
            var sourceGetVersion = bd.SourceGetVersion;
            if (IsGitBuild(sourceGetVersion))
            {
                var versionParts = sourceGetVersion.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (versionParts.Any())
                {
                    var branchName =
                        versionParts.Last()
                            .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList()
                            .FirstOrDefault();
                    return bd.BuildDefinition.Name + "/" + branchName;
                }
            }
            return bd.BuildDefinition.Name;
        }

        private static bool IsGitBuild(string sourceGetVersion)
        {
            return sourceGetVersion.Contains(BuildMustContainThisToBeMonitored);
        }

        public static IEnumerable<BuildContract> GetUniqueBuilds(IEnumerable<BuildContract> builds)
        {
            HashSet<BuildContract> list = HashSetHelper<BuildContract>.Create(b => b.Name);
            var orderedListOfBuilds = builds.Select(b => b).OrderByDescending(z => z.FinishTime);
            foreach (var build in orderedListOfBuilds)
            {
                list.Add(build);
            }
            return list;
        }

        public static IBuildDetail[] FilterMontoredBuildsOnly(IBuildQueryResult details)
        {
            return details.Builds.Where(b => (b.SourceGetVersion.Contains(BuildMustContainThisToBeMonitored) || b.SourceGetVersion.StartsWith(TfsVsStartsWithThis))).ToArray();
        }
    }

    public static class HashSetHelper<T>
    {
        class Wrapper<TValue> : IEqualityComparer<T>
        {
            private readonly Func<T, TValue> func;
            private readonly IEqualityComparer<TValue> comparer;
            public Wrapper(Func<T, TValue> func,
                IEqualityComparer<TValue> comparer)
            {
                this.func = func;
                this.comparer = comparer ?? EqualityComparer<TValue>.Default;
            }
            public bool Equals(T x, T y)
            {
                return comparer.Equals(func(x), func(y));
            }

            public int GetHashCode(T obj)
            {
                return comparer.GetHashCode(func(obj));
            }
        }
        public static HashSet<T> Create<TValue>(Func<T, TValue> func)
        {
            return new HashSet<T>(new Wrapper<TValue>(func, null));
        }
        public static HashSet<T> Create<TValue>(Func<T, TValue> func,
            IEqualityComparer<TValue> comparer)
        {
            return new HashSet<T>(new Wrapper<TValue>(func, comparer));
        }
    }
}