using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TfsMobile.Contracts;

namespace TfsMobile.Repositories.v1
{
    public class BuildsRepository : BaseRepository
    {
        


        public BuildsRepository(RequestTfsUserDto requestTfsUser, bool useLocalDefaultTfs) : base(requestTfsUser,useLocalDefaultTfs)
        {
           
        }

        public async Task<string> GetBuildsAsync(BuildDetailsDto buildDetails)
        {

            using (var handler = GetHttpClientHandler())
                {
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("tfsuri", RequestTfsUser.TfsUri.ToString());
                    //if (UseLocalDefaultTfs)
                    //{
                    //    client.DefaultRequestHeaders.Add("uselocaldefault", "true");
                    //}

                    client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", RequestTfsUser.Username, RequestTfsUser.Password)))
                        );

                    var targetUri = CreateBuildsUri(buildDetails);

                    var taskRes = await client.GetAsync(targetUri).ContinueWith(tt =>
                    {
                        if (tt.Result.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            return null;
                        }
                        tt.Result.EnsureSuccessStatusCode();
                        return tt.Result;
                    });

                    var buildRes = taskRes;
                    return buildRes != null ? buildRes.Content.ReadAsStringAsync().Result : null;

                }
            }


        }

        private Uri CreateBuildsUri(BuildDetailsDto buildDetails)
        {
            var sb = new StringBuilder();
            sb.Append("http://localhost:3389/api/Builds?project=");
            //var project = buildDetails.TfsProject.Replace(" ", "%20");
            sb.Append(buildDetails.TfsProject);
            sb.Append("&fromDays=");
            sb.Append(buildDetails.FromDays);
            return new Uri(sb.ToString());
        }

       

        public IEnumerable<BuildContract> GetBuilds(BuildDetailsDto buildDetails)
        {
            var buildsResult = GetBuildsAsync(buildDetails).Result;

            var buildContracts = JsonConvert.DeserializeObject<List<BuildContract>>(buildsResult);
            return buildContracts;
        }
    }

    
}
