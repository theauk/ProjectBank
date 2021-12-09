param([bool]$dev)

function Main {
    if ($dev) {
        OnlyDatabase
    } else {
        DockerCompose
    }
}

function DockerCompose {
    # Ensure certificate is present
    Write-Host "STARTING VIA DOCKER-COMPOSE"
    Write-Host
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

    # Run docker-compose
    Write-Host
    Write-Host "STARTING 'ProjectBank'"
    docker-compose up -d
    Write-Host "DONE."
    docker-compose ps
    Write-Host "------------------------------------------------------------------------------------"
    Write-Host "TO STOP THE PROJECT WRITE: docker-compose stop"
}

function OnlyDatabase {
    Write-Host "STARTING VIA PROJECT"
    Write-Host

    $user = "postgres"
    $password = "postgress"
    $db = "projectbank"


    Write-Host "STARTING DATABASE"
    docker run --name db --rm -p 5435:5434 -e "POSTGRES_USER=$user" -e "POSTGRES_PASSWORD=$password" -e "POSTGRES_DB=$db" -d postgres
    Write-Host "DONE."

    $connectionString = "Host=localhost;Port=5432;Database=$db;Username=$user;Password=$password;"
    dotnet user-secrets init --project ProjectBank.Server
    dotnet user-secrets set "ConnectionStrings:ProjectBank" "$connectionString" --project ProjectBank.Server

    Write-Host "STARTING APPLICATION"
    dotnet run --project ProjectBank.Server
    docker stop db
}

Main