using CarRentalService.Generator.Grpc.Host.Grpc;
using CarRentalService.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    options.MaxReceiveMessageSize = 32 * 1024 * 1024;
    options.MaxSendMessageSize = 32 * 1024 * 1024;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Rental Generator API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapGrpcService<CarRentalGrpcGeneratorService>();
app.MapDefaultEndpoints();

app.MapGet("Car Rental Generator gRPC", () => "Car Rental Generator gRPC Service is running!");

app.Run();
