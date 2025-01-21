FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/CombatAnalysis.UserApi/CombatAnalysis.UserApi.csproj", "src/API/CombatAnalysis.UserApi/"]
COPY ["src/CombatAnalysis.CutomerBL/CombatAnalysis.CustomerBL.csproj", "src/CombatAnalysis.CutomerBL/"]
COPY ["src/CombatAnalysis.CustomerDAL/CombatAnalysis.CustomerDAL.csproj", "src/CombatAnalysis.CustomerDAL/"]
RUN dotnet restore "./src/API/CombatAnalysis.UserApi/CombatAnalysis.UserApi.csproj"
COPY . .
WORKDIR "/src/src/API/CombatAnalysis.UserApi"
RUN dotnet build "./CombatAnalysis.UserApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CombatAnalysis.UserApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CombatAnalysis.UserApi.dll"]