using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ICaptchaService:IBaseService
    {
        Task<CaptchaGenerateDto> GenerateAsync(
            CancellationToken cancellationToken =
                default);

        Task<ResultDto<CaptchaValidationResultDto>>
            ValidateAsync(
                CaptchaValidateDto input,
                CancellationToken cancellationToken =
                    default);
    }
}
