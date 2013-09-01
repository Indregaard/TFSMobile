using System;
using RestSharp;
using TfsMobile.Contracts;

namespace TfsMobile.Console
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
            var client = new RestClient("localhost:3295");

            {
                var methodUri = "http://localhost:3295/Api/Logins/";
                var request = new RestRequest("api/Logins/", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("header", "value");
                request.AddBody(new TfsSettingsContract
                {
                    TfsServerUri = new Uri("http://tfs.osiris.no:8080/tfs"),
                    ProjectName = "Byggtjeneste - Projects"
                });
                var response = client.Execute<LoggedInContract>(request);
                return response.Data;
                //var content = response.Content; // raw content as string
                //var byteArray =
                //    Encoding.ASCII.GetBytes(Properties.Settings.Default.Username + ":" +
                //                            Properties.Settings.Default.Password);
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                //    Convert.ToBase64String(byteArray));

                
            }
        }
    }
}
