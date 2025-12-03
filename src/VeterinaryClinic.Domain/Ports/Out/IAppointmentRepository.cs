
using VeterinaryClinic.Domain.Entities;

namespace VeterinaryClinic.Domain.Ports.Out
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetByPetIdAsync(int petId);
        Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Appointment>> GetByStatusAsync(string status);
        Task<Appointment> GetWithPetAndOwnerAsync(int id);
    }
}