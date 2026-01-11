/// <summary>
/// Aspire AppHost for Car Rental Service application orchestration
/// </summary>
var builder = DistributedApplication.CreateBuilder(args);

builder.Configuration["Logging:EventLog:LogLevel:Default"] = "None";
builder.Configuration["Logging:EventLog:LogLevel:Microsoft.AspNetCore"] = "None";

var mongo = builder.AddMongoDB("mongo").AddDatabase("carrental");

builder.AddProject<Projects.CarRentalService_API>("carrental-api")
       .WithReference(mongo)
       .WaitFor(mongo);

builder.Build().Run();
