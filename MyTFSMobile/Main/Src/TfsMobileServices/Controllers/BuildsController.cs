using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Web.Http;
using TfsMobile.Contracts;
using TfsMobileServices.Models;

namespace TfsMobileServices.Controllers
{
    public class BuildsController : ApiController
    {
        public IEnumerable<BuildContract> Get(string project, int fromDays)
        {
            var headers = HeradersUtil.FixHeaders(Request.Headers);
            var handler = new AuthenticationHandler(headers);
            var tfs = TfsServiceFactory.Get(handler.TfsUri, handler.NetCredentials, handler.Credentials.UseLocalDefault);

            var rep = new TfsBuildsRepository();
            var res = rep.GetBuilds(tfs, project, fromDays);
            return res;
        }

        

        public IEnumerable<BuildContract> Get()
        {
            
            Request.Headers.Add("uselocaldefault", "true");
            Request.Headers.Add("tfsuri", "http://tfs.osiris.no:8080/tfs");
            var handler = new AuthenticationHandler(Request.Headers);
            var tfs = TfsServiceFactory.Get(handler.TfsUri, handler.NetCredentials,handler.Credentials.UseLocalDefault);

            var rep = new TfsBuildsRepository();
            var res = rep.GetBuilds(tfs, "Byggtjeneste - Projects", 7);
            return res;

        }

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
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