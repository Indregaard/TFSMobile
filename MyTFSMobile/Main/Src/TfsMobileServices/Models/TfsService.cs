using System;
using System.Net;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Client;

public class TfsService : IDisposable
{
    public bool UseLocalAccount { get; private set; }
    public Uri TfsUri { get; private set; }

    public NetworkCredential NetCredentials { get; private set; }

    private TfsTeamProjectCollection Tp { get; set; }

    public string CollectionDisplayName { get; private set; }

    public TfsService() { }



    public TfsService(Uri tfsUri, NetworkCredential cred)
    {
        NetCredentials = cred;
        UseLocalAccount = false;
        TfsUri = tfsUri;
    }

    public TfsTeamProjectCollection Connect()
    {
        Tp = null;

        if (TfsUri != null)
        {
            try
            {
                Tp = new TfsTeamProjectCollection(TfsUri);
                Tp.Credentials = NetCredentials;
                Tp.EnsureAuthenticated();
                CollectionDisplayName = Tp.CatalogNode.Resource.DisplayName;
            }
            catch (TeamFoundationServerUnauthorizedException)
            {
            }
        }
        return Tp;
    }




    #region IDisposable Members

    public void Dispose()
    {
        Tp = null;
    }

    #endregion // IDisposable Members


}