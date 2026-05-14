using HotChocolate;
using HotChocolate.Types;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.Application.Shared.Extensions;
using HotChocolate.Authorization;

namespace Velora.Application.Shared.Services
{
    public interface IRoleGqlResolver
        {
        Task<IQueryable<RoleCrud>> roleView();

        }
}
