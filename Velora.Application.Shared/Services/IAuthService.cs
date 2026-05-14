using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IAuthService:IBaseService
    {
        Task<ResultDto<UserDto>> RegisterAsync(RegisterDto registerDto);
        Task<ResultDto<LoginResultDto>> LoginAsync(LoginDto loginDto);
        Task<ResultDto<UserProfileDto>> CompleteProfileAsync(Guid userId, CompleteProfileDto profileDto);
        Task<ResultDto<LoginResultDto>> RefreshTokenAsync();
        Task<ResultDto<bool>> LogoutAsync();
    }

}
