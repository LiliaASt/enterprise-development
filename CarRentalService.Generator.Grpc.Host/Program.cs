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

var app = builder.Build();

app.MapGrpcService<CarRentalGrpcGeneratorService>();
app.MapDefaultEndpoints();

app.Run();
