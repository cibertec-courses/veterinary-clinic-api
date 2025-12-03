
namespace VeterinaryClinic.Domain.Entities
{
    public class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Pet> Pets { get; set; } = new List<Pet>();

        public string GetFullName() => $"{FirstName} {LastName}";




    }
}