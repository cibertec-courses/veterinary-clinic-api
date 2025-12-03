
namespace VeterinaryClinic.Application.DTOs.Appointment
{
    public class UpdateAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; } 
    }
}