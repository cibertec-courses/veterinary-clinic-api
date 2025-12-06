using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Domain.Entities;
using VeterinaryClinic.Domain.Ports.Out;
using VeterinaryClinic.Infrastructure.Persistence.Contexts;

namespace VeterinaryClinic.Infrastructure.Persistence.Repositories
{
    public class OwnerRepository : Repository<Owner>, IOwnerRepository
    {
        public OwnerRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<Owner?> GetByEmailAsync(string email)
        {
            return await _dbSet.
                FirstOrDefaultAsync(o => o.Email == email);
        }


        public async Task<Owner?> GetWithPetsAsync(int id)
        {
            return await _dbSet
                .Include(o => o.Pets)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Owner>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(o => EF.Functions.Like(o.FirstName, $"%{name}%") ||
                            EF.Functions.Like(o.LastName, $"%{name}%"))
                .ToListAsync();
        }
       
    }
}