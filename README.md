# Veterinary Clinic Management System

A REST API for managing a veterinary clinic, built with ASP.NET Core following Clean Architecture principles.

## Overview

This project implements a veterinary clinic management system with separate layers for domain logic, application services, infrastructure, and API presentation.

## Architecture

- **Domain**: Contains entities, value objects, domain services, and repository interfaces.
- **Application**: Contains application services, DTOs, and interfaces.
- **Infrastructure**: Contains repository implementations and external concerns (e.g., database access).
- **API**: ASP.NET Core Web API with controllers and Swagger documentation.

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (planned for Infrastructure layer)
- Swagger/OpenAPI for API documentation
- Clean Architecture pattern

## Getting Started

### Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository:
   ```
   git clone <repository-url>
   cd VeterinaryClinic
   ```

2. Restore dependencies:
   ```
   dotnet restore
   ```

3. Run the application:
   ```
   cd src/VeterinaryClinic.API
   dotnet run
   ```

The API will be available at `https://localhost:5001` with Swagger UI at `https://localhost:5001/swagger`.

## API Endpoints

*(Endpoints to be implemented)*

- Owners: `/api/owners`
- Pets: `/api/pets`
- Appointments: `/api/appointments`

## Development

The project uses Clean Architecture to ensure separation of concerns and testability. Each layer has specific responsibilities:

- **Domain**: Core business logic and rules
- **Application**: Use cases and application logic
- **Infrastructure**: External dependencies and implementations
- **API**: HTTP interface and presentation

## Contributing

1. Follow Clean Architecture principles
2. Write tests for new features
3. Update documentation as needed

## License

This project is licensed under the MIT License.