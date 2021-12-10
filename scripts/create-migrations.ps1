# USe this script to create a migration and create a certificate to be able to run our Blazor server.

# Migrations
Write-Host "CREATING MIGRATIONS..."
dotnet ef --startup-project ..\ProjectBank.Server\ migrations add InitialCreate --project ..\ProjectBank.InfraStructure
Write-Host "DONE."