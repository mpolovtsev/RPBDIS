using System;
using System.Collections.Generic;

namespace ConsoleUI.Models
{
    public partial class ProductsType
    {
        public ProductsType()
        {
            HeatEnergyConsumptionRates = new HashSet<HeatEnergyConsumptionRate>();
            ProducedProducts = new HashSet<ProducedProduct>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;

        public virtual ICollection<HeatEnergyConsumptionRate> HeatEnergyConsumptionRates { get; set; }
        public virtual ICollection<ProducedProduct> ProducedProducts { get; set; }

        public override string ToString()
        {
            return $"Id: {Id},\nCode: {Code},\nName: {Name},\nUnit: {Unit}.";
        }
    }
}
