using CarRentalService.API.Interfaces;
using CarRentalService.API.Services;
using CarRentalService.Domain.Data;
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

// Register test data (from first lab)
builder.Services.AddSingleton<TestData>();

// Register services
builder.Services.AddScoped<IApplicationService<CarRentalService.API.DTOs.Responses.CarDto,
    CarRentalService.API.DTOs.Requests.CarCreateUpdateDto>, CarService>();

builder.Services.AddScoped<IApplicationService<CarRentalService.API.DTOs.Responses.ClientDto,
    CarRentalService.API.DTOs.Requests.ClientCreateUpdateDto>, ClientService>();

builder.Services.AddScoped<IApplicationService<CarRentalService.API.DTOs.Responses.RentDto,
    CarRentalService.API.DTOs.Requests.RentCreateUpdateDto>, RentService>();

builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// Configure Mapster
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>(); // ПРОСТОЙ ВАРИАНТ

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Rental API v1");
        c.RoutePrefix = string.Empty; // Show Swagger at root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
