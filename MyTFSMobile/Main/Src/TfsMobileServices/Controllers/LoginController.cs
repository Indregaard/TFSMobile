using System.Web.Http;
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