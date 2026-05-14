using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Services
{
    public interface ICurrentUserService:IBaseService
    {
        Guid GetUserId();
        List<string> GetRoles();
        string GetUserName();
    }

}
