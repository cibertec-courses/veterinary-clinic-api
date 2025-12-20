
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Application.DTOs.Pet;
using VeterinaryClinic.Application.Interfaces;
using VeterinaryClinic.Domain.Exceptions;

namespace VeterinaryClinic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PetsController: ControllerBase
    {
        private readonly IPetService _petService;

        public PetsController(IPetService petService)
        {
            _petService = petService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetAll()
        {
            var pets = await _petService.GetAllAsync();
            return Ok(pets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PetDto>> GetById(int id)
        {
            var pet = await _petService.GetByIdAsync(id);           
            return Ok(pet);
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetByOwnerId(int ownerId)
        {
            try
            {
                var pets = await _petService.GetByOwnerIdAsync(ownerId);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("species/{species}")]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetBySpecies(string species)
        { 
            var pets = await _petService.GetBySpeciesAsync(species);
            return Ok(pets);
        }

        [HttpPost]
        public async Task<ActionResult<PetDto>> Create([FromBody] CreatePetDto dto)
        {
            try
            {
                var pet = await _petService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
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

        [HttpPut("{id}")]
        public async Task<ActionResult<PetDto>> Update(int id, [FromBody] UpdatePetDto dto)
        {
            try
            {
                var pet = await _petService.UpdateAsync(id, dto);
                return Ok(pet);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _petService.DeleteAsync(id);
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
}