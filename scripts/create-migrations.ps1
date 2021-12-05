# USe this script to create a migration and create a certificate to be able to run our Blazor server.

# Migrations
Write-Host "CREATING MIGRATIONS..."
Set-Location ..\ProjectBank.Infrastructure
dotnet ef --startup-project ..\ProjectBank.Server\ migrations add InitialCreate
Set-Location ..
Write-Host "DONE."