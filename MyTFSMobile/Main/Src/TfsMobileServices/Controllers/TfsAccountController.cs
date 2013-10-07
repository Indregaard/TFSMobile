using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TfsMobileServices.Controllers
{
    public class TfsAccountController : ApiController
    {
        [Authorize]
        public bool Get()
        {
            return true;
        }
    }
}