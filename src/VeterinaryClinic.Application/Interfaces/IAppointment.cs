
using VeterinaryClinic.Application.DTOs.Appointment;

namespace VeterinaryClinic.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto appointmentDto);
        Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto appointmentDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> CancelAsync(int id);

        Task<IEnumerable<AppointmentDto>> GetByPetIdAsync(int petId); 
        Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AppointmentDto>> GetByStatusAsync(string status);
    }
}