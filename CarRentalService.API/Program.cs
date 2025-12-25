using CarRentalService.Application.Contracts;
using CarRentalService.Application.Services;
using CarRentalService.Application.Contracts.Cars;
using CarRentalService.Application.Contracts.Clients;
using CarRentalService.Application.Contracts.Rents;
using CarRentalService.Domain.TestData;
using Mapster;
using MapsterMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Car Rental Service API",
        Version = "v1",
        Description = "API для сервиса аренды автомобилей. Лабораторная работа №2"
    });
});

// Register test data
builder.Services.AddSingleton<TestData>();

// Register services
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IRentService, RentService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// Configure Mapster
TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// Build the application
var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Rental API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
