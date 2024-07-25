#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Identity/CombatAnalysisIdentity/CombatAnalysisIdentity.csproj", "src/Identity/CombatAnalysisIdentity/"]
COPY ["src/CombatAnalysis.Identity/CombatAnalysis.Identity.csproj", "src/CombatAnalysis.Identity/"]
COPY ["src/CombatAnalysis.IdentityDAL/CombatAnalysis.IdentityDAL.csproj", "src/CombatAnalysis.IdentityDAL/"]
RUN dotnet restore "./src/Identity/CombatAnalysisIdentity/CombatAnalysisIdentity.csproj"
COPY . .
WORKDIR "/src/src/Identity/CombatAnalysisIdentity"
RUN dotnet build "./CombatAnalysisIdentity.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CombatAnalysisIdentity.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CombatAnalysisIdentity.dll"]