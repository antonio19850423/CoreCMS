using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IUserAuthenticationService:IBaseService
    {
        Task<ResultDto<RequestOtpResultDto>> RequestOtpAsync(
            RequestOtpDto input,
            CancellationToken cancellationToken = default);

        Task<ResultDto<WebsiteLoginResultDto>> VerifyOtpAsync(
            VerifyOtpDto input,
            CancellationToken cancellationToken = default);

        Task<ResultDto<WebsiteLoginResultDto>> CompleteRegistrationAsync(
            CompleteRegistrationDto input,
            CancellationToken cancellationToken = default);

        Task<ResultDto<UserAddressDto>> CreateUserAddressAsync(
            UserAddressCrud input,
            CancellationToken cancellationToken = default);
        Task<ResultDto<bool>> DeleteUserAddressAsync(
    Guid addressId,
    CancellationToken cancellationToken = default);
        Task<ResultDto<UserAddressDto>> UpdateUserAddressAsync(
    Guid addressId,
    UserAddressCrud input,
    CancellationToken cancellationToken = default);

    }
}
