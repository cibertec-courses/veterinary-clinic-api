namespace VeterinaryClinic.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "Scheduled";
        public string? Notes { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        

        public int PetId { get; set; }
        public Pet? Pet { get; set; }


        public bool IsFutureAppointment() => AppointmentDate > DateTime.Now;

        public bool CanBeCancelled() => Status == "Scheduled" && IsFutureAppointment();

    }
}