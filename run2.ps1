Write-Host "STARTING VIA PROJECT"
Write-Host

$user = 'postgres'
$password = 'postgress'
$db = 'projectbank'

Write-Host "STARTING DATABASE"
docker run --name db --rm -e "POSTGRES_USER=$user" -e "POSTGRES_PASSWORD=$password" -e "POSTGRES_DB=$db" -p 5435:5434 -d postgres
Write-Host "DONE."

$connectionString = "Host=localhost;Port=5435;Database=$db;Username=$user;Password=$password"
dotnet user-secrets init --project ProjectBank.Server
dotnet user-secrets set "ConnectionStrings:ProjectBank" "$connectionString" --project ProjectBank.Server

Write-Host "STARTING APPLICATION"
dotnet run --project ProjectBank.Server
#docker stop db