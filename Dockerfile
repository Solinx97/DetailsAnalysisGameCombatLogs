#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /DetailsAnalysisGameCombatLogs
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /DetailsAnalysisGameCombatLogs
COPY ["src/CombatAnalysis.DAL/CombatAnalysis.DAL.csproj", "CombatAnalysis.DAL/"]
RUN dotnet build "CombatAnalysis.DAL/CombatAnalysis.ChatApi.csproj" -c Release -o /CombatAnalysis.ChatApi/build

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /DetailsAnalysisGameCombatLogs
COPY ["src/CombatAnalysis.BL/CombatAnalysis.BL.csproj", "CombatAnalysis.BL/"]
RUN dotnet build "CombatAnalysis.BL/CombatAnalysis.ChatApi.csproj" -c Release -o /CombatAnalysis.ChatApi/build

# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# WORKDIR /DetailsAnalysisGameCombatLogs
# COPY ["CombatAnalysis.ChatApi.csproj", "API/CombatAnalysis.ChatApi/"]
# COPY ["CombatAnalysis.BL/CombatAnalysis.BL.csproj", "CombatAnalysis.BL/"]
# COPY ["CombatAnalysis.DAL/CombatAnalysis.DAL.csproj", "CombatAnalysis.DAL/"]
# RUN dotnet restore "CombatAnalysis.ChatApi.csproj"
# COPY . .
# WORKDIR /CombatAnalysis.ChatApi
# RUN dotnet build "CombatAnalysis.ChatApi.csproj" -c Release -o /CombatAnalysis.ChatApi/build

# FROM build AS publish
# RUN dotnet publish "CombatAnalysis.ChatApi.csproj" -c Release -o /CombatAnalysis.ChatApi/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app	
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "CombatAnalysis.ChatApi.dll"]