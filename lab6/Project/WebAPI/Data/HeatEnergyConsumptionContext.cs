using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public partial class HeatEnergyConsumptionContext : DbContext
    {
        public HeatEnergyConsumptionContext() { }

        public HeatEnergyConsumptionContext(DbContextOptions<HeatEnergyConsumptionContext> options)
            : base(options) { }

        public virtual DbSet<Manager> Managers { get; set; } = null!;

        public virtual DbSet<Organization> Organizations { get; set; } = null!;

        public virtual DbSet<OwnershipForm> OwnershipForms { get; set; } = null!;
    }
}