/// <summary>
/// Aspire AppHost for Car Rental Service application orchestration
/// </summary>
var builder = DistributedApplication.CreateBuilder(args);

builder.Configuration["Logging:EventLog:LogLevel:Default"] = "None";
builder.Configuration["Logging:EventLog:LogLevel:Microsoft.AspNetCore"] = "None";

// Add MongoDB
var mongo = builder.AddMongoDB("mongo").AddDatabase("carrental");

// Add generator with parameters
var batchSize = builder.AddParameter("GeneratorBatchSize");
var waitTime = builder.AddParameter("GeneratorWaitTime");

var generator = builder.AddProject<Projects.CarRentalService_Generator_Grpc_Host>("generator")
    .WithEnvironment("Generator:BatchSize", batchSize)
    .WithEnvironment("Generator:WaitTime", waitTime);

// Add main API
builder.AddProject<Projects.CarRentalService_API>("carrental-api")
    .WithReference(mongo)
    .WithReference(generator)
    .WithEnvironment("RentalGenerator:GrpcAddress", generator.GetEndpoint("https"))
    .WaitFor(mongo)
    .WaitFor(generator);

builder.Build().Run();
