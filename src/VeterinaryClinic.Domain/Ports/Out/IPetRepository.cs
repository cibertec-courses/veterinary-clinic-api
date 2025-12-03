
using VeterinaryClinic.Domain.Entities;

namespace VeterinaryClinic.Domain.Ports.Out
{
    public interface IPetRepository : IRepository<Pet>
    {
        Task<IEnumerable<Pet>> GetByOwnerIdAsync(int ownerId);
        Task<Pet?> GetWithOwnerAsync(int id);
        Task<IEnumerable<Pet>> GetBySpeciesAsync(string Species);
       
    }
}