# Migrations
Write-Host "Creating Migrations"
Set-Location .\Infrastructure
dotnet ef --startup-project ..\Server\ migrations add InitialCreate
Set-Location ..
Write-Host "Done."

# Certificate
Write-Host "Creating Certificate"
dotnet dev-certs https --clean
dotnet dev-certs https -ep ./.local/https/dev_cert.pfx -p localhost
dotnet dev-certs https --trust
Write-Host "Done."
