using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Services
{

    public interface ILinkResolverService:IBaseService
    {
        Task<string?> GetUrlAsync(
            Guid? linkTypeId,
            Guid? targetId,
            string? currentUrl = null);
    }


}
