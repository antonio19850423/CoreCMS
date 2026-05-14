using Microsoft.AspNetCore.Mvc;
using System.Data;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    // GET: api/role
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _roleService.GetAllAsync();
        return Ok(roles);
    }

    // GET: api/role/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var role = await _roleService.GetByIdAsync(id);
        if (role == null)
            return NotFound();

        return Ok(role);
    }

    // POST: api/role
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleDto role)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdRole = await _roleService.CreateAsync(role);
        return CreatedAtAction(nameof(GetById), new { id = createdRole.Data.Id }, createdRole);
    }

    // PUT: api/role/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RoleDto role)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != role.Id)
            return BadRequest("Id mismatch");

        var result = await _roleService.UpdateAsync(role,id);
        if (result!=null)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/role/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _roleService.DeleteAsync(id);
        if (!result.Success)
            return NotFound();

        return NoContent();
    }
}
