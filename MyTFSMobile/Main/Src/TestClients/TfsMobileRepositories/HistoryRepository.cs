using System;
using System.Collections.Generic;
using RestSharp;

namespace TfsMobileRepositories
{
    public class HistoryRepository : BaseRepository
    {
        public HistoryRepository(HttpBasicAuthenticator athenticator) : base(athenticator)
        {
        }

        public IEnumerable<HistoryItemContract> Get()
        {
            var client = new RestClient("http://localhost/TfsMobileServices");
            client.Authenticator = Athenticator;
            //var request = new RestRequest("api/history/Get", Method.GET);
            var request = new RestRequest("api/History?project=Main&fromDays=1", Method.GET);
            request.AddHeader("tfsuri", "http://tfs.byggtjeneste.no:8080/tfs");
            var queryResult = client.Execute<List<HistoryItemContract>>(request).Data;
            return queryResult;
        }
    }

    public class TeamProjectRepository : BaseRepository
    {
        public TeamProjectRepository(HttpBasicAuthenticator athenticator) : base(athenticator)
        {
        }

        public IEnumerable<TeamProjectContract> Get()
        {
            var client = new RestClient("http://localhost/TfsMobileServices");
            client.Authenticator = Athenticator;
            //var request = new RestRequest("api/history/Get", Method.GET);
            var request = new RestRequest("api/Projects", Method.GET);
            request.AddHeader("tfsuri", "http://tfs.byggtjeneste.no:8080/tfs");
            var queryResult = client.Execute<List<TeamProjectContract>>(request).Data;
            return queryResult;
        }
    }

    public class BaseRepository
    {
        protected HttpBasicAuthenticator Athenticator { get; set; }
        public BaseRepository(HttpBasicAuthenticator athenticator)
        {
            Athenticator = athenticator;
        }
    }

    public class HistoryItemContract
    {
        
        public int Id { get; set; }

        
        public string WorkType { get; set; }
        
        public string Title { get; set; }

        
        public string Description { get; set; }

        
        public DateTime HistoryDate { get; set; }

        
        public Uri TfsItemUri { get; set; }
        
        public string AreaPath { get; set; }
        
        public string IterationPath { get; set; }
        
        public string State { get; set; }

        
    }

    public class TeamProjectContract
    {
        
        public string Name { get; set; }
    }

}