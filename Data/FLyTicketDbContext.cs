using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace FLyTicketService.Data
{
    public sealed class FLyTicketDbContext: DbContext
    {
        private readonly IConfiguration configuration;

        public FLyTicketDbContext(DbContextOptions<FLyTicketDbContext> options, IConfiguration configuration): base(options)
        {
            this.configuration = configuration;
            Database.EnsureCreated();
        }

        public DbSet<Airline> Operators { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<AircraftSeat> AircraftSeats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The ConnectionString property has not been initialized.");
            }

            optionsBuilder.UseSqlServer(connectionString, options => options.MigrationsHistoryTable("__EFMigrationsHistory", "FLyTicket"))
                          .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                t => t.Namespace == "FLyTicketService.Model");

            // Initial data seeding
            string airportsJson = File.ReadAllText("Data/airports.json");
            List<Airport>? airports = JsonSerializer.Deserialize<List<Airport>>(airportsJson);
            if (airports != null)
            {
                modelBuilder.Entity<Airport>().HasData(data: airports);
            }

            string aircraftsJson = File.ReadAllText("Data/aircrafts.json");
            List<Aircraft>? aircrafts = JsonSerializer.Deserialize<List<Aircraft>>(aircraftsJson);
            List<AircraftSeat> aircraftSeats = new List<AircraftSeat>();

            if (aircrafts != null)
            {
                foreach (Aircraft aircraft in aircrafts)
                {
                    if (aircraft.Seats != null)
                    {
                        foreach (AircraftSeat seat in aircraft.Seats)
                        {
                            seat.AircraftId = aircraft.AircraftId;
                            aircraftSeats.Add(seat);
                        }
                    }

                    aircraft.Seats = new List<AircraftSeat>(); // Initialize with an empty list to avoid null reference
                }

                modelBuilder.Entity<AircraftSeat>().HasData(data: aircraftSeats);
                modelBuilder.Entity<Aircraft>().HasData(data: aircrafts);
            }

            string airlinesJson = File.ReadAllText("Data/airlines.json");
            List<Airline>? airlines = JsonSerializer.Deserialize<List<Airline>>(airlinesJson);
            if (airlines != null)
            {
                modelBuilder.Entity<Airline>().HasData(data: airlines);
            }
        }
    }
}