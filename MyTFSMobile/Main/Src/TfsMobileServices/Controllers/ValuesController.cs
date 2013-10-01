using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.TeamFoundation.Build.Client;
using TfsMobile.Contracts;

namespace TfsMobileServices.Controllers
{
    public class ValuesController : ApiController
    {
        

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }
        public string Get(string project,int fromDays)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

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
