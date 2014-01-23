using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using TfsMobile.Contracts;
using TfsMobile.Repositories.v1.Dtos;

namespace TfsMobile.Repositories.v1
{
    public class TeamProjectsRepository : BaseRepository
    {
        public TeamProjectsRepository(RequestTfsUserDto requestTfsUser) : base(requestTfsUser)
        {
        }

        public async Task<IEnumerable<TeamProjectDto>> GetTeamProjectsAsync(RequestLoginContract requestDetails)
        {
            var historyResult = await GetTeamProjects();
            var teamProjectContracts = JsonConvert.DeserializeObject<List<TeamProjectContract>>(historyResult);

            return teamProjectContracts.Select(Mapper.Map<TeamProjectContract, TeamProjectDto>);
        }


        private async Task<string> GetTeamProjects()
        {

            using (var handler = GetHttpClientHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("tfsuri", RequestTfsUser.TfsUri.ToString());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                       "Basic",
                       Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", RequestTfsUser.Username, RequestTfsUser.Password)))
                       );

                    var targetUri = CreateUri();
                    return await client.GetStringAsync(targetUri);
                }
            }


        }

        private Uri CreateUri()
        {
            var sb = new StringBuilder();
            sb.Append(RequestTfsUser.TfsMobileApiUri);
            sb.Append("/Projects");
            return new Uri(sb.ToString());
        }

    }
}