using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Services;
using Velora.Application.Shared;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Host.Controllers
{
    /// <summary>
    /// /// <summary>
    /// کنترلرهایی که در فرم‌های داینامیک استفاده می‌شوند،
    /// حتماً باید دو متد BulkInsert و Export را پیاده‌سازی کنند.
    ///
    /// BulkInsert:
    /// برای ثبت گروهی اطلاعات از طریق فایل Excel یا فایل ورودی استفاده می‌شود.
    ///
    /// Export:
    /// برای تولید و دانلود فایل Excel اطلاعات Grid استفاده می‌شود.
    ///
    /// وجود این متدها برای موارد زیر الزامی است:
    /// - ثبت صحیح Resource و Permission ها
    /// - شناسایی صحیح سرویس‌ها در فرانت‌اند
    /// - جلوگیری از خطای Service not found
    /// - فعال شدن قابلیت Import و Export اکسل
    ///
    /// بعد از اضافه کردن این متدها:
    /// - جدول SeedHistory پاک شود
    /// - پروژه مجدداً اجرا شود
    /// - EntityName داخل ModelMapping ثبت شود
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeResource(AppRoles.Developer, AppRoles.Admin)]
    public class ComponentTypeController : ControllerBase, IDynamicFormController<ComponentTypeCrud>
    {
        private readonly IComponentTypeService _ComponentTypeService;
        private readonly ITransactionService _transactionService;

        public ComponentTypeController(IComponentTypeService ComponentTypeService, ITransactionService transactionService)
        {
            _ComponentTypeService = ComponentTypeService;
            _transactionService = transactionService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ComponentTypeService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _ComponentTypeService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] ComponentTypeCrud ComponentType)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultDto<ComponentTypeDto>
                {
                    Success = false,
                    Message = "Invalid model",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });

            var result = await _ComponentTypeService.CreateAsync(ComponentType);
            if (!result.Success)
                return BadRequest(result);

            await _transactionService.CommitAsync();
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] ComponentTypeCrud dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultDto<ComponentTypeDto>
                {
                    Success = false,
                    Message = "Invalid model",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });

            var result = await _ComponentTypeService.UpdateAsync(dto);
            if (!result.Success)
                return NotFound(result);

            await _transactionService.CommitAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _ComponentTypeService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result);

            await _transactionService.CommitAsync();
            return Ok(result);
        }

        [HttpPost("BulkInsert")]
        [Consumes("multipart/form-data")]
        [RequestFormLimits(MultipartBodyLengthLimit = 50_000_000)] // 50 MB
        public async Task<IActionResult> BulkInsert()
        {
            // اطمینان از اینکه فرم ارسال شده
            if (!Request.HasFormContentType)
                return BadRequest(new { Message = "Invalid content type, expected multipart/form-data." });

            var form = await Request.ReadFormAsync();
            var file = form.Files.FirstOrDefault();

            if (file == null || file.Length == 0)
                return BadRequest(new { Message = "File is required." });

            using var stream = file.OpenReadStream();
            var result = await _ComponentTypeService.BulkInsertAsync(stream);

            var bulkResult = new BulkInsertResult
            {
                InsertedCount = result.Data?.InsertedCount ?? 0,
                ErrorCount = result.Data?.ErrorCount ?? 0,
                ErrorFileUrl = result.Data.ErrorFileUrl

            };

            await _transactionService.CommitAsync();

            return Ok(new ResultDto<BulkInsertResult>
            {
                Success = result.Success,
                Message = result.Message,
                Data = bulkResult,
                Errors = result.Errors
            });
        }

        // 2️⃣ اکشن POST
        [HttpPost("Export")]
        [AllowAnonymous]
        public async Task<IActionResult> Export([FromBody] ExportRequestDto request)
        {
            if (request == null)
                return BadRequest(new { Success = false, Message = "Invalid request" });

            byte[] fileBytes;
            try
            {
                fileBytes = await _ComponentTypeService.ExportAsync(
                    request.ExportCurrentPage,
                    request.PageNumber,
                    request.PageSize
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error exporting data", Details = ex.Message });
            }

            var fileName = $"file_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }
    }
}
