
using VeterinaryClinic.Application.DTOs.Pet;

namespace VeterinaryClinic.Application.Interfaces
{
    public interface IPetService
    {
        Task<PetDto> GetByIdAsync(int id);
        Task<IEnumerable<PetDto>> GetAllAsync();
        Task<PetDto> CreateAsync(CreatePetDto petDto);
        Task<PetDto> UpdateAsync(int id, UpdatePetDto petDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<PetDto>> GetByOwnerIdAsync(int ownerId);
        Task<IEnumerable<PetDto>> GetBySpeciesAsync(string species);
    }
}