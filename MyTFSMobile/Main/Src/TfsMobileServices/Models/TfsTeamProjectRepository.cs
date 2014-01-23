using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.TeamFoundation.VersionControl.Client;
using TfsMobile.Contracts;

namespace TfsMobileServices.Models
{
    public class TfsTeamProjectRepository : BaseRepository
    {
        public TfsTeamProjectRepository(Uri tfsUri, NetworkCredential cred) : base(tfsUri, cred)
        {
        }


        public IEnumerable<TeamProjectContract> GetAll()
        {
            using (var tfs = Tf.Connect())
            {
                var structService = tfs.GetService<Microsoft.TeamFoundation.Server.ICommonStructureService>();
                var teamProjects = structService.ListAllProjects().Select(a => a);
                foreach (var project in teamProjects)
                {
                    yield return new TeamProjectContract {Name=project.Name};
                }
            }
        }
    }
}