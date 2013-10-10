using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Web.Http;
using Microsoft.Win32;
using TfsMobile.Contracts;

namespace TfsMobileServices.Controllers
{
    public class LoginController : ApiController
    {
        // POST api/values
        public bool Post(RequestLoginContract login)
        {
            var headers = HeradersUtil.FixHeaders(Request.Headers);
            var contract = new LoggedInContract();
            var handler = new AuthenticationHandler(headers);
            var validated =handler.ValidateUser();
            return validated;
        }

        
    }
}