
namespace VeterinaryClinic.Application.DTOs.Pet
{
    public class CreatePetDto
    {
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int OwnerId { get; set; }
    }
}