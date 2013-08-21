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


}
