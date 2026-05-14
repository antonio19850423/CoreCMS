using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Services;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeResource(AppRoles.Developer, AppRoles.Admin)]
    public class permissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ITransactionService _transactionService;

        public permissionController(IPermissionService permissionService, ITransactionService transactionService)
        {
            _permissionService = permissionService;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _permissionService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _permissionService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] PermissionCrud dto)
        {
            var result = await _permissionService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            await _transactionService.CommitAsync();
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] PermissionCrud dto)
        {
            var result = await _permissionService.UpdateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            await _transactionService.CommitAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _permissionService.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result);


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
            var result = await _permissionService.BulkInsertAsync(stream);

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
                fileBytes = await _permissionService.ExportAsync(
                    request.ExportCurrentPage,
                    request.PageNumber,
                    request.PageSize
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error exporting data", Details = ex.Message });
            }

            var fileName = $"permission_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }


    }
}
