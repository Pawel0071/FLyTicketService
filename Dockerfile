# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FLyTicketService.csproj", "."]
RUN dotnet restore "./FLyTicketService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./FLyTicketService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FLyTicketService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables for SQL Server connection
ENV SQL_SERVER=sql1
ENV SQL_PORT=1433
ENV CONNECTION_STRING="Server=${SQL_SERVER},${SQL_PORT};Database=YourDatabaseName;User Id=sa;Password=2019Venza;"

# Ensure the app listens on HTTPS and the required port
ENTRYPOINT ["dotnet", "FLyTicketService.dll"]