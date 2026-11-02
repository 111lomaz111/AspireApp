var builder = DistributedApplication.CreateBuilder(args);

// Add the following line to configure the Docker Compose environment
builder.AddDockerComposeEnvironment("env");

var apiServicePort = builder.AddParameter("apiservice-port", "8081");
var webFrontendPort = builder.AddParameter("webfrontend-port", "8080");

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.AspireApp_ApiService>("apiservice")
    .WithEnvironment("HTTP_PORTS", apiServicePort)
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.AspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithEnvironment("HTTP_PORTS", webFrontendPort)
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);


builder.Build().Run();
