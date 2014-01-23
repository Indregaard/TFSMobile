using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using TfsMobileRepositories;

namespace TfsMobileServiceCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new[] {"Testing team projects"};
            var athenticator = new HttpBasicAuthenticator(@"username", "password");
            if (args.Any())
            {
                var rep = new TeamProjectRepository(athenticator);
                var res = rep.Get().ToList();
                foreach (var teamProjectContract in res)
                {
                    Console.WriteLine(teamProjectContract.Name);
                    Console.WriteLine("\t\n");
                }
            }
            else
            {
                var rep = new HistoryRepository(athenticator);
                var res = rep.Get().ToList();

                if (res.Any())
                {
                    foreach (var item in res)
                    {
                        Console.WriteLine("WI.....");
                        Console.WriteLine("ID: " + item.Id);
                        Console.WriteLine("Title: " + item.Title);
                        Console.WriteLine("WorkType: " + item.WorkType);
                        Console.WriteLine("State: " + item.State);
                        Console.WriteLine("AreaPath: " + item.AreaPath);
                        Console.WriteLine("IterationPath: " + item.IterationPath);
                        Console.WriteLine("TfsItemUri: " + item.TfsItemUri);
                        Console.WriteLine("Description: " + item.Description);
                        Console.WriteLine("HistoryDate: " + item.HistoryDate);
                        Console.WriteLine(".....WI");
                        Console.WriteLine("\t\n");
                    }
                }
                else
                {
                    Console.Write("No results");
                }
            }
            Console.ReadKey();
        }

    }
}
