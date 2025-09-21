FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CatalogManagementService.sln", "."]
COPY ["src/CatalogManagementService.Api/CatalogManagementService.Api.csproj", "src/CatalogManagementService.Api/"]
COPY ["src/CatalogManagementService.Infrastructure/CatalogManagementService.Infrastructure.csproj", "src/CatalogManagementService.Infrastructure/"]
COPY ["src/CatalogManagementService.Application/CatalogManagementService.Application.csproj", "src/CatalogManagementService.Application/"]
COPY ["src/CatalogManagementService.Domain/CatalogManagementService.Domain.csproj", "src/CatalogManagementService.Domain/"]
RUN dotnet restore "CatalogManagementService.sln"
COPY src/ ./src/
RUN dotnet build "CatalogManagementService.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/CatalogManagementService.Api/CatalogManagementService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5012
ENTRYPOINT ["dotnet", "CatalogManagementService.Api.dll"]