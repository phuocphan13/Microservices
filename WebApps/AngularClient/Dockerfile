FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# ================== Angular Build Stage ==================
FROM node:20-alpine AS node-build
WORKDIR /src/WebApps/AngularClient/ClientApp

# Copy only package files first
COPY WebApps/AngularClient/ClientApp/package*.json ./

# Clean install
RUN npm ci --legacy-peer-deps

# Copy source and build
COPY WebApps/AngularClient/ClientApp/ ./
RUN npm run build -- --configuration production

# ================== .NET Publish Stage ==================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
ARG BUILD_CONFIGURATION=Release

# Install Node.js (use the same version as node-build)
RUN apt-get update && apt-get install -y curl \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs \
    && rm -rf /var/lib/apt/lists/* \
    && npm install -g npm@latest   # optional: update npm

WORKDIR /src

# Copy csproj and restore
COPY ["Common/Core.Common/Core.Common.csproj", "Common/Core.Common/"]
COPY ["Common/Platform/Platform.csproj", "Common/Platform/"]
COPY ["Common/ApiClient/ApiClient.csproj", "Common/ApiClient/"]
COPY ["WebApps/AngularClient/AngularClient.csproj", "WebApps/AngularClient/"]

RUN dotnet restore "WebApps/AngularClient/AngularClient.csproj"

COPY . .

# === Copy pre-built Angular files ===
COPY --from=node-build /src/WebApps/AngularClient/ClientApp/dist ./WebApps/AngularClient/wwwroot

WORKDIR "/src/WebApps/AngularClient"

# Publish .NET (npm should no longer be needed, but we have it installed as safety)
RUN dotnet publish "AngularClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ================== Final Stage ==================
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AngularClient.dll"]