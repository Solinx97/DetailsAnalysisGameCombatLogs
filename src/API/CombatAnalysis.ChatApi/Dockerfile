FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/CombatAnalysis.ChatApi/CombatAnalysis.ChatApi.csproj", "src/API/CombatAnalysis.ChatApi/"]
COPY ["src/CombatAnalysis.ChatBL/CombatAnalysis.ChatBL.csproj", "src/CombatAnalysis.ChatBL/"]
COPY ["src/CombatAnalysis.ChatDAL/CombatAnalysis.ChatDAL.csproj", "src/CombatAnalysis.ChatDAL/"]
COPY ["src/CombatAnalysis.CutomerBL/CombatAnalysis.CustomerBL.csproj", "src/CombatAnalysis.CutomerBL/"]
COPY ["src/CombatAnalysis.CustomerDAL/CombatAnalysis.CustomerDAL.csproj", "src/CombatAnalysis.CustomerDAL/"]
RUN dotnet restore "./src/API/CombatAnalysis.ChatApi/CombatAnalysis.ChatApi.csproj"
COPY . .
WORKDIR "/src/src/API/CombatAnalysis.ChatApi"
RUN dotnet build "./CombatAnalysis.ChatApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CombatAnalysis.ChatApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CombatAnalysis.ChatApi.dll"]