#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/API/CombatAnalysis.ChatApi/CombatAnalysis.ChatApi.csproj", "src/API/CombatAnalysis.ChatApi/"]
COPY ["src/CombatAnalysis.BL/CombatAnalysis.BL.csproj", "src/CombatAnalysis.BL/"]
COPY ["src/CombatAnalysis.DAL/CombatAnalysis.DAL.csproj", "src/CombatAnalysis.DAL/"]
RUN dotnet restore "src/API/CombatAnalysis.ChatApi/CombatAnalysis.ChatApi.csproj"
COPY . .
WORKDIR "/src/src/API/CombatAnalysis.ChatApi"
RUN dotnet build "CombatAnalysis.ChatApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CombatAnalysis.ChatApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CombatAnalysis.ChatApi.dll"]