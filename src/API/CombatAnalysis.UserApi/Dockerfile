#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/API/CombatAnalysis.UserApi/CombatAnalysis.UserApi.csproj", "src/API/CombatAnalysis.UserApi/"]
COPY ["src/CombatAnalysis.CutomerBL/CombatAnalysis.CustomerBL.csproj", "src/CombatAnalysis.CutomerBL/"]
COPY ["src/CombatAnalysis.DAL/CombatAnalysis.DAL.csproj", "src/CombatAnalysis.DAL/"]
COPY ["src/CombatAnalysis.Identity/CombatAnalysis.Identity.csproj", "src/CombatAnalysis.Identity/"]
RUN dotnet restore "src/API/CombatAnalysis.UserApi/CombatAnalysis.UserApi.csproj"
COPY . .
WORKDIR "/src/src/API/CombatAnalysis.UserApi"
RUN dotnet build "CombatAnalysis.UserApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CombatAnalysis.UserApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CombatAnalysis.UserApi.dll"]