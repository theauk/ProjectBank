# USe this script to create a migration.

# Migrations
Write-Host "CREATING MIGRATIONS..."
if ($IsWindows) {
    dotnet ef --startup-project ..\ProjectBank.Server\ migrations add InitialCreate --project ..\ProjectBank.InfraStructure
}
elseif ($IsMacOS -or $IsLinux) {
    dotnet ef --startup-project ../ProjectBank.Server/ migrations add InitialCreate --project ../ProjectBank.InfraStructure
}
Write-Host "DONE."