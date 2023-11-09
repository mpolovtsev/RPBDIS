namespace HeatEnergyConsumption.Models
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

        public virtual ICollection<Organization> Organizations { get; set; }
    }
}