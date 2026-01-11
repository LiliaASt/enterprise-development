using Microsoft.EntityFrameworkCore;
using CarRentalService.Domain;
using CarRentalService.Domain.Models;
using CarRentalService.Application.Contracts.Cars;
using CarRentalService.Application.Contracts.Clients;
using CarRentalService.Application.Contracts.Rents;
using CarRentalService.Application.Contracts.CarModel;
using CarRentalService.Application.Contracts.CarModelGeneration;
using CarRentalService.Application.Contracts;
using CarRentalService.Application;
using CarRentalService.Application.Services;
using CarRentalService.Infrastructure.EfCore.Repositories;
using CarRentalService.Infrastructure.EfCore;
using CarRentalService.Domain.TestData;
using CarRentalService.ServiceDefaults;
using MongoDB.Driver;
using System.Text.Json.Serialization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// AutoMapper
builder.Services.AddAutoMapper(config => config.AddProfile<CarRentalProfile>());

// Test data
builder.Services.AddSingleton<TestData>();

// Repository
builder.Services.AddScoped<IRepository<Car, int>, CarEfCoreRepository>();
builder.Services.AddScoped<IRepository<CarModel, int>, CarModelEfCoreRepository>();
builder.Services.AddScoped<IRepository<CarModelGeneration, int>, CarModelGenerationEfCoreRepository>();
builder.Services.AddScoped<IRepository<Customer, int>, CustomerEfCoreRepository>();
builder.Services.AddScoped<IRepository<Rent, int>, RentEfCoreRepository>();

// Service
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IRentService, RentService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<ICarModelService, CarModelService>();
builder.Services.AddScoped<ICarModelGenerationService, CarModelGenerationService>();

builder.AddMongoDBClient("carrental");

builder.Services.AddDbContext<CarRentalDbContext>((services, options) =>
{
    var db = services.GetRequiredService<IMongoDatabase>();

    options.UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Controller
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseCors("AllowAll");

// Database seeding
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
    var testData = scope.ServiceProvider.GetRequiredService<TestData>();

    context.Database.EnsureCreated();

    if (!context.CarModels.Any())
    {
        Console.WriteLine("Database is empty, seeding with test data...");

        context.CarModels.AddRange(testData.CarModels);
        context.SaveChanges();
        context.CarModelGenerations.AddRange(testData.CarModelGenerations);
        context.SaveChanges();
        context.Customers.AddRange(testData.Customers);
        context.SaveChanges();
        context.Cars.AddRange(testData.Cars);
        context.SaveChanges();
        context.Rents.AddRange(testData.Rents);
        context.SaveChanges();
        Console.WriteLine("Database seeded successfully with test data!");
    }
    else
    {
        Console.WriteLine("Database already contains data.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error during database seeding: {ex.Message}");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
