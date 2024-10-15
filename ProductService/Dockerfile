# Use the Linux version of the .NET ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8081

# Use the Linux version of the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["ProductService/ProductService.csproj", "ProductService/"]
RUN dotnet restore "./ProductService/ProductService.csproj"

# Copy the rest of the application files
COPY . .
WORKDIR "/src/ProductService"

# Build the application
RUN dotnet build "./ProductService.csproj" -c ${BUILD_CONFIGURATION} -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "./ProductService.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish /p:UseAppHost=false

# Final stage: Set up the runtime environment
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductService.dll"]
