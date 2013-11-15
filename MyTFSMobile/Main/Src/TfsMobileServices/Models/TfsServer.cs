using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Microsoft.TeamFoundation;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
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
        public IEnumerable<BuildContract> GetBuilds(TfsService2 tf, string project, int fromDays)
        {
            using (var instance = tf.Connect())
            {
                IBuildServer buildServer = (IBuildServer) instance.GetService(typeof (IBuildServer));
                IBuildDetailSpec buildSpec;
                buildSpec = buildServer.CreateBuildDetailSpec(project);
                buildSpec.RequestedFor = (tf.UseLocalAccount)
                    ? instance.AuthorizedIdentity.DisplayName
                    : tf.NetCredentials.UserName;
                buildSpec.MinFinishTime = DateTime.Now.AddDays(-fromDays); //DateTime.Now.AddHours(-10);
                buildSpec.InformationTypes = null; // for speed improvement
                buildSpec.MaxBuildsPerDefinition = 1; //get only one build per build definintion
                buildSpec.QueryOrder = BuildQueryOrder.FinishTimeDescending; //get the latest build only
                buildSpec.QueryOptions = QueryOptions.All;
                try
                {
                    var ibuilds = buildServer.QueryBuilds(buildSpec);
                    var builds =
                        ibuilds.Builds.Select(
                            b =>
                                new BuildContract
                                {
                                    FinishTime = b.FinishTime,
                                    Name = b.BuildDefinition.Name,
                                    Status = b.Status.ToString()
                                }).ToList();

                    return builds;
                }
                catch (Exception ex)
                {
                    return new List<BuildContract>();
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
                VersionControlServer sourceControl = (VersionControlServer)tfs.GetService(typeof(VersionControlServer));
                var tp = sourceControl.GetTeamProject(project);
                var tfsBaseUri = GetTfsBaseUri(tp, project);
                //tp.TeamProjectCollection.CatalogNode.Dump();
                var workItemStore = tfs.GetService<WorkItemStore>();
                // 
                var from = (fromDays > 10) ? 10 : fromDays;
                //DateTime fromDate = DateTime.Now.AddDays(-from).Date;
                //var id = 40008;

                //var query = string.Format(@"Select [Id], [Work Item Type], [Title], [State] From WorkItems Where [Id] == {0}", id);
                var query = "Select [Id],[Area Path], [Work Item Type], [Title], [State], [Iteration Path], [Changed Date], [Changed By], [Created Date], [Created By], [Iteration Path] From WorkItems Where ((([Changed Date] > @Today - [FROM]) AND ([Changed By] = @Me)) || (([Created Date] > @Today - [FROM]) AND ([Created By] = @Me)))";
                //var query2 = "Select [Id],[Area Path], [Work Item Type], [Title], [State], [Iteration Path], [Changed Date], [Changed By], [Created Date], [Created By], [Iteration Path] From WorkItems Where ((([Changed Date] > @Today - 7) AND ([Changed By] = @Me)) || (([Created Date] > @Today - 7) AND ([Created By] = @Me)))";
                var query2 = query.Replace("[FROM]", from.ToString(CultureInfo.InvariantCulture));


                
                var res = workItemStore.Query(query2).Cast<WorkItem>()
                    .Select(wi => new
                    {
                        wi.Id,
                        wi.AreaPath,
                        Type = wi.Type.Name,
                        wi.Title,
                        wi.State,
                        wi.ChangedDate,
                        wi.CreatedDate,
                        wi.CreatedBy,
                        wi.ChangedBy,
                        wi.IterationPath
                    })
                    .OrderBy(wi => wi.ChangedDate).ThenBy(wi => wi.Type).ThenBy(wi => wi.Title);

                var historyItems = new List<HistoryItemContract>(); 
                foreach (var item in res)
                {
                    var historyItem = new HistoryItemContract()
                    {
                        Description = item.Title,Id = item.Id,HistoryDate = GetHistoryDate(item.CreatedDate,item.ChangedDate),TfsItemUri = GetWorkItemTfsUri(item.Id,tfsBaseUri),HistoryItemType = "WI",WorkType = item.Type,State = item.State,AreaPath = item.AreaPath,IterationPath = item.IterationPath
                    };
                    historyItems.Add(historyItem);
       
                }

                var changesets = sourceControl.QueryHistory("$/*", VersionSpec.Latest, 0, RecursionType.Full, tfs.AuthorizedIdentity.DisplayName, new DateVersionSpec(DateTime.Now.AddDays(-from).Date), new DateVersionSpec(DateTime.Now.AddDays(1).Date), 50, true, false);
                foreach (var changeset in changesets.OfType<Changeset>())
                {
                    var item = 
                    new HistoryItemContract()
                    {
                        HistoryItemType = "CS",
                        Id = changeset.ChangesetId,
                        Description = changeset.Comment,
                        HistoryDate = changeset.CreationDate,
                        TfsItemUri = GetChangeSetTfsUri(changeset.ChangesetId, tfsBaseUri)
                        //WorkItems = changeset.WorkItems
                    };
                    historyItems.Add(item);
                }
                return historyItems;


//                var tfs = TfsTeamProjectCollectionFactory
//    .GetTeamProjectCollection(new Uri("http://tfs.osiris.no:8080/tfs"));
//tfs.EnsureAuthenticated();

////TfsTeamProjectCollection tfsConnection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri("http://TFS:8080/TFS/DefaultCollection"));
//VersionControlServer sourceControl = (VersionControlServer)tfs.GetService(typeof(VersionControlServer));

//var changesets = sourceControl.QueryHistory("$/*", VersionSpec.Latest, 0, RecursionType.Full, "torarnev", new DateVersionSpec (new DateTime(2013,09,28)), new DateVersionSpec (new DateTime(2013,10,02)), Int32.MaxValue, true, false);
////foreach (Changeset change in changesets)
            }
            
        }

        private DateTime GetHistoryDate(DateTime created, DateTime changed)
        {
            return (created > changed) ? created : changed;
        }

        public string GetTfsBaseUri(TeamProject teamProject, string project)
        {
            var collectionName = teamProject.TeamProjectCollection.CatalogNode.Resource.DisplayName;
            var tfsUri = teamProject.TeamProjectCollection.Uri;
            return tfsUri + "/" + collectionName + "/" + project + "/";
        }
        private Uri GetWorkItemTfsUri(int wid, string tfsBaseUri)
        {
            return new Uri(tfsBaseUri + "/_workitems#id=" + wid.ToString(CultureInfo.InvariantCulture));             
        }

        private Uri GetChangeSetTfsUri(int csid, string tfsBaseUri)
        {
            return new Uri(tfsBaseUri + "/_versionControl/changeset/" + csid.ToString(CultureInfo.InvariantCulture));
        }
    }
    public class TfsService2 : IDisposable
    {
        public bool UseLocalAccount { get; private set; }
        private Uri TfsUri { get; set; }
        
        public NetworkCredential NetCredentials { get; private set; }
        
        private TfsTeamProjectCollection Tp { get; set; }


        public TfsService2(){}
       
  

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
}