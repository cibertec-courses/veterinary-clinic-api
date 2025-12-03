
namespace VeterinaryClinic.Application.DTOs.Appointment
{
    public class CreateAppointmentDto
    {
      
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public int PetId { get; set; }
    }
}