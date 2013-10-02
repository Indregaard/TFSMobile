using System;
using System.Collections.Generic;
using System.Net.Http;
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
                    var targetUri = CreateHistoryUri(buildDetails);
                    return await client.GetStringAsync(targetUri);
                }
            }


        }

        private Uri CreateHistoryUri(RequestHistoryDto buildDetails)
        {
            var sb = new StringBuilder();
            sb.Append("http://localhost:3295/api/History?project=");
            //var project = buildDetails.TfsProject.Replace(" ", "%20");
            sb.Append(buildDetails.TfsProject);
            sb.Append("&fromDays=");
            sb.Append(buildDetails.FromDays);
            return new Uri(sb.ToString());
        }
    }
}