#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/CombatAnalysis.CommunicationAPI/CombatAnalysis.CommunicationAPI.csproj", "src/API/CombatAnalysis.CommunicationAPI/"]
COPY ["src/CombatAnalysis.CommunicationBL/CombatAnalysis.CommunicationBL.csproj", "src/CombatAnalysis.CommunicationBL/"]
COPY ["src/CombatAnalysis.CommunicationDAL/CombatAnalysis.CommunicationDAL.csproj", "src/CombatAnalysis.CommunicationDAL/"]
COPY ["src/CombatAnalysis.CutomerBL/CombatAnalysis.CustomerBL.csproj", "src/CombatAnalysis.CutomerBL/"]
COPY ["src/CombatAnalysis.CustomerDAL/CombatAnalysis.CustomerDAL.csproj", "src/CombatAnalysis.CustomerDAL/"]
RUN dotnet restore "./src/API/CombatAnalysis.CommunicationAPI/CombatAnalysis.CommunicationAPI.csproj"
COPY . .
WORKDIR "/src/src/API/CombatAnalysis.CommunicationAPI"
RUN dotnet build "./CombatAnalysis.CommunicationAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CombatAnalysis.CommunicationAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CombatAnalysis.CommunicationAPI.dll"]