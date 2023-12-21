using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public partial class OwnershipForm
    {
        public OwnershipForm()
        {
            Organizations = new HashSet<Organization>();
        }

        public int Id { get; set; }
        
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}