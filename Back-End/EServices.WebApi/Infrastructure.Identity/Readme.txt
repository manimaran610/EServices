Execute in cmd
dotnet ef --startup-project ..\WebApi  migrations add AddIsRestricted --context IdentityContext 
dotnet ef --startup-project ..\WebApi database update  --context IdentityContext 