using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TfsMobile.Contracts;

namespace TfsMobile.Repositories.v1
{
    public class BuildsRepository
    {
        private bool UseLocalDefaultTfs { get; set; }
        private RequestTfsUserDto RequestTfsUser { get; set; }
        public BuildsRepository()
        {
            
        }

        public BuildsRepository(RequestTfsUserDto requestTfsUser,bool useLocalDefaultTfs)
        {
            RequestTfsUser = requestTfsUser;
            UseLocalDefaultTfs = useLocalDefaultTfs;
        }
        private async Task<string> GetBuildsAsync(BuildDetailsDto buildDetails)
        {
            
            using (
                var handler = new HttpClientHandler()
                {
                    Credentials = GetNetworkCredentials()
                })
            {
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("tfsuri", RequestTfsUser.TfsUri.ToString());
                    if (UseLocalDefaultTfs)
                    {
                        client.DefaultRequestHeaders.Add("uselocaldefault", "true");
                    }
                    var targetUri = CreateBuildsUri(buildDetails);
                    return await client.GetStringAsync(targetUri);
                }
            }
            
            
        }

        private Uri CreateBuildsUri(BuildDetailsDto buildDetails)
        {
            var sb = new StringBuilder();
            sb.Append("http://localhost:3295/api/Builds?project=");
            //var project = buildDetails.TfsProject.Replace(" ", "%20");
            sb.Append(buildDetails.TfsProject);
            sb.Append("&fromDays=");
            sb.Append(buildDetails.FromDays);
            return new Uri(sb.ToString());
        }

        private NetworkCredential GetNetworkCredentials()
        {
            return new NetworkCredential(RequestTfsUser.Username, RequestTfsUser.Password);
        }

        public IEnumerable<BuildContract> GetBuilds(BuildDetailsDto buildDetails)
        {
            var buildsResult = GetBuildsAsync(buildDetails).Result;
            var buildContracts = JsonConvert.DeserializeObject<List<BuildContract>>(buildsResult);
            return buildContracts;  
        }
    }


}
