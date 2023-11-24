Execute in cmd
dotnet ef --startup-project ..\WebApi  migrations add GrillColumnAlter1 --context ApplicationDbContext 
dotnet ef --startup-project ..\WebApi database update  --context ApplicationDbContext 