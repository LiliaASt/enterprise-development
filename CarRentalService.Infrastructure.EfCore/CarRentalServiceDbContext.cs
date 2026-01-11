using CarRentalService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CarRentalService.Infrastructure.EfCore;

/// <summary>
/// Database context for Car Rental Service using MongoDB
/// </summary>
public class CarRentalDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the CarRentalDbContext
    /// </summary>
    /// <param name="options">DbContext options</param>
    public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
    {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }
    /// <summary>
    /// Gets or sets the CarModels collection
    /// </summary>
    public DbSet<CarModel> CarModels { get; set; } = null!;
    /// <summary>
    /// Gets or sets the CarModelGenerations collection
    /// </summary>
    public DbSet<CarModelGeneration> CarModelGenerations { get; set; } = null!;
    /// <summary>
    /// Gets or sets the Cars collection
    /// </summary>
    public DbSet<Car> Cars { get; set; } = null!;
    /// <summary>
    /// Gets or sets the Customers collection
    /// </summary>
    public DbSet<Customer> Customers { get; set; } = null!;
    /// <summary>
    /// Gets or sets the Rents collection
    /// </summary>
    public DbSet<Rent> Rents { get; set; } = null!;

    /// <summary>
    /// Configures the database context options
    /// </summary>
    /// <param name="optionsBuilder">Options builder</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableThreadSafetyChecks(false);
    }

    /// <summary>
    /// Configures the entity models and relationships for MongoDB
    /// </summary>
    /// <param name="modelBuilder">Model builder instance</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // CarModel configuration
        modelBuilder.Entity<CarModel>(builder =>
        {
            builder.ToCollection("car_models");
            builder.HasKey(cm => cm.Id);
            builder.Property(cm => cm.Id).HasElementName("_id");
            builder.Property(cm => cm.Name).IsRequired().HasElementName("name");
            builder.Property(cm => cm.DriveType).IsRequired().HasElementName("drive_type");
            builder.Property(cm => cm.SeatCount).IsRequired().HasElementName("seat_count");
            builder.Property(cm => cm.BodyType).IsRequired().HasElementName("body_type");
            builder.Property(cm => cm.CarClass).IsRequired().HasElementName("car_class");
        });

        // CarModelGeneration configuration
        modelBuilder.Entity<CarModelGeneration>(builder =>
        {
            builder.ToCollection("car_model_generations");
            builder.HasKey(cmg => cmg.Id);
            builder.Property(cmg => cmg.Id).HasElementName("_id");
            builder.Property(cmg => cmg.CarModelId).IsRequired().HasElementName("car_model_id");
            builder.Property(cmg => cmg.ProductionYear).IsRequired().HasElementName("production_year");
            builder.Property(cmg => cmg.EngineVolume).IsRequired().HasElementName("engine_volume");
            builder.Property(cmg => cmg.TransmissionType).IsRequired().HasElementName("transmission_type");
            builder.Property(cmg => cmg.RentalCostPerHour).IsRequired().HasElementName("rental_cost_per_hour");
            builder.HasOne(cmg => cmg.CarModel)
               .WithMany()
               .HasForeignKey(cmg => cmg.CarModelId);
        });

        // Car configuration
        modelBuilder.Entity<Car>(builder =>
        {
            builder.ToCollection("cars");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasElementName("_id");
            builder.Property(c => c.LicensePlate).IsRequired().HasElementName("license_plate");
            builder.Property(c => c.Color).IsRequired().HasElementName("color");
            builder.Property(c => c.CarModelGenerationId).IsRequired().HasElementName("car_model_generation_id");
            builder.HasOne(c => c.CarModelGeneration)
               .WithMany()
               .HasForeignKey(c => c.CarModelGenerationId);
        });

        // Customer configuration
        modelBuilder.Entity<Customer>(builder =>
        {
            builder.ToCollection("customers");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasElementName("_id");
            builder.Property(c => c.DriverLicenseNumber).IsRequired().HasElementName("driver_license_number");
            builder.Property(c => c.FullName).IsRequired().HasElementName("full_name");
            builder.Property(c => c.DateOfBirth).IsRequired().HasElementName("date_of_birth");
        });

        // Rent configuration
        modelBuilder.Entity<Rent>(builder =>
        {
            builder.ToCollection("rents");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).HasElementName("_id");
            builder.Property(r => r.CarId).IsRequired().HasElementName("car_id");
            builder.Property(r => r.CustomerId).IsRequired().HasElementName("customer_id");
            builder.Property(r => r.StartTime).IsRequired().HasElementName("start_time");
            builder.Property(r => r.Duration).IsRequired().HasElementName("duration");
            builder.HasOne(r => r.Car)
               .WithMany()
               .HasForeignKey(r => r.CarId);
            builder.HasOne(r => r.Customer)
               .WithMany()
               .HasForeignKey(r => r.CustomerId);
        });
    }
}
