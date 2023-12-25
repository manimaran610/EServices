Execute in cmd
dotnet ef --startup-project ..\WebApi  migrations add AlterRoomGrills --context ApplicationDbContext 
dotnet ef --startup-project ..\WebApi database update  --context ApplicationDbContext 