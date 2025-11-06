using FLyTicketService.Data;
using FLyTicketService.Middleware;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service;
using FLyTicketService.Service.Interfaces;
using FLyTicketService.Services;
using FLyTicketService.Services.Interfaces;
using FLyTicketService.Shared;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
       .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new EnumConverter<SimplyTimeZone>());
            options.JsonSerializerOptions.Converters.Add(new EnumConverter<DaylightSavingTime>());
            options.JsonSerializerOptions.Converters.Add(new EnumConverter<SeatClass>());
            options.JsonSerializerOptions.Converters.Add(new EnumConverter<DiscountCondition>());
            options.JsonSerializerOptions.Converters.Add(new EnumConverter<DiscountCategory>());
            options.JsonSerializerOptions.Converters.Add(new EnumConverter<DaysOfWeekMask>());
            options.JsonSerializerOptions.Converters.Add(new EnumConverter<TenantGroup>());
            options.JsonSerializerOptions.Converters.Add(new EnumConverter<TicketStatus>());
        });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add FlyTicketDbContext
builder.Services.AddDbContext<FLyTicketDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection") ??
            Environment.GetEnvironmentVariable("CONNECTION_STRING") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found."),
            sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

// Register repositories and services

builder.Services.AddScoped<IGenericRepository<Ticket>, GenericRepository<Ticket>>();
builder.Services.AddScoped<IGenericRepository<Tenant>, GenericRepository<Tenant>>();
builder.Services.AddScoped<IGenericRepository<FlightSchedule>, GenericRepository<FlightSchedule>>();
builder.Services.AddScoped<IGenericRepository<Discount>, GenericRepository<Discount>>();
builder.Services.AddScoped<IGenericRepository<Aircraft>, GenericRepository<Aircraft>>();
builder.Services.AddScoped<IGenericRepository<Airline>, GenericRepository<Airline>>();
builder.Services.AddScoped<IGenericRepository<Airport>, GenericRepository<Airport>>();
builder.Services.AddScoped<IGenericRepository<FlightSeat>, GenericRepository<FlightSeat>>();
builder.Services.AddScoped<IGenericRepository<AircraftSeat>, GenericRepository<AircraftSeat>>();
builder.Services.AddScoped<IGenericRepository<Condition>, GenericRepository<Condition>>();

builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IFlightScheduleService, FlightScheduleService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IFlightPriceService, FlightPriceService>();
builder.Services.AddTransient<IGroupStrategy, GroupAStrategy>();
builder.Services.AddTransient<IGroupStrategy, GroupBStrategy>();
builder.Services.AddSingleton<IGroupStrategyFactory, GroupStrategyFactory>();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    try
    {
        FLyTicketDbContext context = services.GetRequiredService<FLyTicketDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>(); // Add custom exception handling middleware

app.MapControllers();

app.Run();