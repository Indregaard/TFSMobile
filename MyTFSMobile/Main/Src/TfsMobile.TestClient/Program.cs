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
            var df = RequestTfsUserDto.Default();
            //var rep = new BuildsRepository(df,true);
            df.Username = "tomindre/crayon";
            df.Password = "Fo4maxi!s";
            //var rep = new BuildsRepository(df, false);
            //var res =  rep.GetBuilds(BuildDetailsDto.Default());
            //foreach (var buildContract in res)
            //{
            //    Console.WriteLine("Name: " + buildContract.Name + " - Status: " + buildContract.Status + " - Finished: " + buildContract.FinishTime);
            //}
            var rep = new TfsAccountRepository(df, false);
            var res = rep.CanConnectToTfs();
            
            Console.ReadKey();
        }
    }



    
}
