# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY ../ /source
WORKDIR /source/ProjectBank.Server

RUN dotnet restore

RUN dotnet publish --configuration Release --output /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "ProjectBank.Server.dll"]