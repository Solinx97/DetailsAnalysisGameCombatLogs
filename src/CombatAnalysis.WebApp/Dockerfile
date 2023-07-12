#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/CombatAnalysis.WebApp/CombatAnalysis.WebApp.csproj", "src/CombatAnalysis.WebApp/"]
RUN dotnet restore "src/CombatAnalysis.WebApp/CombatAnalysis.WebApp.csproj"
COPY . .
WORKDIR "/src/src/CombatAnalysis.WebApp"
RUN dotnet build "CombatAnalysis.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CombatAnalysis.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CombatAnalysis.WebApp.dll"]