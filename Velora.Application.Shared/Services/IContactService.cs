using HotChocolate.Execution.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IContactService: IBaseService
    {
        Task<ResultDto<ContactUsDto>> SendContactAsync(ContactUsDto input);
    }
}
