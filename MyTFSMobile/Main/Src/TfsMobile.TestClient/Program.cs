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
                var rep = new BuildsRepository(RequestTfsUserDto.Default(), true);
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
            var rep = new LoginRepository(RequestTfsUserDto.Default(), true);
            return rep.TryLogin();
        }
    }



    
}
