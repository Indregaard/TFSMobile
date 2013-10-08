using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using TfsMobile.Contracts;
using TfsMobile.Repositories.v1;

namespace TfsMobile.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            if (CheckLogin())
            {
                var rep = new BuildsRepository(LoginDetails(), false);
                var res = rep.GetBuilds(BuildDetailsDto.Default());
                foreach (var buildContract in res)
                {
                    Console.WriteLine("Name: " + buildContract.Name + " - Status: " + buildContract.Status +
                                      " - Finished: " + buildContract.FinishTime);
                }
            }
            Console.ReadKey();
        }
            
        private static bool CheckLogin()
        {

            var rep = new LoginRepository(LoginDetails(), false);
            return rep.TryLogin();
        }

        private static RequestTfsUserDto LoginDetails()
        {
            var userDetails = RequestTfsUserDto.Default();
            userDetails.Username = "tomindre/crayon";
            userDetails.Password = "";
            return userDetails;
        }
    }



    
}
