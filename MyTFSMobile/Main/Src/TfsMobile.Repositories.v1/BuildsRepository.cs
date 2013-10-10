using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TfsMobile.Contracts;

namespace TfsMobile.Repositories.v1
{
    public class BuildsRepository : BaseRepository
    {
        


        public BuildsRepository(RequestTfsUserDto requestTfsUser, bool useLocalDefaultTfs) : base(requestTfsUser,useLocalDefaultTfs)
        {
           
        }

        private async Task<string> GetBuildsAsync(BuildDetailsDto buildDetails)
        {

            using (var handler = GetHttpClientHandler())
                {
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("tfsuri", RequestTfsUser.TfsUri.ToString());
                    if (UseLocalDefaultTfs)
                    {
                        client.DefaultRequestHeaders.Add("uselocaldefault", "true");
                    }
                        //client.DefaultRequestHeaders.Authorization =
                        //new AuthenticationHeaderValue(
                        //    "Basic",
                        //    Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", RequestTfsUser.Username, RequestTfsUser.Password)))
                        //    );
                   



                    var targetUri = CreateBuildsUri(buildDetails);

                    var taskRes = client.GetAsync(targetUri).ContinueWith(tt =>
                    {
                        if (tt.Result.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            return null;
                        }
                        tt.Result.EnsureSuccessStatusCode();
                        return tt.Result;
                    });

                    var buildRes = await taskRes;
                    return buildRes != null ? buildRes.Content.ReadAsStringAsync().Result : null;

                }
            }


        }

        

        private Uri CreateBuildsUri(BuildDetailsDto buildDetails)
        {
            var sb = new StringBuilder();
            sb.Append("http://192.168.10.193/TfsMobileServices/api/Builds?project=");
            //var project = buildDetails.TfsProject.Replace(" ", "%20");
            sb.Append(buildDetails.TfsProject);
            sb.Append("&fromDays=");
            sb.Append(buildDetails.FromDays);
            return new Uri(sb.ToString());
        }

       

        public IEnumerable<BuildContract> GetBuilds(BuildDetailsDto buildDetails)
        {
            var buildsResult = GetBuildsAsync(buildDetails).Result;

            var buildContracts = JsonConvert.DeserializeObject<List<BuildContract>>(buildsResult);
            return buildContracts;
        }

        //private static HttpResponseMessage Get(HttpClient client, string url)
        //{
        //    using (var task = client.GetAsync(url))
        //    {
        //        task.Wait();
        //        return task.Result;
        //    }
        //}

        //private static string GetContent(HttpResponseMessage response)
        //{
        //    using (var contentTask = response.Content.ReadAsStringAsync())
        //    {
        //        contentTask.Wait();
        //        return contentTask.Result;
        //    }
        //}
    }

    public class LoginRepository : BaseRepository
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
                    if (UseLocalDefaultTfs)
                    {
                        client.DefaultRequestHeaders.Add("uselocaldefault", "true");
                    }

                    client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", RequestTfsUser.Username, RequestTfsUser.Password)))
                        );

                    client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
       


                    var mediaType = new MediaTypeHeaderValue("application/json");
                    var jsonSerializerSettings = new JsonSerializerSettings();
                    //var jsonFormatter = new JsonNetFormatter(jsonSerializerSettings);
                    //var d = JsonConvert.SerializeObject(s);
                    dynamic s = new ExpandoObject();
                    s.comeValue = 1;
                    var d = JsonConvert.SerializeObject(s);
                    var requestcontent = new StringContent(d, Encoding.UTF8, "application/json");
                    //var requestMessage = new HttpRequestMessage<T>(data, mediaType, new MediaTypeFormatter[] { jsonFormatter });


                    //HttpContent requestcontent = new FormUrlEncodedContent(new[]
                    //{
                    //    new KeyValuePair<string, string>("login", requestMessage.),
                    //});

                    requestcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    
                    
                    //var result = await client.PostAsync(CreateTryLoginUri().ToString(), content).ContinueWith(tt =>
                    //{
                    //    if (tt.Result.StatusCode == HttpStatusCode.Accepted)
                    //    {
                    //        return true;
                    //    }
                    //    return false;
          
                    //});
                    var requestUri = CreateTryLoginUri().ToString();
                   
                    var response =  client.PostAsync(requestUri, requestcontent).ContinueWith(tt =>
                    {
                        if (tt.Result.StatusCode == HttpStatusCode.OK)
                        {
                            var foo = tt.Result.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<bool>(foo.Result);
                        }
                        return false;
                    });
                    var resultat = await response;
                    return resultat;
                    //string content = await response.Content.ReadAsStringAsync();
                    //return content;
                    //return Task.Run(() => content);
                    ////return result.Content.ReadAsStringAsync().ToString();
                    //using (var loginStream = result.Content.ReadAsStreamAsync().)
                    //{

                    //}
                    //using (StreamReader responseReader = new StreamReader(result.Content))
                    //{
                    //    //String sLine = responseReader.ReadLine();
                    //    String sResponse = responseReader.ReadToEnd();
                    //}


                }
            }


        }

        private Uri CreateTryLoginUri()
        {
            var sb = new StringBuilder();
            //sb.Append("http://mytfsmobile-api.azurewebsites.net/api/Login");
            sb.Append("http://192.168.10.193/TfsMobileServices/api/Login");
            //var project = buildDetails.TfsProject.Replace(" ", "%20");
            //sb.Append(buildDetails.TfsProject);
            //sb.Append("&fromDays=");
            //sb.Append(buildDetails.FromDays);
            return new Uri(sb.ToString());
        }

        public bool TryLogin()
        {
            return TryLoginAsync(GetLoggedInContract()).Result;
        }

        private RequestLoginContract GetLoggedInContract()
        {
            return new RequestLoginContract {TfsUri = RequestTfsUser.TfsUri.ToString()};
        }
    }
}
