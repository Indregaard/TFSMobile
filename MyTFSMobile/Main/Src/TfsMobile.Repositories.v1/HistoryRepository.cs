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
    public class HistoryRepository : BaseRepository
    {
        public HistoryRepository(RequestTfsUserDto requestTfsUser) : base(requestTfsUser)
        {
        }

        public async Task<IEnumerable<HistoryItemDto>> GetHistoryAsync(RequestHistoryDto historyRequest)
        {
            var historyResult = await GetHistory(historyRequest);
            var historyContracts = JsonConvert.DeserializeObject<List<HistoryItemContract>>(historyResult);
            
            return historyContracts.Select(Mapper.Map<HistoryItemContract, HistoryItemDto>);
        }


        private async Task<string> GetHistory(RequestHistoryDto buildDetails)
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
            sb.Append(RequestTfsUser.TfsMobileApiUri);
            sb.Append("/History?project=");
            sb.Append(buildDetails.TfsProject);
            sb.Append("&fromDays=");
            sb.Append(buildDetails.FromDays);
            return new Uri(sb.ToString());
        }


      
    }
}