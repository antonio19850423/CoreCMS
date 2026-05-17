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
    public class ComponentTypeController : ControllerBase
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
    }
}
