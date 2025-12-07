using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Application.DTOs.Owner;
using VeterinaryClinic.Application.Interfaces;
using VeterinaryClinic.Domain.Exceptions;

namespace VeterinaryClinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnersController : ControllerBase
{
    private readonly IOwnerService _ownerService;

    public OwnersController(IOwnerService ownerService)
    {
        _ownerService = ownerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OwnerDto>>> GetAll()
    {
        var owners = await _ownerService.GetAllAsync();
        return Ok(owners);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OwnerDto>> GetById(int id)
    {
        var owner = await _ownerService.GetByIdAsync(id);
        if (owner == null)
            return NotFound(new { message = $"Owner with ID {id} not found." });
        
        return Ok(owner);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<OwnerDto>> GetByEmail(string email)
    {
        var owner = await _ownerService.GetByEmailAsync(email);
        if (owner == null)
            return NotFound(new { message = $"Owner with email {email} not found." });
        
        return Ok(owner);
    }

    [HttpGet("search/{name}")]
    public async Task<ActionResult<IEnumerable<OwnerDto>>> SearchByName(string name)
    {
        var owners = await _ownerService.SearchByNameAsync(name);
        return Ok(owners);
    }

    [HttpPost]
    public async Task<ActionResult<OwnerDto>> Create([FromBody] CreateOwnerDto dto)
    {
        try
        {
            var owner = await _ownerService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = owner.Id }, owner);
        }
        catch (DuplicateEntityException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OwnerDto>> Update(int id, [FromBody] UpdateOwnerDto dto)
    {
        try
        {
            var owner = await _ownerService.UpdateAsync(id, dto);
            return Ok(owner);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (DuplicateEntityException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _ownerService.DeleteAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}