FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/* \
    
FROM node:alpine
WORKDIR /app/WebApps/AngularClient/ClientApp
COPY . /app/WebApps/AngularClient/ClientApp

RUN npm install -g @angular/cli

RUN npm install -f

WORKDIR /src
COPY ["Common/Core.Common/Core.Common.csproj", "Common/Core.Common/"]
COPY ["Common/Platform/Platform.csproj", "Common/Platform/"]
COPY ["Common/ApiClient/ApiClient.csproj", "Common/ApiClient/"]
COPY ["WebApps/AngularClient/AngularClient.csproj", "WebApps/AngularClient/"]
RUN dotnet restore "WebApps/AngularClient/AngularClient.csproj"
COPY . .
WORKDIR "/src/WebApps/AngularClient"
RUN dotnet build "AngularClient.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AngularClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AngularClient.dll"]
