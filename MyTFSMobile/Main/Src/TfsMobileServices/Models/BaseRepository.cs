using System;
using System.Net;

namespace TfsMobileServices.Models
{
    public class BaseRepository : IDisposable
    {
        public Uri TfsUri { get; private set; }
        public NetworkCredential Cred { get; private set; }
        public TfsService Tf { get; private set; }
        public BaseRepository(Uri tfsUri, NetworkCredential cred)
        {
            Cred = cred;
            TfsUri = tfsUri;
            Tf = TfsServiceFactory.Get(TfsUri,Cred);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (Tf != null)
                {
                    Tf.Dispose();
                    Tf = null;
                }
            }
          
        }
    }
}