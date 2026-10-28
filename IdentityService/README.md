# Identity Service - Hotel Management System

The **IdentityService** is a microservice responsible for user authentication and identity management in the Hotel Management System. It provides user registration, login, and JWT-based authentication.

## Technology Stack

- **Framework**: ASP.NET Core Web API (.NET 8)
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT Bearer Authentication
- **Password Hashing**: BCrypt
- **API Documentation**: Swagger/OpenAPI

## Architecture

This service follows a traditional layered architecture with clear separation of concerns:

```
IdentityService/
│
├── Controllers/         # API endpoints and request handling
├── DTOs/               # Data Transfer Objects for API requests/responses
├── Services/           # Business logic and application services
├── Repositories/       # Data access layer
├── Data/              # Entity Framework DbContext
├── Models/            # Database entities
├── Migrations/        # EF Core database migrations
└── Properties/        # Project properties
```

### Dependency Flow

```
HTTP Request
     ↓
Controller (AuthController)
     ↓
Service (AuthService)
     ↓
Repository (UserRepository)
     ↓
DbContext (AppDbContext)
     ↓
SQL Server
```

## Project Structure

### Controllers
- **AuthController**: Handles authentication endpoints (register, login, test-auth)

### Services
- **IAuthService** / **AuthService**: Contains authentication business logic, password hashing, and JWT generation

### Repositories
- **IUserRepository** / **UserRepository**: Handles all database operations for User entity

### Models
- **User**: Database entity representing a user
- **UserRole**: Enum for user roles (Guest, Staff, Manager, Admin)

### DTOs
- **RegisterRequest**: Request model for user registration
- **LoginRequest**: Request model for user login
- **LoginResponse**: Response model containing JWT token and user data
- **UserResponse**: User information response model

## API Endpoints

### POST /api/auth/register
Register a new user.

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "Password@123"
}
```

**Password Requirements:**
- Minimum 8 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one number
- At least one special character (@$!%*?&#)

**Response (201 Created):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "role": "Guest"
}
```

### POST /api/auth/login
Login with email and password.

**Request Body:**
```json
{
  "email": "john@example.com",
  "password": "Password@123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2024-11-28T10:30:00Z",
  "user": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "role": "Guest"
  }
}
```

### GET /api/auth/test-auth
Test endpoint to verify JWT authentication (requires authentication).

**Headers:**
```
Authorization: Bearer {your-jwt-token}
```

**Response (200 OK):**
```json
{
  "message": "Authentication successful",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "john@example.com",
  "role": "Guest"
}
```

## Configuration

### Database Connection String

Update `appsettings.json` or `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HotelIdentityDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**For SQL Server Authentication:**
```
Server=localhost;Database=HotelIdentityDb;User Id=your_username;Password=your_password;TrustServerCertificate=True;MultipleActiveResultSets=true
```

### JWT Configuration

Configure JWT settings in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "HotelManagementSystem",
    "Audience": "HotelManagementClients",
    "ExpiryMinutes": "60"
  }
}
```

**Important:** 
- Never commit real secrets to version control
- Use environment variables or secret managers in production
- The JWT Key must be at least 32 characters long

## Setup Instructions

### Prerequisites

1. **.NET 8 SDK** or later
2. **SQL Server** (SQL Server Express, Developer Edition, or SQL Server on Docker)
3. **Visual Studio 2022**, **VS Code**, or **Rider** (optional)

### Installation Steps

1. **Clone/Navigate to the project directory**
   ```bash
   cd IdentityService
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection string**
   - Edit `appsettings.Development.json`
   - Set your SQL Server connection string

4. **Create the database**
   ```bash
   dotnet ef database update
   ```
   
   This will:
   - Create the `HotelIdentityDb_Dev` database (or name specified in connection string)
   - Apply all migrations
   - Set up the Users table with proper indexes

5. **Build the project**
   ```bash
   dotnet build
   ```

6. **Run the application**
   ```bash
   dotnet run
   ```

   The API will start at:
   - HTTPS: `https://localhost:7xxx`
   - HTTP: `http://localhost:5xxx`
   
   Exact ports are shown in console output

7. **Access Swagger UI**
   - Navigate to `https://localhost:7xxx/` or `http://localhost:5xxx/`
   - Swagger UI will open showing all available endpoints

## Testing the API

### Using Swagger UI

1. Open Swagger UI in your browser
2. Try the **POST /api/auth/register** endpoint:
   - Click "Try it out"
   - Enter sample user data
   - Click "Execute"
   - Should receive 201 Created response

3. Try the **POST /api/auth/login** endpoint:
   - Use the registered email and password
   - Should receive JWT token in response

4. Test authenticated endpoint:
   - Copy the token from login response
   - Click the "Authorize" button at the top
   - Enter: `Bearer {paste-your-token-here}`
   - Click "Authorize"
   - Try **GET /api/auth/test-auth**
   - Should receive 200 OK with user information

### Using cURL

**Register:**
```bash
curl -X POST https://localhost:7xxx/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "password": "Password@123"
  }'
```

**Login:**
```bash
curl -X POST https://localhost:7xxx/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john@example.com",
    "password": "Password@123"
  }'
```

**Test Authentication:**
```bash
curl -X GET https://localhost:7xxx/api/auth/test-auth \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```

### Using Postman

1. Import the API by pasting the Swagger JSON URL
2. Set up an environment variable for the JWT token
3. Use the login endpoint to get a token
4. Set the token in Authorization header for protected endpoints

## Database Schema

### Users Table

| Column       | Type         | Constraints                    |
|--------------|--------------|--------------------------------|
| Id           | uniqueidentifier | PRIMARY KEY                    |
| FirstName    | nvarchar(100)    | NOT NULL                       |
| LastName     | nvarchar(100)    | NOT NULL                       |
| Email        | nvarchar(255)    | NOT NULL, UNIQUE INDEX         |
| PasswordHash | nvarchar(MAX)    | NOT NULL                       |
| Role         | nvarchar(MAX)    | NOT NULL (Guest/Staff/Manager/Admin) |
| CreatedAt    | datetime2        | NOT NULL, DEFAULT GETUTCDATE() |
| UpdatedAt    | datetime2        | NOT NULL, DEFAULT GETUTCDATE() |
| IsActive     | bit              | NOT NULL, DEFAULT 1            |

### Indexes
- **IX_Users_Email**: Unique index on Email column for fast lookups

## Entity Framework Core Commands

### Create a new migration
```bash
dotnet ef migrations add MigrationName
```

### Apply migrations to database
```bash
dotnet ef database update
```

### Rollback to a specific migration
```bash
dotnet ef database update PreviousMigrationName
```

### Remove last migration (if not applied)
```bash
dotnet ef migrations remove
```

### View migration SQL script
```bash
dotnet ef migrations script
```

## Security Features

1. **Password Hashing**: All passwords are hashed using BCrypt before storage
2. **JWT Authentication**: Secure token-based authentication with configurable expiry
3. **Email Uniqueness**: Enforced at database level with unique index
4. **Input Validation**: Comprehensive validation using Data Annotations
5. **Role-based Authorization**: Users have roles for future authorization requirements
6. **HTTPS**: Enforced in production

## Error Handling

The API returns standard HTTP status codes:

- **200 OK**: Request successful
- **201 Created**: Resource created successfully
- **400 Bad Request**: Invalid request data or validation errors
- **401 Unauthorized**: Invalid credentials or missing/invalid token
- **409 Conflict**: Email already exists during registration
- **500 Internal Server Error**: Server-side error

Error responses include a message field:
```json
{
  "message": "Error description"
}
```

## Future Enhancements

Potential improvements for the IdentityService:

1. Password reset functionality
2. Email verification
3. Refresh tokens
4. Two-factor authentication (2FA)
5. User profile management endpoints
6. Account deactivation/deletion
7. Password change endpoint
8. Admin endpoints for user management
9. Role management
10. Audit logging

## Troubleshooting

### Cannot connect to SQL Server
- Verify SQL Server is running
- Check connection string
- Ensure firewall allows connection
- For SQL Server Express, verify instance name (e.g., `localhost\SQLEXPRESS`)

### Migration errors
```bash
# Drop the database and recreate
dotnet ef database drop
dotnet ef database update
```

### JWT errors
- Verify JWT key is at least 32 characters
- Check that Issuer and Audience match between token generation and validation
- Ensure token hasn't expired

### Port already in use
- Change ports in `Properties/launchSettings.json`
- Or stop the process using the port

## Development

### Build
```bash
dotnet build
```

### Run in development mode
```bash
dotnet run --environment Development
```

### Watch mode (auto-reload on file changes)
```bash
dotnet watch run
```

### Clean build artifacts
```bash
dotnet clean
```

## License

This project is part of the Hotel Management System.

## Contact

For questions or issues, please contact the development team.
