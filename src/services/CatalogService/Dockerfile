FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CatalogService.sln", "."]
COPY ["src/CatalogService.Api/CatalogService.Api.csproj", "src/CatalogService.Api/"]
COPY ["src/CatalogService.Infrastructure/CatalogService.Infrastructure.csproj", "src/CatalogService.Infrastructure/"]
COPY ["src/CatalogService.Application/CatalogService.Application.csproj", "src/CatalogService.Application/"]
COPY ["src/CatalogService.Domain/CatalogService.Domain.csproj", "src/CatalogService.Domain/"]
RUN dotnet restore "CatalogService.sln"
COPY src/ ./src/
RUN dotnet build "CatalogService.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/CatalogService.Api/CatalogService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5011
ENTRYPOINT ["dotnet", "CatalogService.Api.dll"]