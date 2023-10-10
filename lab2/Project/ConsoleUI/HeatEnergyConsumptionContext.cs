using ConsoleUI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleUI
{
    public partial class HeatEnergyConsumptionContext : DbContext
    {
        string connectionString;

        public HeatEnergyConsumptionContext(string connectionString) =>
            this.connectionString = connectionString;

        public HeatEnergyConsumptionContext(DbContextOptions<HeatEnergyConsumptionContext> options, string connectionString)
            : base(options) => 
            this.connectionString = connectionString;

        public string ConnectionString => connectionString;

        public virtual DbSet<ChiefPowerEngineer> ChiefPowerEngineers { get; set; } = null!;
        public virtual DbSet<HeatEnergyConsumptionRate> HeatEnergyConsumptionRates { get; set; } = null!;
        public virtual DbSet<Manager> Managers { get; set; } = null!;
        public virtual DbSet<Organization> Organizations { get; set; } = null!;
        public virtual DbSet<OwnershipForm> OwnershipForms { get; set; } = null!;
        public virtual DbSet<ProducedProduct> ProducedProducts { get; set; } = null!;
        public virtual DbSet<ProductsType> ProductsTypes { get; set; } = null!;
        public virtual DbSet<ProvidedService> ProvidedServices { get; set; } = null!;
        public virtual DbSet<ServicesType> ServicesTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer(connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiefPowerEngineer>(entity =>
            {
                entity.HasIndex(e => e.PhoneNumber, "UQ_ChiefPowerEngineer_PhoneNumber")
                    .IsUnique();

                entity.Property(e => e.MiddleName).HasMaxLength(20);

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(12)
                    .IsFixedLength();

                entity.Property(e => e.Surname).HasMaxLength(20);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.ChiefPowerEngineers)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_ChiefPowerEngineer_OrganizationId");
            });

            modelBuilder.Entity<HeatEnergyConsumptionRate>(entity =>
            {
                entity.HasIndex(e => new { e.OrganizationId, e.ProductTypeId, e.Date }, "UQ_HeatEnergyConsumptionRate_Record")
                    .IsUnique();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.HeatEnergyConsumptionRates)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_HeatEnergyConsumptionRate_OrganizationId");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.HeatEnergyConsumptionRates)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK_HeatEnergyConsumptionRate_ProductTypeId");
            });

            modelBuilder.Entity<Manager>(entity =>
            {
                entity.HasIndex(e => e.PhoneNumber, "UQ_Manager_PhoneNumber")
                    .IsUnique();

                entity.Property(e => e.MiddleName).HasMaxLength(20);

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(12)
                    .IsFixedLength();

                entity.Property(e => e.Surname).HasMaxLength(20);
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.ManagerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Organization_ManagerId");

                entity.HasOne(d => d.OwnershipForm)
                    .WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.OwnershipFormId)
                    .HasConstraintName("FK_Organization_OwnershipFormId");
            });

            modelBuilder.Entity<OwnershipForm>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ_OwnershipForm_Name")
                    .IsUnique();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<ProducedProduct>(entity =>
            {
                entity.HasIndex(e => new { e.OrganizationId, e.ProductTypeId, e.Date }, "UQ_ProducedProduct_Record")
                    .IsUnique();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.ProducedProducts)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_ProducedProduct_OrganizationId");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.ProducedProducts)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK_ProducedProduct_ProductTypeId");
            });

            modelBuilder.Entity<ProductsType>(entity =>
            {
                entity.HasIndex(e => e.Code, "UQ_ProductType_Code")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Unit).HasMaxLength(10);
            });

            modelBuilder.Entity<ProvidedService>(entity =>
            {
                entity.HasIndex(e => new { e.OrganizationId, e.ServiceTypeId, e.Date }, "UQ_ProvidedServices_Record")
                    .IsUnique();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.ProvidedServices)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_ProvidedService_OrganizationId");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.ProvidedServices)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("FK_ProvidedService_ServiceTypeId");
            });

            modelBuilder.Entity<ServicesType>(entity =>
            {
                entity.HasIndex(e => e.Code, "UQ_ServiceType_Code")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Unit).HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
