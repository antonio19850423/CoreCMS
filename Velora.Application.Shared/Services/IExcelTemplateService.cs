using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Services
{
    public interface IExcelTemplateService:IBaseService
    {
        Task<byte[]> GenerateTemplateWithLookupsAsync(string entityName, int emptyRows = 100);
    }

}
