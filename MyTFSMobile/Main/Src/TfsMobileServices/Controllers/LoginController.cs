using System.Web.Http;
using TfsMobile.Contracts;

namespace TfsMobileServices.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public bool Login(RequestLoginContract login)
        {
            var headers = HeradersUtil.FixHeaders(Request.Headers);
            var handler = new AuthenticationHandler(headers);
            var validated = handler.ValidateUser();
            return validated;
        }

        
    }
}