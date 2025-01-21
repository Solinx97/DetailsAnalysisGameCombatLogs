FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CombatAnalysis.WebApp/CombatAnalysis.WebApp.csproj", "src/CombatAnalysis.WebApp/"]
RUN dotnet restore "./src/CombatAnalysis.WebApp/CombatAnalysis.WebApp.csproj"
COPY . .
WORKDIR "/src/src/CombatAnalysis.WebApp"
RUN dotnet build "./CombatAnalysis.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN apt-get update && apt-get upgrade -y && \
    apt-get install -y nodejs npm 
    
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CombatAnalysis.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CombatAnalysis.WebApp.dll"]