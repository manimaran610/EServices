Execute in cmd
dotnet ef --startup-project ..\WebApi  migrations add AlterTrainee-addempId --context ApplicationDbContext 
dotnet ef --startup-project ..\WebApi database update  --context ApplicationDbContext 