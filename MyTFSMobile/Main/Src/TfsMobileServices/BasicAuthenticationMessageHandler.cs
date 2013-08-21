using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Common.Internal;

namespace TfsMobileServices
{
    public class BasicAuthenticationMessageHandler : DelegatingHandler
    {
        //private readonly ILogger _logger;

       

        public BasicAuthenticationMessageHandler()
        {
            //_logger = logger;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization != null)
            {
                var credentials = ExtractCredentials(request.Headers.Authorization);
                if (credentials != null && ValidateUser(credentials))
                {

                    IPrincipal principal = new GenericPrincipal(new GenericIdentity(credentials.Username, "Basic"), Roles.GetRolesForUser(credentials.Username));
                    Thread.CurrentPrincipal = principal;
                    HttpContext.Current.User = principal;

                    //request.Properties.Add(HttpPropertyKeys.UserPrincipalKey, new GenericPrincipal(identity, new string[0]));
                }
            }
            return base.SendAsync(request, cancellationToken);
        }

        private bool ValidateUser(Credentials credentials)
        {

            TfsServer.Instance().SetCredentials(credentials);
            TfsServer.Instance().Tfs.EnsureAuthenticated();
            return TfsServer.Instance().Tfs.HasAuthenticated;
            //if (!Membership.Provider.ValidateUser(credentials.Username, credentials.Password))
            //{
            //    Debug.WriteLine("BasicAuthenticationMessageHandler.ExtractCredentials: Authentication failed for user '{0}'", credentials.Username);
            //    return false;
            //}
            //return true;
        }

        private Credentials ExtractCredentials(AuthenticationHeaderValue authHeader)
        {
            try
            {
                if (authHeader == null)
                {
                    Debug.WriteLine("BasicAuthenticationMessageHandler.ExtractCredentials: auth header is null, returning null");
                    return null;
                }

                if (authHeader.Scheme != "Basic")
                {
                    Debug.WriteLine("BasicAuthenticationMessageHandler.ExtractCredentials: unsupported scheme {{0}), returning null", authHeader.Scheme);
                    return null;
                }

                var encodedUserPass = authHeader.Parameter.Trim();
                var encoding = Encoding.GetEncoding("iso-8859-1");
                var userPass = encoding.GetString(Convert.FromBase64String(encodedUserPass));
                var parts = userPass.Split(":".ToCharArray());
                return new Credentials { Username = parts[0], Password = parts[1] };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "BasicAuthenticationMessageHandler.ExtractCredentials: Cannot extract credentials.");
                return null;
            }
        }


       
    }


    public class TfsServer
    {

        private static TfsServer _instance;
        public TfsTeamProjectCollection Tfs { get; private set; }
        public Credentials Credentials { get; private set; }

        // Constructor is 'protected'

        //protected TfsServer()
        //{

        //}

        public void SetCredentials(Credentials credentials)
        {
            Credentials = credentials;
            Tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri("http://tfs.osiris.no:8080/tfs"));
            Tfs.ClientCredentials = new TfsClientCredentials(new BasicAuthCredential(CredentialsAuth.GetCredentials(credentials)));
        }

        public static TfsServer Instance()
        {

            // Uses lazy initialization.

            // Note: this is not thread safe.

            if (_instance == null)
            {

                _instance = new TfsServer();

            }



            return _instance;

        }

    }

    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class CredentialsAuth : ICredentials
    {
        private Credentials Cred { get; set; }
        private CredentialsAuth(Credentials cred)
        {
            Cred = cred;
        }

        public static ICredentials GetCredentials(Credentials cred)
        {
            return new CredentialsAuth(cred);
        }

        public NetworkCredential GetCredential(Uri uri, string authType)
        {
            return new NetworkCredential(Cred.Username, Cred.Password);
        }
    }
}