param([bool]$production)

Write-Host "STARTING PROJECTBANK VIA DOCKER-COMPOSE"
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