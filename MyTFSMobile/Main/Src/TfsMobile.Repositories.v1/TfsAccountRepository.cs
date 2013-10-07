using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TfsMobile.Contracts;

namespace TfsMobile.Repositories.v1
{
    public class TfsAccountRepository : BaseRepository
    {
        public TfsAccountRepository(RequestTfsUserDto requestTfsUser, bool useLocalDefaultTfs) : base(requestTfsUser, useLocalDefaultTfs)
        {
        }

        public async Task<bool> CanConnectToTfs()
        {
            using (var handler = new HttpClientHandler {Credentials = GetNetworkCredentials()})
            {
                using (var client = new HttpClient(handler))
                {
                    AddHttpClientAuthHeaders(client);

                    //var targetUri = new Uri("http://mytfsmobile-api.azurewebsites.net/api/TfsAccount");
                    var targetUri = new Uri("http://192.168.1.23/TfsMobileServices/api/TfsAccount");

                    var taskRes = client.GetAsync(targetUri).ContinueWith(tt =>
                    {
                        if (tt.Result.StatusCode == HttpStatusCode.Accepted)
                        {
                            return true;
                        }
                        return false;
                    });

                    var canConnectToTfs = await taskRes;
                    return canConnectToTfs;
                }

            }
        }
    }

}
