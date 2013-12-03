using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TfsMobile.Contracts;
using TfsMobile.Repositories.v1.Interfaces;
using TfsMobile.Repositories.v1.TfsMobileServices;

namespace TfsMobile.Repositories.v1
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        public LoginRepository(RequestTfsUserDto requestTfsUser, bool useLocalDefaultTfs)
            : base(requestTfsUser, useLocalDefaultTfs)
        {

        }

        public async Task<bool> TryLoginAsync(RequestLoginContract requestLoginDetails)
        {

            using (var handler = GetHttpClientHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("tfsuri", RequestTfsUser.TfsUri.ToString());

                    client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", RequestTfsUser.Username, RequestTfsUser.Password)))
                        );

                    client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.MaxResponseContentBufferSize = 256000;
                    client.Timeout = TimeSpan.FromSeconds(10);

                    var requestContract = GetLoggedInContract();
                    var jsonvalue = JsonConvert.SerializeObject(requestContract);
                    var requestcontent = new StringContent(jsonvalue, Encoding.UTF8, "application/json");

                    var requestUri = CreateTryLoginUri().ToString();

                    var resultat = await client.PostAsync(requestUri, requestcontent).ContinueWith(tt =>
                    {
                        if (tt.Result.StatusCode == HttpStatusCode.OK)
                        {
                            var foo = tt.Result.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<bool>(foo.Result);
                        }
                        return false;
                    });
                    return resultat;
                }
            }
        }

        private Uri CreateTryLoginUri()
        {
            var sb = new StringBuilder();
            sb.Append(RequestTfsUser.TfsMobileApiUri);
            sb.Append("/Login");
            return new Uri(sb.ToString());
        }

        public bool TryLogin()
        {
            return TryLoginAsync(GetLoggedInContract()).Result;
        }

        private RequestLoginContract GetLoggedInContract()
        {
            return new RequestLoginContract { TfsUri = RequestTfsUser.TfsUri.ToString() };
        }

        public string GetDataFromTfsMobileWfcService(int inValue)
        {
            return "";
        }

        private void ServiceOnGetDataCompleted(object sender, GetDataCompletedEventArgs getDataCompletedEventArgs)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetData(int inValue)
        {
           // var service = new TfsMobileClient(TfsMobileClient.EndpointConfiguration.BasicHttpBinding_ITfsMobile, "http://tfsmobileservices.azurewebsites.net/TfsMobile.svc");

           // var task = new Task<string>((state) =>
           // {
           //     service.GetDataAsync(inValue);

           //     return new string() { PayloadOriginal = req.Payload, PayloadResponse = "Response was " + Guid.NewGuid().ToString() };
           // }
           //, asyncState);

           // //When the task completes notify the callback
           // task.ContinueWith((t) => { ServiceOnGetDataCompleted(t); });
           // task.Start();

           // return task;


            //var task = Task.Run(() =>
            //{
            //    var t = new TaskCompletionSource<string>();

                

              

            //    service.GetDataAsync(inValue);

            //    return t.Task;
            //}).ContinueWith(service.GetDataCompleted);
            return null;
        }

        public Task<string> GetDataTest(int val)
        {
            var tcs = new TaskCompletionSource<string>();

            var service = new TfsMobileClient(TfsMobileClient.EndpointConfiguration.BasicHttpBinding_ITfsMobile, "http://tfsmobileservices.azurewebsites.net/TfsMobile.svc");


            service.GetDataCompleted += (s, e) => { tcs.SetResult( e.Result);};

            service.GetDataAsync(val);
            

            return tcs.Task;
        }

        public void GetDataCompleted()
        {
        }
    }
}
