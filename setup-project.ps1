# USe this script to create a migration and create a certificate to be able to run our Blazor server.

# Migrations
Write-Host "Creating Migrations"
Set-Location .\ProjectBank.Infrastructure
dotnet ef --startup-project ..\ProjectBank.Server\ migrations add InitialCreate
Set-Location ..
Write-Host "Done."

# Certificate
Write-Host "Creating Certificate"
dotnet dev-certs https --clean
dotnet dev-certs https -ep ./.local/https/dev_cert.pfx -p localhost
dotnet dev-certs https --trust
Write-Host "Done."
