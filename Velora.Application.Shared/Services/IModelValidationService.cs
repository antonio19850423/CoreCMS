using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IModelValidationService : IBaseService
    {
        Task<ResultDto<List<string>>> ValidateAsync<T>(T model) where T : class;
    }
}
