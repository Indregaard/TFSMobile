using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using TfsMobileServices.Models;

namespace TfsMobileServices
{
    public class BasicAuthenticationMessageHandler : DelegatingHandler
    {
        private const string WwwAuthenticateHeader = "WWW-Authenticate";
        private const string Basic = "Basic";

        public BasicAuthenticationMessageHandler()
        {
            //_logger = logger;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization != null)
            {
                var authentication = new AuthenticationHandler(request.Headers);


                if (authentication.ValidateUser())
                {
                    IPrincipal principal = authentication.GetGenericPrincipal();
                    Thread.CurrentPrincipal = principal;
                    HttpContext.Current.User = principal;
                }
                //else
                //{
                //    return Task<HttpResponseMessage>.Factory.StartNew(CreateUnauthorizedResponse, cancellationToken);
                //}
            }
            return base.SendAsync(request, cancellationToken);
        }

        private static HttpResponseMessage CreateUnauthorizedResponse()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("Access Denied.")
            };
            response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic"));
            return response;
        }

        

        


       
    }

    public class AuthenticationHandler
    {
        public Uri TfsUri { get; set; }
        private AuthenticationHeaderValue AuthHeader { get; set; }
        public Credentials Credentials { get; private set; }
        public NetworkCredential NetCredentials { get; private set; }
        //public ITfsServer Tfs { get; private set; }
        private AuthenticationHandler(AuthenticationHeaderValue authHeader)
        {
            AuthHeader = authHeader;
            ExtractCredentials();
            
        }

        public AuthenticationHandler(HttpRequestHeaders authHeader):this(authHeader.Authorization)
        {
            SetTfsUri(authHeader);
            SetUseLocalAccount(authHeader);
            //Tfs = new TfsServiceHandler(Credentials, TfsUri);
        }

        private void SetTfsUri(HttpRequestHeaders authHeader)
        {
            var tfsHeader = authHeader.FirstOrDefault(h => h.Key == "tfsuri");
            if (tfsHeader.Value != null)
            {
                TfsUri = new Uri(tfsHeader.Value.First());
            }
        }
        private void SetUseLocalAccount(HttpRequestHeaders authHeader)
        {
            var uselocaldefault = authHeader.FirstOrDefault(h => h.Key == "uselocaldefault");
            if (uselocaldefault.Value != null)
            {
                Credentials.UseLocalDefault = true;
            }
        }

        //public string ErrorMsg { get; private set; }

        private void ExtractCredentials()
        {
            try
            {
                if (AuthHeader == null)
                {
                    Credentials = new Credentials();
                }
                else
                {
                    if (AuthHeader.Scheme != "Basic")
                    {
                        Credentials = null;
                    }

                    var encodedUserPass = AuthHeader.Parameter.Trim();
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    var userPass = encoding.GetString(Convert.FromBase64String(encodedUserPass));
                    var parts = userPass.Split(":".ToCharArray());
                    Credentials = new Credentials(parts[0],parts[1]);
                    NetCredentials = new NetworkCredential(Credentials.Username, Credentials.Password,
                        Credentials.Domain);
                }

                
            }
            catch (Exception)
            {
                //ErrorMsg = "BasicAuthenticationMessageHandler.ExtractCredentials: Cannot extract credentials.";
                Credentials = null;
            }
        }

        public GenericPrincipal GetGenericPrincipal()
        {
            return new GenericPrincipal(new GenericIdentity(Credentials.Username, "Basic"), null);
        }

        public bool ValidateUser()
        {

            using (var tfs = TfsServiceFactory.Get(TfsUri, NetCredentials, Credentials.UseLocalDefault).Connect())
            {
                return tfs.HasAuthenticated;
            }
        }
    }
}