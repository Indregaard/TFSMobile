using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsMobile.Contracts;

namespace TfsMobileServices.Models
{
    public class TfsHistoryRepository : BaseRepository
    {
        public TfsHistoryRepository(Uri tfsUri, NetworkCredential cred) : base(tfsUri, cred)
        {
        }

        public IEnumerable<HistoryItemContract> GetHistory(string project, int fromDays)
        {
            using (var tfs = Tf.Connect())
            {
                //--> There is no team project named...
                var structService = tfs.GetService<ICommonStructureService>();
                var tp = structService.ListAllProjects().FirstOrDefault(a => a.Name == project);


                var tfsBaseUri = GetTfsBaseUri(Tf, tp);
                var workItemStore = tfs.GetService<WorkItemStore>();
                // 
                var from = (fromDays > 10) ? 10 : fromDays;

                var query = "Select [Id],[Area Path], [Work Item Type], [Title], [State], [Iteration Path], [Changed Date], [Changed By], [Created Date], [Created By], [Iteration Path] From WorkItems Where ((([Changed Date] > @Today - [FROM]) AND ([Changed By] = @Me)) || (([Created Date] > @Today - [FROM]) AND ([Created By] = @Me)))";
                //var query2 = "Select [Id],[Area Path], [Work Item Type], [Title], [State], [Iteration Path], [Changed Date], [Changed By], [Created Date], [Created By], [Iteration Path] From WorkItems Where ((([Changed Date] > @Today - 7) AND ([Changed By] = @Me)) || (([Created Date] > @Today - 7) AND ([Created By] = @Me)))";
                var query2 = query.Replace("[FROM]", from.ToString(CultureInfo.InvariantCulture));

                var historyItems = new List<HistoryItemContract>();


                foreach (var wi in workItemStore.Query(query2).Cast<WorkItem>())
                {
                    var historyItem = new HistoryItemContract
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

        public string GetTfsBaseUri(TfsService tfs, ProjectInfo project)
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
}