using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TfsMobile.Contracts;

namespace TfsMobile.Repositories.v1
{
    public class BaseRepository
    {
        protected bool UseLocalDefaultTfs { get; set; }
        protected RequestTfsUserDto RequestTfsUser { get; set; }

        public BaseRepository(RequestTfsUserDto requestTfsUser, bool useLocalDefaultTfs)
        {
            RequestTfsUser = requestTfsUser;
            UseLocalDefaultTfs = useLocalDefaultTfs;
        }
        protected NetworkCredential GetNetworkCredentials()
        {
            return new NetworkCredential(RequestTfsUser.Username, RequestTfsUser.Password);
        }

        protected HttpClient AddHttpClientAuthHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("tfsuri", RequestTfsUser.TfsUri.ToString());
            if (UseLocalDefaultTfs)
            {
                client.DefaultRequestHeaders.Add("uselocaldefault", "true");
            }
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(string.Format("{0}:{1}", RequestTfsUser.Username,
                            RequestTfsUser.Password)))
                    );
            return client;
        }
    }
}