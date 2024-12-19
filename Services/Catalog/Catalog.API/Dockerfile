FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Catalog/Catalog.API/Catalog.API.csproj", "Services/Catalog/Catalog.API/"]
COPY ["Common/ApiClient/ApiClient.csproj", "Common/ApiClient/"]
COPY ["Common/Core.Common/Core.Common.csproj", "Common/Core.Common/"]
COPY ["Common/Platform/Platform.csproj", "Common/Platform/"]
COPY ["Common/IntegrationFramework/IntegrationFramework.csproj", "Common/IntegrationFramework/"]
COPY ["Common/Worker/Worker.csproj", "Common/Worker/"]
COPY ["EventBus.Messages/EventBus.Messages.csproj", "EventBus.Messages/"]
COPY ["Identity/IdentityServer.Common/IdentityServer.Common.csproj", "Identity/IdentityServer.Common/"]
COPY ["Common/OpenTelemetryFramework/OpenTelemetryFramework.csproj", "Common/OpenTelemetryFramework/"]
RUN dotnet restore "Services/Catalog/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
