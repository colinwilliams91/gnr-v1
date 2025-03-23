var builder = DistributedApplication.CreateBuilder(args);

// To connect to existing Redis instance use the following method: builder.AddConnectionString
// See: https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/app-host-overview#reference-existing-resources
var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.gnr_v1_ApiService>("apiservice");

builder.AddProject<Projects.gnr_v1_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
