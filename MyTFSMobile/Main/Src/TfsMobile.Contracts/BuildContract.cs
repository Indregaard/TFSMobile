using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;

namespace TfsMobile.Contracts
{
    [DataContract]
    public class BuildContract
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public DateTime FinishTime { get; set; }

       
    }

    [DataContract]
    public class HistoryItemContract
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string WorkType { get; set; }

        [DataMember]
        public string HistoryItemType { get; set; }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime HistoryDate { get; set; }

        [DataMember]
        public Uri TfsItemUri { get; set; }
        [DataMember]
        public string AreaPath { get; set; }
        [DataMember]
        public string IterationPath { get; set; }
        [DataMember]
        public string State { get; set; }
    }


    [DataContract]
    public class TfsSettingsContract
    {
        //[DataMember]
        //public string Username { get; set; }
        //[DataMember()]
        //public string Password { get; set; }
        [DataMember]
        public string TfsServerUri { get; set; }
        [DataMember]
        public string ProjectName { get; set; }
    }

    [DataContract]
    public class LoggedInContract
    {
        [DataMember]
        public bool IsValidUser { get; set; }
        //[DataMember]
        //public Uri LoggedInUser { get; set; }
    }


    public class RequestTfsUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Uri TfsUri { get; set; }

        public static RequestTfsUserDto Default()
        {
            return new RequestTfsUserDto() { Username = "username", Password = "password", TfsUri = new Uri("http://tfs.osiris.no:8080/tfs") };
        }
    }

    public class BuildDetailsDto
    {
        public string TfsProject { get; set; }
        public string FromDays { get; set; }

        public static BuildDetailsDto Default()
        {
            return new BuildDetailsDto() {FromDays = "7", TfsProject = "Byggtjeneste - Projects"};
        }
    }

    public class QueueBuildDto
    {
        public string TfsProject { get; set; }
        public string BuildName { get; set; }
    }

    public class RequestHistoryDto
    {
        public string TfsProject { get; set; }
        public string FromDays { get; set; }
    }

    public class RequestLoginContract
    {
        public string TfsUri { get; set; }
    }
}
