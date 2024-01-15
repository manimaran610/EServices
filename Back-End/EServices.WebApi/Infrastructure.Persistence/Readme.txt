Execute in cmd
dotnet ef --startup-project ..\WebApi  migrations add AlterRoomGrillstype --context ApplicationDbContext 
dotnet ef --startup-project ..\WebApi database update  --context ApplicationDbContext 