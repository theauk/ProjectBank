param([bool]$production)

Write-Host "STARTING PROJECTBANK VIA DOCKER-COMPOSE"
Write-Host "PRODUCTION MODE = $production"
Write-Host
$file = "docker-compose.dev.yml"

if ($production) {
    $file = "docker-compose.prod.yml"
}

docker-compose -f $file up -d
Write-Host "DONE."
Write-Host "------------------------------------------------------------------------------------"
Write-Host "TO STOP THE PROJECT WRITE:"

if ($production) {
    Write-Host "docker-compose -f docker-compose.prod.yml stop"
} else {
    Write-Host "docker-compose -f docker-compose.dev.yml stop"
}