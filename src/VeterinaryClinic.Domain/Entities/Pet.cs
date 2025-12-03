namespace VeterinaryClinic.Domain.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public int OwnerId { get; set; }
        public Owner? Owner { get; set; }


        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public int GetAgeInYears()
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }


    }
}