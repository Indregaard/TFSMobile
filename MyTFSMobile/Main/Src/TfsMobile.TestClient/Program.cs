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
                Console.WriteLine("Login success!!!");

                Console.WriteLine("Builds:");
                var rep = new BuildsRepository(LoginDetails(), false);
                var res = rep.GetBuilds(BuildDetailsDto.Default());
                foreach (var buildContract in res)
                {
                    Console.WriteLine("Name: " + buildContract.Name + " - Status: " + buildContract.Status +
                                      " - Finished: " + buildContract.FinishTime);
                }

                Console.WriteLine("History last 7 days;");
                var historyRep = new HistoryRepository(LoginDetails(),false);
                var hist =
                    historyRep.GetHistory(new RequestHistoryDto() {FromDays = "7", TfsProject = "Byggtjeneste - Projects"});
                foreach (var h  in hist)
                {
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine("Id: " + h.Id + " - AreaPath: " + h.AreaPath);
                    Console.WriteLine("- Description: " + h.Description);
                    Console.WriteLine("- HistoryDate:" + h.HistoryDate + "- HistoryItemType:" + h.HistoryItemType +
                                      "- State:" + h.State);
                    Console.WriteLine("- IterationPath" + h.IterationPath);
                    Console.WriteLine("- ItemUrl" + h.TfsItemUri);
                    Console.WriteLine("- WorkType" + h.WorkType);
                    Console.WriteLine("-------------------------------------------------------");
                }
            }
            Console.ReadKey();
        }
            
        private static bool CheckLogin()
        {
            //return true;
            var rep = new LoginRepository(LoginDetails(), false);
            return rep.TryLogin();
        }

        private static RequestTfsUserDto LoginDetails()
        {
            var userDetails = RequestTfsUserDto.Default();
            userDetails.Username = "torarnev/osiris-lan";
            userDetails.Password = "pwd";
            return userDetails;
        }
    }



    
}
