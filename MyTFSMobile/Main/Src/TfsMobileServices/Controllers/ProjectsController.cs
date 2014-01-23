using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TfsMobile.Contracts;
using TfsMobileServices.Models;

namespace TfsMobileServices.Controllers
{
    public class ProjectsController : ApiController
    {
        public IEnumerable<TeamProjectContract> Get()
        {
            var headers = HeradersUtil.FixHeaders(Request.Headers);
            var handler = new AuthenticationHandler(headers);

            var rep = new TfsTeamProjectRepository(handler.TfsUri, handler.NetCredentials);
            var res = rep.GetAll();
            return res;
        }
    }
}