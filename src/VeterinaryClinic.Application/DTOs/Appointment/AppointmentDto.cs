
namespace VeterinaryClinic.Application.DTOs.Appointment
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public bool CanBeCancelled { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PetId { get; set; }
        public string PetName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
    }
}