using System;
using System.Collections.Generic;
using System.Linq;
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
        public Guid Id { get; set; }
        [DataMember]
        public Uri LoggedInUser { get; set; }
    }
}
