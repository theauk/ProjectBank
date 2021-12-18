# USe this script to create a migration.

# Migrations
Write-Host "CREATING MIGRATIONS..."

dotnet ef --startup-project ../ProjectBank.Server/ migrations add InitialCreate --project ../ProjectBank.InfraStructure

Write-Host "DONE."