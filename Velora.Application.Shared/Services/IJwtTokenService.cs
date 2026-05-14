using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IJwtTokenService : IBaseService
    {
        TokenResultDto GenerateToken(UserDto user);
        TokenResultDto GenerateRefreshToken(UserDto user);
        ClaimsPrincipal? ValidateToken(string token);


    }
}
