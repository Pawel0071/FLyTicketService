using FLyTicketService.Data.Configuration;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace FLyTicketService.Data
{
    public sealed class FLyTicketDbContext: DbContext
    {
        #region Fields

        private readonly IConfiguration _configuration;

        #endregion

        #region Properties

        public DbSet<Airport> Airports { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<AircraftSeat> AircraftSeats { get; set; }
        public DbSet<FlightSeat> FlightSeats { get; set; }
        public DbSet<FlightSchedule> FlightScheduler { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Discount> DiscountTypes { get; set; }

        #endregion

        #region Constructors

        public FLyTicketDbContext(DbContextOptions<FLyTicketDbContext> options, IConfiguration configuration): base(options)
        {
            _configuration = configuration;
             //Database.EnsureCreated();
        }

        #endregion

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string? connectionString = _configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");
                }

                optionsBuilder.UseSqlServer(connectionString, options =>
                {
                    options.MigrationsHistoryTable("__EFMigrationsHistory", "FLyTicket");
                });

                optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
            }

            if (_configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.ConfigureWarnings(warnings =>
                    warnings.Throw(RelationalEventId.PendingModelChangesWarning));
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new FlightSeatConfiguration());
            modelBuilder.ApplyConfiguration(new FlightScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new AircraftSeatConfiguration());
            modelBuilder.ApplyConfiguration(new AircraftConfiguration());
            modelBuilder.ApplyConfiguration(new AirportConfiguration());
            modelBuilder.ApplyConfiguration(new AirlineConfiguration());
            modelBuilder.ApplyConfiguration(new TenantConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountConfiguration());
            modelBuilder.ApplyConfiguration(new ConditionConfiguration());

            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnumConverter<SimplyTimeZone>());
            options.Converters.Add(new EnumConverter<DaylightSavingTime>());
            options.Converters.Add(new EnumConverter<SeatClass>());
            options.Converters.Add(new EnumConverter<DiscountCondition>());
            options.Converters.Add(new EnumConverter<DiscountCategory>());
            options.Converters.Add(new EnumConverter<DaysOfWeekMask>());
            options.Converters.Add(new EnumConverter<TenantGroup>());
            options.Converters.Add(new EnumConverter<TicketStatus>());

            string airportsJson = File.ReadAllText("Data/WarmingUpData/airports.json");
            List<Airport>? airports = JsonSerializer.Deserialize<List<Airport>>(airportsJson, options);
            if (airports != null)
            {
                modelBuilder.Entity<Airport>().HasData(data: airports);
            }

            string aircraftsJson = File.ReadAllText("Data/WarmingUpData/aircrafts.json");
            List<Aircraft>? aircrafts = JsonSerializer.Deserialize<List<Aircraft>>(aircraftsJson, options);
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

            string airlinesJson = File.ReadAllText("Data/WarmingUpData/airlines.json");
            List<Airline>? airlines = JsonSerializer.Deserialize<List<Airline>>(airlinesJson, options);
            if (airlines != null)
            {
                modelBuilder.Entity<Airline>().HasData(data: airlines);
            }

            string discountJson = File.ReadAllText("Data/WarmingUpData/discount.json");
            List<Discount>? discounts = JsonSerializer.Deserialize<List<Discount>>(discountJson, options);
            List<Condition> discountConditions = new List<Condition>();
            if (discounts != null)
            {
                foreach (Discount discount in discounts)
                {
                    if (discount.Conditions != null)
                    {
                        foreach (Condition condition in discount.Conditions)
                        {
                            condition.DiscountId = discount.DiscountId;
                            discountConditions.Add(condition);
                        }
                    }
                    discount.Conditions = new List<Condition>(); // Initialize with an empty list to avoid null reference
                }
                modelBuilder.Entity<Discount>().HasData(data: discounts);
                modelBuilder.Entity<Condition>().HasData(data: discountConditions);
            }


        }

        #endregion
    }
}