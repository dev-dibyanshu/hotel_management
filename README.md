# Hotel Management System - Identity/Authentication Microservice

## Overview

This is the Identity and Authentication microservice for the Hotel Management System. It provides user management and JWT-based authentication capabilities.

## Technology Stack

- **.NET 8**
- **ASP.NET Core Web API**
- **C#**
- **Entity Framework Core 8**
- **SQL Server**
- **ASP.NET Core Identity**
- **JWT Bearer Authentication**
- **Swagger/OpenAPI**
- **Repository Pattern**
- **Layered Architecture**

## Project Structure

```
IdentityService/
│
├── IdentityService.API/
│   ├── Controllers/
│   │   ├── AuthController.cs           # Login endpoint
│   │   └── UsersController.cs          # User management endpoints
│   ├── Middleware/
│   │   └── GlobalExceptionHandlerMiddleware.cs
│   ├── Program.cs                       # Application configuration and startup
│   └── appsettings.json                # Configuration including connection string and JWT settings
│
├── IdentityService.Application/
│   ├── DTOs/
│   │   ├── LoginRequestDto.cs
│   │   ├── LoginResponseDto.cs
│   │   ├── CreateUserDto.cs
│   │   ├── UpdateUserDto.cs
│   │   └── UserResponseDto.cs
│   ├── Interfaces/
│   │   ├── IIdentityService.cs         # Business logic interface
│   │   └── IJwtTokenService.cs         # JWT generation interface
│   └── Services/
│       └── IdentityService.cs          # Business logic implementation
│
├── IdentityService.Domain/
│   ├── Entities/
│   │   └── ApplicationUser.cs          # User entity extending IdentityUser<int>
│   └── Interfaces/
│       └── Repositories/
│           └── IUserRepository.cs       # Repository interface
│
└── IdentityService.Infrastructure/
    ├── Data/
    │   └── AuthDbContext.cs            # EF Core DbContext
    ├── Repositories/
    │   └── UserRepository.cs           # Repository implementation
    └── Security/
        └── JwtTokenService.cs          # JWT token generation
```

## Architecture

The solution follows a clean layered architecture:

```
API Layer
    ↓
Application Layer (Business Logic)
    ↓
Domain Layer (Entities & Interfaces)
```

Infrastructure layer implements abstractions from Application and Domain layers.

### Dependency Flow:
- **IdentityService.API** → Application → Infrastructure
- **IdentityService.Application** → Domain
- **IdentityService.Infrastructure** → Domain + Application
- **IdentityService.Domain** → No dependencies (pure domain)

## Database

- **Database Name**: `HotelAuthDb`
- **Provider**: SQL Server
- **Connection String**: Configured in `appsettings.json`

### Tables Created by Identity:
- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- AspNetUserClaims
- AspNetUserLogins
- AspNetUserTokens
- AspNetRoleClaims

## User Roles

The system supports three roles:
1. **Owner** - Full access to all operations
2. **Manager** - Can manage users (create, read, update)
3. **Receptionist** - Limited access (needs authentication)

## API Endpoints

### Authentication

#### POST `/api/auth/login`
- **Description**: Login with username and password
- **Access**: Public (AllowAnonymous)
- **Request Body**:
```json
{
  "username": "admin",
  "password": "Admin@123"
}
```
- **Success Response** (200):
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "role": "Owner"
}
```
- **Error Response** (401): Invalid username or password

### User Management

#### GET `/api/users`
- **Description**: Get all users
- **Access**: Owner, Manager
- **Response** (200):
```json
[
  {
    "id": 1,
    "username": "admin",
    "email": "owner@hotel.com",
    "role": "Owner"
  }
]
```

#### GET `/api/users/{id}`
- **Description**: Get user by ID
- **Access**: Owner, Manager
- **Response** (200):
```json
{
  "id": 1,
  "username": "admin",
  "email": "owner@hotel.com",
  "role": "Owner"
}
```
- **Error Response** (404): User not found

#### POST `/api/users`
- **Description**: Create a new user
- **Access**: Owner, Manager
- **Request Body**:
```json
{
  "username": "manager1",
  "email": "manager1@hotel.com",
  "password": "Manager@123",
  "role": "Manager"
}
```
- **Success Response** (201):
```json
{
  "id": 2,
  "username": "manager1",
  "email": "manager1@hotel.com",
  "role": "Manager"
}
```
- **Error Response** (409): Username or email already exists

#### PUT `/api/users/{id}`
- **Description**: Update user details
- **Access**: Owner, Manager
- **Request Body**:
```json
{
  "email": "newemail@hotel.com",
  "role": "Receptionist"
}
```
- **Success Response** (200):
```json
{
  "id": 2,
  "username": "manager1",
  "email": "newemail@hotel.com",
  "role": "Receptionist"
}
```
- **Error Response** (404): User not found

#### DELETE `/api/users/{id}`
- **Description**: Delete a user
- **Access**: Owner only
- **Success Response** (200):
```json
{
  "message": "User deleted successfully"
}
```
- **Error Response** (404): User not found

## JWT Authentication

### JWT Configuration (appsettings.json)
```json
{
  "JwtSettings": {
    "SecretKey": "YourVerySecureSecretKeyForJWTTokenGeneration12345",
    "Issuer": "HotelManagementSystem",
    "Audience": "HotelManagementSystemUsers",
    "ExpirationHours": "24"
  }
}
```

### JWT Claims
- **Sub**: Username
- **Jti**: Unique token identifier
- **NameIdentifier**: User ID
- **Name**: Username
- **Role**: User role

### Token Format
```
Authorization: Bearer <token>
```

## Seeded Data

The application automatically seeds:
1. **Roles**: Owner, Manager, Receptionist
2. **Default Owner User**:
   - Username: `admin`
   - Email: `owner@hotel.com`
   - Password: `Admin@123`
   - Role: `Owner`

These values can be configured in `appsettings.json`:
```json
{
  "SeedUser": {
    "Username": "admin",
    "Email": "owner@hotel.com",
    "Password": "Admin@123"
  }
}
```

## Setup and Running

### Prerequisites
1. .NET 8 SDK
2. SQL Server (local or remote)
3. EF Core CLI tools

### Install EF Core Tools (if not already installed)
```bash
dotnet tool install --global dotnet-ef
```

### Steps to Run

1. **Update Connection String**
   
   Edit `IdentityService.API/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=HotelAuthDb;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

2. **Build the Solution**
   ```bash
   cd IdentityService
   dotnet build
   ```

3. **Apply Database Migration**
   
   The application will automatically apply migrations on startup. Alternatively, you can manually apply them:
   ```bash
   cd IdentityService.API
   dotnet ef database update --project ../IdentityService.Infrastructure
   ```

4. **Run the Application**
   ```bash
   cd IdentityService.API
   dotnet run
   ```

   The API will start on:
   - HTTP: `http://localhost:5178`
   - HTTPS: `https://localhost:7178`
   - Swagger UI: `http://localhost:5178/swagger`

## Testing the Service

### Using Swagger UI

1. Navigate to `http://localhost:5178/swagger`
2. Test the `/api/auth/login` endpoint:
   - Click on `POST /api/auth/login`
   - Click "Try it out"
   - Enter credentials:
     ```json
     {
       "username": "admin",
       "password": "Admin@123"
     }
     ```
   - Click "Execute"
   - Copy the `token` from the response

3. Authorize Swagger with JWT:
   - Unfortunately, the current Swagger configuration doesn't support Bearer token UI due to package compatibility issues
   - To test protected endpoints, use Postman or curl (see below)

### Using Postman or curl

#### 1. Login
```bash
curl -X POST http://localhost:5178/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "Admin@123"
  }'
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "role": "Owner"
}
```

#### 2. Get All Users (requires token)
```bash
curl -X GET http://localhost:5178/api/users \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

#### 3. Create a New User
```bash
curl -X POST http://localhost:5178/api/users \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "username": "manager1",
    "email": "manager1@hotel.com",
    "password": "Manager@123",
    "role": "Manager"
  }'
```

#### 4. Update User
```bash
curl -X PUT http://localhost:5178/api/users/2 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "email": "newemail@hotel.com",
    "role": "Receptionist"
  }'
```

#### 5. Delete User (Owner only)
```bash
curl -X DELETE http://localhost:5178/api/users/2 \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## Validation Rules

### Password Requirements
- Minimum 6 characters
- Must contain at least one digit
- Must contain at least one lowercase letter
- Must contain at least one uppercase letter
- Non-alphanumeric characters are optional

### Email
- Must be a valid email format
- Must be unique

### Username
- Required
- Must be unique

### Role
- Must be one of: Owner, Manager, Receptionist

## Security Features

1. **Password Hashing**: ASP.NET Core Identity handles password hashing automatically
2. **JWT Authentication**: Stateless token-based authentication
3. **Role-Based Authorization**: Fine-grained access control
4. **Global Exception Handling**: Prevents sensitive error details from being exposed
5. **HTTPS Support**: Configured for production use

## Error Handling

The API uses standard HTTP status codes:
- `200 OK`: Successful request
- `201 Created`: Resource successfully created
- `400 Bad Request`: Invalid input data
- `401 Unauthorized`: Invalid credentials or missing token
- `403 Forbidden`: Insufficient permissions
- `404 Not Found`: Resource not found
- `409 Conflict`: Duplicate username or email
- `500 Internal Server Error`: Server error (details logged, not exposed to client)

## Important Notes

1. **Passwords are never returned** in API responses
2. **JWT tokens expire** after 24 hours (configurable)
3. **Role changes** require a new login to get an updated token
4. **Database migrations** are automatically applied on application startup
5. **Connection string** must be updated for your SQL Server instance
6. **JWT SecretKey** should be changed in production and stored securely (e.g., Azure Key Vault, AWS Secrets Manager)

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in `appsettings.json`
- Ensure SQL Server is configured to accept TCP connections

### Login Fails with Seeded User
- Verify the database was seeded successfully (check logs on startup)
- Try manually checking the database for the admin user
- Ensure password meets complexity requirements

### Token Not Working
- Verify the token hasn't expired (24 hours default)
- Check that the `Authorization` header format is correct: `Bearer <token>`
- Ensure the JWT secret in configuration matches between token generation and validation

### EF Core Migration Issues
```bash
# Remove and recreate migration
cd IdentityService.API
dotnet ef migrations remove --project ../IdentityService.Infrastructure
dotnet ef migrations add InitialCreate --project ../IdentityService.Infrastructure
dotnet ef database update --project ../IdentityService.Infrastructure
```

## Future Enhancements (NOT in current scope)

The following features are intentionally NOT implemented in this version:
- Refresh tokens
- Password reset via email
- Email verification
- Two-factor authentication (2FA)
- OAuth/External identity providers
- User profile pictures
- Audit logging
- Rate limiting
- Redis caching
- Message queues (Kafka/RabbitMQ)
- Docker containerization
- Kubernetes deployment

## License

This is an evaluation/demonstration project.
