using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Domain.Entities;
using VeterinaryClinic.Domain.Ports.Out;
using VeterinaryClinic.Infrastructure.Persistence.Context;

namespace VeterinaryClinic.Infrastructure.Persistence.Repositories
{
    public class PetRepository : Repository<Pet>, IPetRepository
    {
        public PetRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Pet>> GetByOwnerIdAsync(int ownerId)
        {
            return await _dbSet
                .Where(p => p.OwnerId == ownerId)
                .ToListAsync();
        }

        public async Task<Pet?> GetWithOwnerAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pet>> GetBySpeciesAsync(string species)
        {
            return await _dbSet
                .Where(p => p.Species == species)
                .ToListAsync();
        }
        
    }
}