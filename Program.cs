using FLyTicketService.Data;
using FLyTicketService.Mapper;
using FLyTicketService.Middleware;
using FLyTicketService.Model;
using FLyTicketService.Repositories;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service;
using FLyTicketService.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add FlyTicketDbContext
builder.Services.AddDbContext<FLyTicketDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// Correct AutoMapper configuration
builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); }, AppDomain.CurrentDomain.GetAssemblies());

// Register repositories and services

builder.Services.AddScoped<IGenericRepository<Ticket>, GenericRepository<Ticket>>();
builder.Services.AddScoped<IGenericRepository<Tenant>, GenericRepository<Tenant>>();
builder.Services.AddScoped<IGenericRepository<FlightSchedule>, GenericRepository<FlightSchedule>>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IFlightScheduleService, FlightScheduleService>();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    try
    {
        FLyTicketDbContext context = services.GetRequiredService<FLyTicketDbContext>();
        context.Database.EnsureCreated(); // For initial database creation
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