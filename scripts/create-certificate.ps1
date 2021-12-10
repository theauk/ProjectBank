# Use this script to create a developer certificate in order to be able to run the program.

Write-Host "CREATING DEV CERTIFICATE FOR OS $Env:OS"
dotnet dev-certs https --clean

if ($IsWindows) {
    Write-Host "WINDOWS DETECTED"
    dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p localhost
}
elseif ($IsMacOS -or $IsLinux) {
    Write-Host "MAC OR LINUX DETECTED"
    dotnet dev-certs https -ep $env:HOME/.aspnet/https/aspnetapp.pfx -p localhost
}

dotnet dev-certs https --trust
Write-Host "DONE."