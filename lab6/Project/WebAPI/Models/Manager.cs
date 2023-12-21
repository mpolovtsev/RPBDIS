using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public partial class Manager
    {
        public Manager()
        {
            Organizations = new HashSet<Organization>();
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string PhoneNumber { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}