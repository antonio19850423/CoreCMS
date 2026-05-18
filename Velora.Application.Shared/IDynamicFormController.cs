using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Application.Shared
{
    public interface IDynamicFormController<TDto> where TDto : class
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(Guid id);

        Task<IActionResult> Create([FromBody] TDto dto);

        Task<IActionResult> Update([FromBody] TDto dto);
        Task<IActionResult> Delete(Guid id);
        Task<IActionResult> BulkInsert();
        Task<IActionResult> Export([FromBody] ExportRequestDto request);
    }
}
