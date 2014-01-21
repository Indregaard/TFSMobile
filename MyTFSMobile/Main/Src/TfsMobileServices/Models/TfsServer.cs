using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Microsoft.TeamFoundation;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsMobile.Contracts;

namespace TfsMobileServices.Models
{
    public class TfsServiceFactory
    {

        public static TfsService2 Get(Uri tfsUri, NetworkCredential cred)
        {
            return new TfsService2(tfsUri, cred);
        }

    }


    public class TfsBuildsRepository
    {
        public IEnumerable<BuildContract> GetMyBuilds(TfsService2 tf, string project, int fromDays)
        {
            using (var instance = tf.Connect())
            {
                var buildServer = (IBuildServer) instance.GetService(typeof (IBuildServer));
                var buildSpec = GetBuildDetailSpec(buildServer, project, fromDays);
               
                buildSpec.RequestedFor = (tf.UseLocalAccount)
                    ? instance.AuthorizedIdentity.DisplayName
                    : tf.NetCredentials.UserName;
                
                var ibuilds = buildServer.QueryBuilds(buildSpec);
                return CreateBuildContractsFromBuildResults(ibuilds);
            }
        }

        public IEnumerable<BuildContract> GetBuildDefinitions(TfsService2 tf, string project)
        {
            using (var instance = tf.Connect())
            {
                var buildServer = (IBuildServer)instance.GetService(typeof(IBuildServer));
                var ibuilds = buildServer.QueryBuildDefinitions(project);
                return CreateBuildContractsFromBuildResults(ibuilds);
            }
        }

        public IEnumerable<BuildContract> GetTeamBuilds(TfsService2 tf, string project, int fromDays)
        {
            using (var instance = tf.Connect())
            {
                var buildServer = (IBuildServer)instance.GetService(typeof(IBuildServer));
                var buildSpec = GetBuildDetailSpec(buildServer, project, fromDays);
                var ibuilds = buildServer.QueryBuilds(buildSpec);
                return CreateBuildContractsFromBuildResults(ibuilds);
            }
        }

        private IBuildDetailSpec GetBuildDetailSpec(IBuildServer buildServer, string project, int fromDays)
        {
            IBuildDetailSpec buildSpec;
            buildSpec = buildServer.CreateBuildDetailSpec(project);
                buildSpec.MinFinishTime = DateTime.Now.AddDays(-fromDays); //DateTime.Now.AddHours(-10);
                buildSpec.InformationTypes = null; // for speed improvement
                buildSpec.MaxBuildsPerDefinition = 1; //get only one build per build definintion
                buildSpec.QueryOrder = BuildQueryOrder.FinishTimeDescending; //get the latest build only
                buildSpec.QueryOptions = QueryOptions.All;

            return buildSpec;
        }

        private static IEnumerable<BuildContract> CreateBuildContractsFromBuildResults(IBuildQueryResult builds)
        {
            return builds.Builds.Select(
                        b =>
                            new BuildContract
                            {
                                FinishTime = b.FinishTime,
                                Name = b.BuildDefinition.Name,
                                Status = b.Status.ToString(),
                            }).ToList();
        }

        private static IEnumerable<BuildContract> CreateBuildContractsFromBuildResults(IEnumerable<IBuildDefinition> builds)
        {
            return builds.OrderBy(c=>c.Name).Select(
                        b =>
                            new BuildContract
                            {
                                FinishTime = b.DateCreated,
                                Name = b.Name,
                                Status = "BuildDefinition",
                            }).ToList();
        }
                
        public void QueueBuild(TfsService2 tf, string teamProject, string buildName)
        {
            using (var tfsInstance = tf.Connect())
            {
                var buildServer = (IBuildServer)tfsInstance.GetService(typeof(IBuildServer));
                var buildDef = buildServer.GetBuildDefinition(teamProject, buildName);
                buildServer.QueueBuild(buildDef);
            }
            
        }
            }
        }



public class TfsHistoryRepository
{
    public IEnumerable<HistoryItemContract> GetHistory(TfsService2 tf, string project, int fromDays)
    {
        using (var tfs = tf.Connect())
        {
            //--> There is no team project named...
            var structService = tfs.GetService<Microsoft.TeamFoundation.Server.ICommonStructureService>();
            var tp = structService.ListAllProjects().FirstOrDefault(a => a.Name == project);


            var tfsBaseUri = GetTfsBaseUri(tf, tp);
            var workItemStore = tfs.GetService<WorkItemStore>();
            // 
            var from = (fromDays > 10) ? 10 : fromDays;

            var query = "Select [Id],[Area Path], [Work Item Type], [Title], [State], [Iteration Path], [Changed Date], [Changed By], [Created Date], [Created By], [Iteration Path] From WorkItems Where ((([Changed Date] > @Today - [FROM]) AND ([Changed By] = @Me)) || (([Created Date] > @Today - [FROM]) AND ([Created By] = @Me)))";
            //var query2 = "Select [Id],[Area Path], [Work Item Type], [Title], [State], [Iteration Path], [Changed Date], [Changed By], [Created Date], [Created By], [Iteration Path] From WorkItems Where ((([Changed Date] > @Today - 7) AND ([Changed By] = @Me)) || (([Created Date] > @Today - 7) AND ([Created By] = @Me)))";
            var query2 = query.Replace("[FROM]", from.ToString(CultureInfo.InvariantCulture));

            var historyItems = new List<HistoryItemContract>();


            foreach (var wi in workItemStore.Query(query2).Cast<WorkItem>())
            {
                var historyItem = new HistoryItemContract()
                {
                    Title = wi.Title,
                    Description = wi.Description,
                    Id = wi.Id,
                    HistoryDate = GetHistoryDate(wi.CreatedDate, wi.ChangedDate),
                    TfsItemUri = GetWorkItemTfsUri(wi.Id, tfsBaseUri),
                    WorkType = wi.Type.Name,
                    State = wi.State,
                    AreaPath = wi.AreaPath,
                    IterationPath = wi.IterationPath,

                };

                historyItems.Add(historyItem);

            }

            return historyItems;

        }

    }


    private DateTime GetHistoryDate(DateTime created, DateTime changed)
    {
        return (created > changed) ? created : changed;
    }

    public string GetTfsBaseUri(TfsService2 tfs, ProjectInfo project)
    {

        var collectionName = tfs.CollectionDisplayName;
        var tfsUri = tfs.TfsUri.ToString();
        return tfsUri + "/" + collectionName + "/" + project.Name + "/";
    }
    private Uri GetWorkItemTfsUri(int wid, string tfsBaseUri)
    {
        return new Uri(tfsBaseUri + "/_workitems#id=" + wid.ToString(CultureInfo.InvariantCulture));
    }

    //private Uri GetChangeSetTfsUri(int csid, string tfsBaseUri)
    //{
    //    return new Uri(tfsBaseUri + "/_versionControl/changeset/" + csid.ToString(CultureInfo.InvariantCulture));
    //}
}
public class TfsService2 : IDisposable
{
    public bool UseLocalAccount { get; private set; }
    public Uri TfsUri { get; private set; }

    public NetworkCredential NetCredentials { get; private set; }

    private TfsTeamProjectCollection Tp { get; set; }

    public string CollectionDisplayName { get; private set; }

    public TfsService2() { }



    public TfsService2(Uri tfsUri, NetworkCredential cred)
    {
        NetCredentials = cred;
        UseLocalAccount = false;
        TfsUri = tfsUri;
    }

    public TfsTeamProjectCollection Connect()
    {
        Tp = null;

        if (TfsUri != null)
        {
            try
            {
                Tp = new TfsTeamProjectCollection(TfsUri);
                Tp.Credentials = NetCredentials;
                Tp.EnsureAuthenticated();
                CollectionDisplayName = Tp.CatalogNode.Resource.DisplayName;
            }
            catch (TeamFoundationServerUnauthorizedException)
            {
            }
        }
        return Tp;
    }




    #region IDisposable Members

    public void Dispose()
    {
        Tp = null;
    }

    #endregion // IDisposable Members


}
