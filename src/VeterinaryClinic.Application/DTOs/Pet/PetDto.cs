
namespace VeterinaryClinic.Application.DTOs.Pet
{
    public class PetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int AgeInYears { get; set; }

        public DateTime CreatedAt { get; set; }
        public int OwnerId { get; set; }

        public string OwnerName { get; set; } = string.Empty;
    }
}