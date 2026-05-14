using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _resourceService;
        private readonly ITransactionService _transactionService;
        private readonly IResourceCacheService _resourceCacheService;
        private readonly IGeneralContextService _generalContextService;


        public ResourceController(IResourceService resourceService, ITransactionService transactionService, IResourceCacheService resourceCacheService, IGeneralContextService generalContextService)
        {
            _resourceService = resourceService;
            _transactionService = transactionService;
            _resourceCacheService = resourceCacheService;
            _generalContextService = generalContextService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _resourceService.GetAllAsync();
            return Ok(result);
        }
        /// <summary>
        /// Retrieves cached resources filtered by resource codes and resource type codes.
        /// Supports case-insensitive matching for resource codes and allows filtering by multiple resource type codes.
        /// </summary>
        /// <param name="resourceCodes">List of resource code patterns to include in the result (case-insensitive, partial match).</param>
        /// <returns>Returns the list of resources from cache that match the specified resource codes and resource type codes.</returns>
        [HttpGet("GetAllCacheView")]
        public async Task<IActionResult> GetAllCacheView([FromQuery] string[] resourceCodes)
        {
            var data = await _resourceCacheService.GetResourcesAsync(_generalContextService.CurrentLanguage, resourceCodes);
            return StatusCode(data.StatusCode, data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _resourceService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] ResourceCrud resource)
        {
            var result = await _resourceService.CreateAsync(resource);
            if (!result.Success)
                return BadRequest(result);

            await _transactionService.CommitAsync();
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] ResourceCrud resource)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(new ResultDto<ResourceDto>
            //    {
            //        Success = false,
            //        Message = "Invalid model",
            //        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            //    });

            var result = await _resourceService.UpdateAsync(resource);
            if (!result.Success)
                return NotFound(result);

            await _transactionService.CommitAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _resourceService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result);

            await _transactionService.CommitAsync();
            return Ok(result);
        }
        [HttpGet("Export")]
        [AllowAnonymous]
        public async Task<IActionResult> Export([FromQuery] bool exportCurrentPage = false,
                                        [FromQuery] int pageNumber = 1,
                                        [FromQuery] int pageSize = 50)
        {
            // 1️⃣ گرفتن فایل اکسل به صورت بایت آرایه
            byte[] fileBytes;
            try
            {
                fileBytes = await _resourceService.ExportAsync(exportCurrentPage, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error exporting data", Details = ex.Message });
            }

            // 2️⃣ تنظیم هدر پاسخ برای دانلود فایل
            var fileName = $"Resources_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(fileBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }

    }
}
