using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using TfsMobile.Contracts;

namespace TfsMobileServices.Controllers
{
    public class LoginController : ApiController
    {
        [System.Web.Http.HttpPost]
        public IHttpActionResult Login(RequestLoginContract login)
        {
            var headers = HeradersUtil.FixHeaders(Request.Headers);
            var handler = new AuthenticationHandler(headers);
            var validated = handler.ValidateUser();
            return Json(validated);
        }

        
    }
}