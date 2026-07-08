var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("sqlserver")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var database = sqlServer.AddDatabase("ceplan");

var webApi = builder
    .AddProject<Projects.Ceplan_WebApi>("webapi")
    .WithReference(database)
    .WaitFor(database);

builder
    .AddProject<Projects.Ceplan_Portal>("portal")
    .WithReference(webApi)
    .WaitFor(webApi);

builder.Build().Run();
