Execute in cmd
dotnet ef --startup-project ..\WebApi  migrations add name --context ApplicationDbContext 
dotnet ef --startup-project ..\WebApi database update  --context ApplicationDbContext 