using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public partial class Organization
    {
        public Organization() { }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int OwnershipFormId { get; set; }

        public string Address { get; set; } = null!;

        public int? ManagerId { get; set; }

        [JsonIgnore]
        public virtual Manager? Manager { get; set; }

        [JsonIgnore]
        public virtual OwnershipForm? OwnershipForm { get; set; }
    }
}