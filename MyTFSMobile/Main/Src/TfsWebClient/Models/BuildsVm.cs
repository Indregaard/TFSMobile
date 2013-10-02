using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TfsMobile.Contracts;

namespace TfsWebClient.Models
{
    public class BuildsVm
    {
        public IEnumerable<BuildContract> Builds { get; set; } 

        public BuildsVm(IEnumerable<BuildContract> builds)
        {
            Builds = builds;
        }
    }
}