using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Web.Http;
using TfsMobile.Contracts;
using TfsMobileServices.Models;

namespace TfsMobileServices.Controllers
{
    public class BuildsController : ApiController
    {

        [HttpGet]
        public IEnumerable<BuildContract> GetMyBuilds(string project, int fromDays)
        {
            var headers = HeradersUtil.FixHeaders(Request.Headers);
            var handler = new AuthenticationHandler(headers);
            var tfs = TfsServiceFactory.Get(handler.TfsUri, handler.NetCredentials, handler.Credentials.UseLocalDefault);

            var rep = new TfsBuildsRepository();
            var res = rep.GetMyBuilds(tfs, project, fromDays);
            return res;
        }

        [HttpGet]
        public IEnumerable<BuildContract> GetAllTeamBuilds(string project, int fromDays)
        {
            var headers = HeradersUtil.FixHeaders(Request.Headers);
            var handler = new AuthenticationHandler(headers);
            var tfs = TfsServiceFactory.Get(handler.TfsUri, handler.NetCredentials, handler.Credentials.UseLocalDefault);

            var rep = new TfsBuildsRepository();
            var res = rep.GetAllTeamBuilds(tfs, project, fromDays);
            return res;
        }

        [HttpPost]
        public void QueueBuild(QueueBuildDto queueBuildDto)
        {
            var headers = HeradersUtil.FixHeaders(Request.Headers);
            var handler = new AuthenticationHandler(headers);
            var tfs = TfsServiceFactory.Get(handler.TfsUri, handler.NetCredentials, handler.Credentials.UseLocalDefault);

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
            if (headers.Authorization == null)
            {
                headers.Add("uselocaldefault", "true");
            }
            return headers;
        }
    }
}