
namespace VeterinaryClinic.Application.DTOs.Pet
{
    public class UpdatePetDto
    {
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}