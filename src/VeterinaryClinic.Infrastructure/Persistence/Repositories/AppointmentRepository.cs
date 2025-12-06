using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Domain.Entities;
using VeterinaryClinic.Domain.Ports.Out;
using VeterinaryClinic.Infrastructure.Persistence.Context;

namespace VeterinaryClinic.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Appointment>> GetByPetIdAsync(int petId)
        {
            return await _dbSet
                .Where(a => a.PetId == petId)
                .Include(a => a.Pet)
                    .ThenInclude(p => p!.Owner)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate)
                .Include(a => a.Pet)
                    .ThenInclude(p => p!.Owner)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(a => a.Status == status)
                .Include(a => a.Pet)
                    .ThenInclude(p => p!.Owner)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<Appointment?> GetWithPetAndOwnerAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Pet)
                    .ThenInclude(p => p!.Owner)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}