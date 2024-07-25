#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/CombatAnalysis.CombatParserAPI/CombatAnalysis.CombatParserAPI.csproj", "src/API/CombatAnalysis.CombatParserAPI/"]
COPY ["src/CombatAnalysis.BL/CombatAnalysis.BL.csproj", "src/CombatAnalysis.BL/"]
COPY ["src/CombatAnalysis.DAL/CombatAnalysis.DAL.csproj", "src/CombatAnalysis.DAL/"]
COPY ["src/CombatAnalysis.CombatParser/CombatAnalysis.CombatParser.csproj", "src/CombatAnalysis.CombatParser/"]
RUN dotnet restore "./src/API/CombatAnalysis.CombatParserAPI/CombatAnalysis.CombatParserAPI.csproj"
COPY . .
WORKDIR "/src/src/API/CombatAnalysis.CombatParserAPI"
RUN dotnet build "./CombatAnalysis.CombatParserAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CombatAnalysis.CombatParserAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CombatAnalysis.CombatParserAPI.dll"]