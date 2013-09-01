using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using TfsMobile.Contracts;

namespace TfsMobile.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var foo = new LoginsRepository();
            var res = foo.Login("torarnev", "Su1c1dal9");
            Console.WriteLine(res.Id);
            Console.WriteLine(res.LoggedInUser);
            Console.ReadKey();
        }
    }

    public class LoginsRepository
    {
        public LoggedInContract Login(string username, string password)
        {
            var client = new RestClient("http://localhost:3295/Api/Logins");

            var request = new RestRequest("/Logins", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-type", "application/json");
            client.Authenticator = new HttpBasicAuthenticator("username","password");
            

            request.AddBody(request.JsonSerializer.Serialize(new TfsSettingsContract
            {
                TfsServerUri = "tfs.osiris.no:8080/tfs",
                ProjectName = "Byggtjeneste - Projects"
            }));


            var response = client.Execute<LoggedInContract>(request);
            return response.Data;

        }
    }
}
