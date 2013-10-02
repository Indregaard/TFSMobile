using System.Net;
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
    }
}