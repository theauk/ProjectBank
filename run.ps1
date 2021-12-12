param([bool]$production, [bool]$dev)

function Main {
    if (!$dev) {
        Write-Host "STARTING VIA DOCKER-COMPOSE"
        DockerCompose
    } else {
        Write-Host "STARTING VIA DOTNET RUN WITH HOT RELOAD"
        DotnetRun
    }
}

function DockerCompose {
    Write-Host "PRODUCTION MODE = $production"
    Write-Host
    $file = "docker-compose.dev.yml"

    if ($production) {
        $file = "docker-compose.prod.yml"
    }

    docker-compose -f $file up -d --build
    Write-Host "DONE."
    Write-Host

    docker-compose -f $file ps
    Write-Host
    Write-Host "------------------------------------------------------------------------------------"
    Write-Host "TO STOP THE PROJECT WRITE:"
    Write-Host "docker-compose -f $file stop"
}

function DotnetRun {
    $Env:ASPNETCORE_ENVIRONMENT = "Development"

    $user = "postgres"
    $password = New-Guid
    $db = "projectbank"

    Write-Host "STARTING DATABASE"
    docker run --name db --rm -d -p 5431:5432 -e "POSTGRES_USER=$user" -e "POSTGRES_PASSWORD=$password" -e "POSTGRES_DB=$db" postgres:latest
    Write-Host "DONE."

    Write-Host "SETTING DOTNET SECRETS"
    $connectionString = "Host=localhost;Port=5431;Database=$db;Username=$user;Password=$password"
    dotnet user-secrets init --project ProjectBank.Server
    dotnet user-secrets set "ConnectionStrings:ProjectBank" "$connectionString" --project ProjectBank.Server
    Write-Host "DONE."

    Write-Host 
    Write-Host "TO STOP THE DATA BASE WRITE:"
    Write-Host "docker stop db"
    Write-Host
    Start-Sleep -Seconds 5

    Write-Host "STARTING APPLICATION"
    dotnet watch run --project .\ProjectBank.Server\
}

Main