using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsMobile.Contracts;

namespace TfsMobile.Repositories.v1.Interfaces
{
    public interface ILoginRepository
    {
        Task<bool> TryLoginAsync(RequestLoginContract requestLoginDetails);
        bool TryLogin();
    }
}
