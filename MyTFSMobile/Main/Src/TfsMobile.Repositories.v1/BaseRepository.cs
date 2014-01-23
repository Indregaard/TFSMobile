using System.Net;
using System.Net.Http;
using TfsMobile.Contracts;
using TfsMobile.Repositories.v1.Dtos;

namespace TfsMobile.Repositories.v1
{
    public class BaseRepository
    {
        
        protected RequestTfsUserDto RequestTfsUser { get; set; }

        public BaseRepository(RequestTfsUserDto requestTfsUser)
        {
            RequestTfsUser = requestTfsUser;
            
        }
        protected NetworkCredential GetNetworkCredentials()
        {
            return new NetworkCredential(RequestTfsUser.Username, RequestTfsUser.Password);
        }

        protected HttpClientHandler GetHttpClientHandler()
        {
            return new HttpClientHandler()
            {
                Credentials = GetNetworkCredentials()
            };
        }

        public static void Configure()
        {
            AutoMapperBootstrapper.Configure();
        }
    }
}