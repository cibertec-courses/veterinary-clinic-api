# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY src/VeterinaryClinic.Domain/*.csproj ./VeterinaryClinic.Domain/
COPY src/VeterinaryClinic.Application/*.csproj ./VeterinaryClinic.Application/
COPY src/VeterinaryClinic.Infrastructure/*.csproj ./VeterinaryClinic.Infrastructure/
COPY src/VeterinaryClinic.API/*.csproj ./VeterinaryClinic.API/

RUN dotnet restore ./VeterinaryClinic.API/VeterinaryClinic.API.csproj

# Copy source code and build
COPY src/ .
RUN dotnet publish ./VeterinaryClinic.API/VeterinaryClinic.API.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "VeterinaryClinic.API.dll"]
