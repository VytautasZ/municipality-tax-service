var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sql");
var db = sqlServer.AddDatabase("MunicipalityTaxDbString");

builder.AddProject<Projects.MunicipalityTaxService_Api>("api")
       .WithReference(db)
       .WaitFor(sqlServer);

builder.Build().Run();
