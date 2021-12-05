# Ensure certificate is present
Write-Host "CREATING DEV CERTIFICATE FOR OS $Env:OS"

if ($IsWindows) {
    dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p localhost
 } elseif ($IsMacOS -or $IsLinux) {
    dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p localhost
}

dotnet dev-certs https --trust
Write-Host "DONE."

# Run docker-compose
Write-Host
Write-Host "STARTING 'ProjectBank'"
docker-compose up -d
Write-Host "DONE."
docker-compose ps
Write-Host "------------------------------------------------------------------------------------"
Write-Host "TO STOP THE PROJECT WRITE: docker-compose stop"