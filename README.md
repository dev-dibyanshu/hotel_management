# Hotel Management System

A microservices-based hotel management system built with ASP.NET Core Web API and C#.

## Architecture

This system follows a microservices architecture where each service is responsible for a specific domain of the hotel management system.

## Microservices

### ✅ IdentityService
**Status**: Implemented

User authentication and identity management service.

**Features:**
- User registration
- User login with JWT authentication
- Password hashing with BCrypt
- Role-based user management (Guest, Staff, Manager, Admin)

**Technology Stack:**
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- BCrypt password hashing

[📖 View IdentityService Documentation](./IdentityService/README.md)

### 🔜 Future Microservices

The following microservices are planned for future implementation:

- **GuestService** - Guest profile and preferences management
- **RoomService** - Room inventory and availability management
- **ReservationService** - Booking and reservation management
- **BillingService** - Billing, invoicing, and payment processing
- **ManagementService** - Hotel operations and reporting
- **API Gateway** - Centralized entry point for all services

## Project Structure

```
Hotel_management/
│
├── IdentityService/           # ✅ User authentication and identity
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── Models/
│   ├── DTOs/
│   ├── Data/
│   └── README.md
│
├── GuestService/              # 🔜 Coming soon
├── RoomService/               # 🔜 Coming soon
├── ReservationService/        # 🔜 Coming soon
├── BillingService/            # 🔜 Coming soon
├── ManagementService/         # 🔜 Coming soon
├── APIGateway/                # 🔜 Coming soon
│
└── README.md                  # This file
```

## Getting Started

### Prerequisites

- .NET 8 SDK or later
- SQL Server (Express, Developer, or Docker)
- Visual Studio 2022, VS Code, or Rider (optional)
- Git

### Quick Start

1. **Clone the repository**
   ```bash
   git clone https://github.com/dev-dibyanshu/hotel_management.git
   cd hotel_management
   ```

2. **Navigate to a service**
   ```bash
   cd IdentityService
   ```

3. **Follow the service-specific setup**
   - Each service has its own README with detailed setup instructions
   - See [IdentityService/README.md](./IdentityService/README.md) for the Identity Service setup

## Architecture Principles

### Service Design
- **Single Responsibility**: Each microservice handles one specific domain
- **Traditional Layered Architecture**: Simple and maintainable folder-based structure
- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: Loose coupling and testability

### Technology Choices
- **ASP.NET Core Web API**: RESTful services
- **Entity Framework Core**: ORM for database access
- **SQL Server**: Relational database
- **JWT**: Token-based authentication
- **Swagger/OpenAPI**: API documentation

### Communication
- **REST APIs**: Synchronous HTTP communication between services
- **JWT Tokens**: Authentication across services
- **Future**: Event-driven communication with message brokers (if needed)

## Development Workflow

### Branch Strategy
- `main` - Production-ready code
- `develop` - Integration branch
- `feature/*` - Feature development branches

### Commit Guidelines
- Use conventional commits format
- Write clear, descriptive commit messages
- Keep commits focused and atomic

## Current Status

| Service | Status | Version | Documentation |
|---------|--------|---------|---------------|
| IdentityService | ✅ Complete | 1.0.0 | [View](./IdentityService/README.md) |
| GuestService | 🔜 Planned | - | - |
| RoomService | 🔜 Planned | - | - |
| ReservationService | 🔜 Planned | - | - |
| BillingService | 🔜 Planned | - | - |
| ManagementService | 🔜 Planned | - | - |
| API Gateway | 🔜 Planned | - | - |

## Contributing

This is a learning/development project. Contributions and suggestions are welcome!

## License

This project is for educational and development purposes.

## Contact

**Developer**: Dibyanshu  
**Repository**: [https://github.com/dev-dibyanshu/hotel_management](https://github.com/dev-dibyanshu/hotel_management)

## Next Steps

1. ✅ Complete IdentityService implementation
2. Test IdentityService thoroughly
3. Design and implement GuestService
4. Design and implement RoomService
5. Design and implement ReservationService
6. Implement inter-service communication
7. Add API Gateway
8. Add monitoring and logging
9. Containerize with Docker
10. Set up CI/CD pipeline

---

**Last Updated**: October 28, 2026  
**Current Phase**: Phase 1 - IdentityService Complete
