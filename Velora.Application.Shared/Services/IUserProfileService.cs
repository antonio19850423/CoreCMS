using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

namespace Velora.Application.Shared.Services
{
    public interface IUserProfileService : IGenericService<SqlUserProfile, PgUserProfile, UserProfileDto>, IBaseService
    {
        Task<UserProfileDto?> GetByUserIdAsync(Guid UserId);
    }

}
