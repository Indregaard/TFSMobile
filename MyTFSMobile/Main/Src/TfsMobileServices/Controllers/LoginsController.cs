using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using TfsMobile.Contracts;

namespace TfsMobileServices.Controllers
{
    public class LoginsController : ApiController
    {

        // GET api/values/5
        public string Get(string id)
        {
            return "value";
        }

        // POST api/values
        [Authorize(Roles = "StoreSortment")]
        public LoggedInContract LoggedInContract([FromBody]TfsSettingsContract value)
        {
            var foo = value;

            var userId = Guid.NewGuid();
            var result = new LoggedInContract()
            {
                LoggedInUser = new Uri("http://localhost:3295/Api/Logins/" + userId),
                Id = userId
            };
            return result;
        }

        // POST api/values
        //public LoggedInContract Post(TfsSettingsContract value)
        //{
        //    var foo = value;
        //    var userId = Guid.NewGuid();
        //    var result = new LoggedInContract()
        //    {
        //        LoggedInUser = new Uri("http://localhost:3295/Api/Logins/" + userId),
        //        Id = userId
        //    };
        //    return result;
            

        //}
        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }


    
}