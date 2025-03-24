var builder = DistributedApplication.CreateBuilder(args);

// To connect to existing Redis instance use the following method: builder.AddConnectionString
// See: https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/app-host-overview#reference-existing-resources
var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.gnr_v1_ApiService>("apiservice");

// TODO: update scriptName: "dev" arg when UI.package.json.scripts.start (prod) script exists
builder.AddNpmApp("reacttypescript", "../gnr-v1.UI", "dev")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .WithEnvironment("BROWSER", "none")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.gnr_v1_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
