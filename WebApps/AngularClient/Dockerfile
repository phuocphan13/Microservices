FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# ================== Angular Build Stage (Clean & Consistent) ==================
FROM node:20-alpine AS node-build
WORKDIR /src/WebApps/AngularClient/ClientApp

# Copy package files
COPY WebApps/AngularClient/ClientApp/package*.json ./

# Force consistent Angular 17 versions + clean install
RUN npm install -g npm@latest && \
    npm ci --legacy-peer-deps --force || \
    npm install --legacy-peer-deps --force

COPY WebApps/AngularClient/ClientApp/ ./

# Build Angular
RUN npm run build -- --configuration production

# ================== .NET Publish Stage ==================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
ARG BUILD_CONFIGURATION=Release

# Install Node.js
RUN apt-get update && apt-get install -y curl \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src

# Restore .NET
COPY ["Common/Core.Common/Core.Common.csproj", "Common/Core.Common/"]
COPY ["Common/Platform/Platform.csproj", "Common/Platform/"]
COPY ["Common/ApiClient/ApiClient.csproj", "Common/ApiClient/"]
COPY ["WebApps/AngularClient/AngularClient.csproj", "WebApps/AngularClient/"]
RUN dotnet restore "WebApps/AngularClient/AngularClient.csproj"

COPY . .

# Copy pre-built Angular output
COPY --from=node-build /src/WebApps/AngularClient/ClientApp/dist ./WebApps/AngularClient/wwwroot

WORKDIR "/src/WebApps/AngularClient"

# Publish .NET
RUN dotnet publish "AngularClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ================== Final Stage ==================
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AngularClient.dll"]