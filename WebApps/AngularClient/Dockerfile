FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM node:18-alpine AS node-build
WORKDIR /app/WebApps/AngularClient/ClientApp
# Copy only package files first for faster installs
COPY WebApps/AngularClient/ClientApp/package*.json ./
RUN npm install -g @angular/cli && npm install --legacy-peer-deps
COPY WebApps/AngularClient/ClientApp/ ./
RUN npm run build -- --configuration production || true

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
ARG BUILD_CONFIGURATION=Release
# Install Node.js in the SDK image so MSBuild targets that call `npm` can run
RUN apt-get update \
	&& apt-get install -y curl gnupg \
	&& curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
	&& apt-get install -y nodejs \
	&& rm -rf /var/lib/apt/lists/*
WORKDIR /src
COPY ["Common/Core.Common/Core.Common.csproj", "Common/Core.Common/"]
COPY ["Common/Platform/Platform.csproj", "Common/Platform/"]
COPY ["Common/ApiClient/ApiClient.csproj", "Common/ApiClient/"]
COPY ["WebApps/AngularClient/AngularClient.csproj", "WebApps/AngularClient/"]
RUN dotnet restore "WebApps/AngularClient/AngularClient.csproj"
COPY . .
WORKDIR "/src/WebApps/AngularClient"
RUN dotnet build "AngularClient.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "AngularClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AngularClient.dll"]
