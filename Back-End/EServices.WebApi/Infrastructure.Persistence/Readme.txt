Execute in cmd
dotnet ef --startup-project ..\WebApi  migrations add AlterCustomerDetails-Addprops --context ApplicationDbContext 
dotnet ef --startup-project ..\WebApi database update  --context ApplicationDbContext 