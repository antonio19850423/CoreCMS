using Microsoft.AspNetCore.Mvc;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceTypeController : ControllerBase
    {
        private readonly IResourceTypeService _resourceTypeService;
        private readonly ITransactionService _transactionService;

        public ResourceTypeController(IResourceTypeService resourceTypeService, ITransactionService transactionService)
        {
            _resourceTypeService = resourceTypeService;
            _transactionService = transactionService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _resourceTypeService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _resourceTypeService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] ResourceTypeCrud resourceType)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultDto<ResourceTypeDto>
                {
                    Success = false,
                    Message = "Invalid model",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });

            var result = await _resourceTypeService.CreateAsync(resourceType);
            if (!result.Success)
                return BadRequest(result);

            await _transactionService.CommitAsync();
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] ResourceTypeCrud dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultDto<ResourceTypeDto>
                {
                    Success = false,
                    Message = "Invalid model",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });

            var result = await _resourceTypeService.UpdateAsync(dto);
            if (!result.Success)
                return NotFound(result);

            await _transactionService.CommitAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _resourceTypeService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result);

            await _transactionService.CommitAsync();
            return Ok(result);
        }
    }
}
