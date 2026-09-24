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
    public class InventoryManagementController : ControllerBase, IDynamicFormController<InventoryManagementCrud>
    {
        private readonly IInventoryManagementService _InventoryManagementService;
        private readonly ITransactionService _transactionService;

        public InventoryManagementController(IInventoryManagementService InventoryManagementService, ITransactionService transactionService)
        {
            _InventoryManagementService = InventoryManagementService;
            _transactionService = transactionService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _InventoryManagementService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _InventoryManagementService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
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
                fileBytes = await _InventoryManagementService.ExportAsync(
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
        [HttpPost]
        public Task<IActionResult> Create([FromBody] InventoryManagementCrud dto)
        {
            throw new NotImplementedException();
        }
        [HttpPut]
        public Task<IActionResult> Update([FromBody] InventoryManagementCrud dto)
        {
            throw new NotImplementedException();
        }
        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
        [HttpPost("BulkInsert")]
        public Task<IActionResult> BulkInsert()
        {
            throw new NotImplementedException();
        }


    }
}
