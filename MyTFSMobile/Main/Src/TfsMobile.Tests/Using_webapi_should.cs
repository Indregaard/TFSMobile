using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TfsMobile.Contracts;

namespace TfsMobile.Tests
{
    [TestClass]
    public class Using_webapi_should
    {
        [TestMethod]
        public void Get_all_builds()
        {
            string projectName = "Byggtjeneste - Projects";
            int fromDays = 7;

            var client = new TfsClientService("username", "password");
            var res = client.GetBuilds(projectName, fromDays);

            Assert.IsTrue(res.Any());
        }

    }

    public class TfsClientService
    {
        private string Username { get; set; }
        private string Password { get; set; }

        public TfsClientService(string username, string password)
        {
            Password = password;
            Username = username;
        }

        public IEnumerable<BuildContract> GetBuilds(string projectName, int fromDays)
        {
            var methodUri = "http://localhost:3295/Api/Builds/";
            using (var client = new HttpClient())
            {
                var byteArray = Encoding.ASCII.GetBytes(Username + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(byteArray));

                HttpResponseMessage response =
                    client.GetAsync(methodUri + fromDays + "/" + projectName)
                        .Result;

                if (response.IsSuccessStatusCode)
                {
                    var builds = response.Content.ReadAsAsync<IEnumerable<BuildContract>>().Result;
                    return builds;
                }
                return new List<BuildContract>();
            }
        }

        //private static IRestResponse CallNewMethod(string nobbNr)
        //{
        //    var req = new DokumenterForListeAvNobbNrExRequest() { NobbNos = nobbNr, Gtins = "1234" };

        //    var client = new RestClient("http://localhost:20400/media.svc");
        //    var request = new RestRequest("/DokumenterForListeAvNobbNrEx", Method.POST);
        //    request.AddParameter("text/json", HackyBodyCreation(req), ParameterType.RequestBody);
        //    request.RequestFormat = DataFormat.Json;

        //    return client.Execute(request);
        //}

        //private static string HackyBodyCreation(DokumenterForListeAvNobbNrExRequest myRequest)
        //{

        //    var serializer = new DataContractSerializer(typeof(DokumenterForListeAvNobbNrExRequest));
        //    var sb = new StringBuilder();
        //    using (var xw = XmlWriter.Create(sb, new XmlWriterSettings
        //    {
        //        OmitXmlDeclaration = true,
        //        Indent = true
        //    }))
        //    {
        //        serializer.WriteObject(xw, myRequest);
        //        xw.Flush();

        //        return sb.ToString();
        //    }
        //}
    }
}