#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs
WORKDIR /src
COPY ["src/CombatAnalysis.WebApp/CombatAnalysis.WebApp.csproj", "src/CombatAnalysis.WebApp/"]
COPY ["src/CombatAnalysis.BL/CombatAnalysis.BL.csproj", "src/CombatAnalysis.BL/"]
COPY ["src/CombatAnalysis.DAL/CombatAnalysis.DAL.csproj", "src/CombatAnalysis.DAL/"]
COPY ["src/CombatAnalysis.Identity/CombatAnalysis.Identity.csproj", "src/CombatAnalysis.Identity/"]
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