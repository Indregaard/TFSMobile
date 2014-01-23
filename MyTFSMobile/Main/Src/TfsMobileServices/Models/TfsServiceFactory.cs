using System;
using System.Net;
using System.Text;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TfsMobileServices.Models
{
    public class TfsServiceFactory
    {

        public static TfsService Get(Uri tfsUri, NetworkCredential cred)
        {
            return new TfsService(tfsUri, cred);
        }

    }
}