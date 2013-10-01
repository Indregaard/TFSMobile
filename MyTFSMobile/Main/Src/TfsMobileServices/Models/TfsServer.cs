using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using TfsMobile.Contracts;

namespace TfsMobileServices.Models
{
    public class TfsServiceFactory
    {

        public static TfsService2 Get(Uri tfsUri, NetworkCredential cred, bool uselocaldefault)
        {
            if (uselocaldefault)
            {
                return new TfsService2(tfsUri);
            }
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
        }

    }

    public class TfsService2 : IDisposable
    {
        public bool UseLocalAccount { get; private set; }
        private Uri TfsUri { get; set; }
        
        public NetworkCredential NetCredentials { get; private set; }
        
        private TfsTeamProjectCollection Tp { get; set; }


        public TfsService2(){}
       
        public TfsService2(Uri tfsUri)
        {
            UseLocalAccount = true;
            TfsUri = tfsUri;
        }

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
                Tp = !UseLocalAccount ? new TfsTeamProjectCollection(TfsUri, NetCredentials) : new TfsTeamProjectCollection(TfsUri);
                Tp.Authenticate();
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