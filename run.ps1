param([bool]$dev)

function Main {
    if ($dev) {
        CreateCertificate
    }

    DockerCompose
}

function DockerCompose {
    Write-Host "STARTING VIA DOCKER-COMPOSE"

    # Run docker-compose
    Write-Host
    Write-Host "STARTING 'ProjectBank'"
    docker-compose up -d
    Write-Host "DONE."
    docker-compose ps
    Write-Host "------------------------------------------------------------------------------------"
    Write-Host "TO STOP THE PROJECT WRITE: docker-compose stop"
}

function CreateCertificate {
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
}

Main