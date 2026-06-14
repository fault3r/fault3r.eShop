FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UserService.sln", "."]
COPY ["src/UserService.Api/UserService.Api.csproj", "src/UserService.Api/"]
COPY ["src/UserService.Infrastructure/UserService.Infrastructure.csproj", "src/UserService.Infrastructure/"]
COPY ["src/UserService.Application/UserService.Application.csproj", "src/UserService.Application/"]
COPY ["src/UserService.Domain/UserService.Domain.csproj", "src/UserService.Domain/"]
RUN dotnet restore "UserService.sln"
COPY src/ ./src/
RUN dotnet build "UserService.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/UserService.Api/UserService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5012
ENTRYPOINT ["dotnet", "UserService.Api.dll"]