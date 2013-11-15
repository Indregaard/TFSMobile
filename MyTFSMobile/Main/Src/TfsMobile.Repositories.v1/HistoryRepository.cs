using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TfsMobile.Contracts;

namespace TfsMobile.Repositories.v1
{
    public class HistoryRepository : BaseRepository
    {
        public HistoryRepository(RequestTfsUserDto requestTfsUser, bool useLocalDefaultTfs) : base(requestTfsUser, useLocalDefaultTfs)
        {
        }

        public IEnumerable<HistoryItemContract> GetHistory(RequestHistoryDto historyRequest)
        {
            var historyResult = GetHistoryAsync(historyRequest).Result;
            var historyContracts = JsonConvert.DeserializeObject<List<HistoryItemContract>>(historyResult);
            return historyContracts;
        }

        private async Task<string> GetHistoryAsync(RequestHistoryDto buildDetails)
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

                    var targetUri = CreateHistoryUri(buildDetails);
                    return await client.GetStringAsync(targetUri);
                }
            }


        }

        private Uri CreateHistoryUri(RequestHistoryDto buildDetails)
        {
            var sb = new StringBuilder();
            sb.Append("http://localhost:3389/api/History?project=");
            //var project = buildDetails.TfsProject.Replace(" ", "%20");
            sb.Append(buildDetails.TfsProject);
            sb.Append("&fromDays=");
            sb.Append(buildDetails.FromDays);
            return new Uri(sb.ToString());
        }
    }
}