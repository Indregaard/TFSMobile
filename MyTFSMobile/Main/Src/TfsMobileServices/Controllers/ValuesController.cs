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
        // GET api/values
        [HttpGet]
        public IEnumerable<BuildContract> Builds(int fromDays,string projectName)
        {
            try
            {



                IBuildServer buildServer = (IBuildServer) TfsServer.Instance().Tfs.GetService(typeof (IBuildServer));
                
                IBuildDetailSpec buildSpec;
                //buildSpec = buildServer.CreateBuildDetailSpec("Byggtjeneste - Projects");
                buildSpec = buildServer.CreateBuildDetailSpec(projectName);
                buildSpec.RequestedFor = TfsServer.Instance().Credentials.Username;
                //buildSpec.MinFinishTime = DateTime.Now.AddHours(-7);
                buildSpec.MinFinishTime = DateTime.Now.AddDays(fromDays);
                buildSpec.InformationTypes = null; // for speed improvement
                
                buildSpec.MaxBuildsPerDefinition = 1; //get only one build per build definintion
                buildSpec.QueryOrder = BuildQueryOrder.FinishTimeDescending; //get the latest build only
                buildSpec.QueryOptions = QueryOptions.All;
                var builds = buildServer.QueryBuilds(buildSpec);

                var listOfBuilds =
                    builds.Builds.Select(
                        build =>
                            new BuildContract
                            {
                                FinishTime = build.FinishTime,
                                Name = build.BuildDefinition.Name,
                                Status = build.Status.ToString()
                            }).ToList();
                return listOfBuilds;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        // GET api/values/5
        public string Get(int id)
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
