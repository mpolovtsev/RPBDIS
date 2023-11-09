namespace HeatEnergyConsumption.Models
{
    public partial class Organization
    {
        public Organization()
        {
            ChiefPowerEngineers = new HashSet<ChiefPowerEngineer>();
            HeatEnergyConsumptionRates = new HashSet<HeatEnergyConsumptionRate>();
            ProducedProducts = new HashSet<ProducedProduct>();
            ProvidedServices = new HashSet<ProvidedService>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int OwnershipFormId { get; set; }
        public string Address { get; set; } = null!;
        public int? ManagerId { get; set; }

        public virtual Manager? Manager { get; set; }
        public virtual OwnershipForm OwnershipForm { get; set; } = null!;
        public virtual ICollection<ChiefPowerEngineer> ChiefPowerEngineers { get; set; }
        public virtual ICollection<HeatEnergyConsumptionRate> HeatEnergyConsumptionRates { get; set; }
        public virtual ICollection<ProducedProduct> ProducedProducts { get; set; }
        public virtual ICollection<ProvidedService> ProvidedServices { get; set; }
    }
}