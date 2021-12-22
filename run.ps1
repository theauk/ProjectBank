# Run with Docker-compose in production mode: -docker $true
# Run with Docker-compose in development mode: -docker $true -dev $true

# Run with Dotnet run: <No arguments>
# Run with Dotnet run with hot reload: -hotreload $true

param([bool]$docker, [bool]$dev, [bool]$hotreload)

function Main {
    if ($docker) {
        Write-Host "STARTING VIA DOCKER-COMPOSE UP"
        RunWithDockerCompose
    } else {
        Write-Host "STARTING VIA DOTNET RUN"
        RunWithDotnetRun
    }
}

function RunWithDockerCompose {
    Write-Host "PRODUCTION MODE =" (-not $dev)
    Write-Host
    $file = "docker-compose.prod.yml"
    
    if ($dev) {
        $file = "docker-compose.dev.yml"
    }

    docker-compose -f $file up -d --build
    Write-Host "DONE."
    Write-Host

    docker-compose -f $file ps
    Write-Host
    Write-Host "------------------------------------------------------------------------------------"
    Write-Host "TO STOP THE PROJECT WRITE:"
    Write-Host "docker-compose -f $file stop"
    Write-Host
}

function RunWithDotnetRun {
    if ($dev) {
        Write-Host "HOT RELOAD = $hotreload"
    }
    Write-Host
    
    $project = "ProjectBank.Server"
    $user = "postgres"
    $password = New-Guid
    $db = "projectbank"

    Write-Host "STARTING DATABASE"
    if ($dev) {
        # Delete container when stopped
        docker run --name db --rm -d -p 5431:5432 -e "POSTGRES_USER=$user" -e "POSTGRES_PASSWORD=$password" -e "POSTGRES_DB=$db" postgres:latest
    } else {
        docker run --name db -d -p 5431:5432 -e "POSTGRES_USER=$user" -e "POSTGRES_PASSWORD=$password" -e "POSTGRES_DB=$db" postgres:latest
    }
    
    Write-Host "DONE."
    Write-Host 

    Write-Host "CONFIGURING CONNECTION STRING"
    $connectionString = "Host=localhost;Port=5431;Database=$db;Username=$user;Password=$password"
    dotnet user-secrets init --project $project
    dotnet user-secrets set "ConnectionStrings:ProjectBank" "$connectionString" --project ProjectBank.Server
    Write-Host "DONE."

    Write-Host 
    Write-Host "TO STOP THE DATA BASE WRITE:"
    Write-Host "docker stop db"
    Write-Host
    Start-Sleep -Seconds 5

    Write-Host "STARTING APPLICATION"
    
    if ($dev) {
        $Env:ASPNETCORE_ENVIRONMENT = "Development"
        dotnet watch run --project $project
    } else {
        dotnet run --project $project
    }
    
    Write-Host
    Write-Host "STOPPING AND DELETING DATABASE"
    docker stop db
    Write-Host "DONE."
}

Main