
using VeterinaryClinic.Domain.Entities;

namespace VeterinaryClinic.Domain.Ports.Out
{
    public interface IOwnerRepository : IRepository<Owner>
    {
        Task<Owner?> GetByEmailAsync(string email);
        Task<Owner?> GetWithPetsAsync(int id);
        Task<IEnumerable<Owner>> SearchByNameAsync(string name);
    }


}