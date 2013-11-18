using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Web.Http;
using TfsMobile.Contracts;
using TfsMobileServices.Models;

namespace TfsMobileServices.Controllers
{
    public class BuildsController : ApiController
    {
        public IEnumerable<BuildContract> Get(string project, int fromDays, bool myBuilds)
        {
            var headers = HeradersUtil.FixHeaders(Request.Headers);
            var handler = new AuthenticationHandler(headers);
            var tfs = TfsServiceFactory.Get(handler.TfsUri, handler.NetCredentials);

            var rep = new TfsBuildsRepository();
            var res = myBuilds ? rep.GetMyBuilds(tfs, project, fromDays) : rep.GetTeamBuilds(tfs, project, fromDays);
            return res;
        }

        public IEnumerable<BuildContract> Get(string project)
        {
            var headers = HeradersUtil.FixHeaders(Request.Headers);
            var handler = new AuthenticationHandler(headers);
            var tfs = TfsServiceFactory.Get(handler.TfsUri, handler.NetCredentials);
            
            var rep = new TfsBuildsRepository();
            var res = rep.GetBuildDefinitions(tfs, project);
            return res;
        }

        [HttpPost]
        public void QueueBuild(QueueBuildDto queueBuildDto)
        {
            var headers = HeradersUtil.FixHeaders(Request.Headers);
            var handler = new AuthenticationHandler(headers);
            var tfs = TfsServiceFactory.Get(handler.TfsUri, handler.NetCredentials);

            var rep = new TfsBuildsRepository();
            rep.QueueBuild(tfs, queueBuildDto.Project, queueBuildDto.BuildName);
        }
    }

    public class QueueBuildDto
    {
        public string Project { get; set; }
        public string BuildName { get; set; }
    }

    public class HeradersUtil
    {
        public static HttpRequestHeaders FixHeaders(HttpRequestHeaders headers)
        {
            if (!headers.Contains("tfsuri"))
            {
                headers.Add("tfsuri", "http://tfs.osiris.no:8080/tfs");
            }
            //if (headers.Authorization == null)
            //{
            //    headers.Add("uselocaldefault", "true");
            //}
            return headers;
        }
    }
}