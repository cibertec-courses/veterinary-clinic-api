
using VeterinaryClinic.Application.DTOs.Owner;

namespace VeterinaryClinic.Application.Interfaces
{
    public interface IOwnerService
    {
        Task<OwnerDto?> GetByIdAsync(int id);
        Task<IEnumerable<OwnerDto?>> GetAllAsync();
        Task<OwnerDto> CreateAsync(CreateOwnerDto ownerDto);
        Task<OwnerDto> UpdateAsync(int id, UpdateOwnerDto ownerDto);
        
        Task<bool> DeleteAsync(int id);

        Task<OwnerDto?> GetByEmailAsync(string email);
        Task<IEnumerable<OwnerDto>> SearchByNameAsync(string name);
    }
}