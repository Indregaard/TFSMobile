using System.Collections.Generic;
using System.Web.Http;
using TfsMobile.Contracts;
using TfsMobileServices.Models;

namespace TfsMobileServices.Controllers
{
    public class HistoryController : ApiController
    {

        public IEnumerable<HistoryItemContract> Get(string project, int fromDays)
        {
            var handler = new AuthenticationHandler(Request.Headers);
            var tfs = TfsServiceFactory.Get(handler.TfsUri, handler.NetCredentials, handler.Credentials.UseLocalDefault);

            var rep = new TfsHistoryRepository();
            var res = rep.GetHistory(tfs, "Byggtjeneste - Projects", fromDays);
            return res;
        }

        public IEnumerable<HistoryItemContract> Get()
        {
            Request.Headers.Add("uselocaldefault", "true");
            Request.Headers.Add("tfsuri", "http://tfs.osiris.no:8080/tfs");
            var handler = new AuthenticationHandler(Request.Headers);
            var tfs = TfsServiceFactory.Get(handler.TfsUri, handler.NetCredentials, handler.Credentials.UseLocalDefault);

            var rep = new TfsHistoryRepository();
            var res = rep.GetHistory(tfs, "Byggtjeneste - Projects", 7);
            return res;
        }
    }
}