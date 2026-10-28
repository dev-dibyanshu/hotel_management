# Quick Start Guide - IdentityService

Get the IdentityService up and running in 5 minutes.

## Prerequisites

✅ .NET 8 SDK installed  
✅ SQL Server running (Express, Developer, or Docker)  
✅ Terminal or command prompt

## Step 1: Verify SQL Server

Make sure SQL Server is running and accessible at `localhost`.

**Test connection:**
```bash
# For Windows with SSMS, try connecting to: localhost or localhost\SQLEXPRESS
# For Docker: docker ps (to check if SQL Server container is running)
```

## Step 2: Update Connection String (if needed)

Edit `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HotelIdentityDb_Dev;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**Common connection string variations:**

**SQL Server Express:**
```
Server=localhost\\SQLEXPRESS;Database=HotelIdentityDb_Dev;Trusted_Connection=True;TrustServerCertificate=True;
```

**SQL Authentication:**
```
Server=localhost;Database=HotelIdentityDb_Dev;User Id=sa;Password=YourPassword;TrustServerCertificate=True;
```

**Docker SQL Server:**
```
Server=localhost,1433;Database=HotelIdentityDb_Dev;User Id=sa;Password=YourPassword;TrustServerCertificate=True;
```

## Step 3: Create the Database

```bash
cd IdentityService
dotnet ef database update
```

**Expected output:**
```
Build started...
Build succeeded.
Done.
```

This creates the `HotelIdentityDb_Dev` database with the Users table.

## Step 4: Run the Service

```bash
dotnet run
```

**Expected output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7xxx
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5xxx
```

## Step 5: Open Swagger UI

Open your browser and navigate to:
```
https://localhost:7xxx
```

(Use the HTTPS port shown in your terminal output)

The Swagger UI will load automatically.

## Step 6: Test the API

### 6.1 Register a User

1. In Swagger UI, find **POST /api/auth/register**
2. Click **"Try it out"**
3. Enter this sample data:

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "Test@123"
}
```

4. Click **"Execute"**
5. You should see **201 Created** response

### 6.2 Login

1. Find **POST /api/auth/login**
2. Click **"Try it out"**
3. Enter:

```json
{
  "email": "john.doe@example.com",
  "password": "Test@123"
}
```

4. Click **"Execute"**
5. You should see **200 OK** with a JWT token in the response
6. **Copy the token value** (the long string after "token":")

### 6.3 Test Authentication

1. Click the **"Authorize"** button at the top of Swagger UI
2. In the dialog, enter:
```
Bearer YOUR_TOKEN_HERE
```
(Replace YOUR_TOKEN_HERE with the token you copied)

3. Click **"Authorize"** then **"Close"**
4. Find **GET /api/auth/test-auth**
5. Click **"Try it out"** then **"Execute"**
6. You should see **200 OK** with your user information

## Troubleshooting

### Cannot connect to database

**Error:** "A network-related or instance-specific error occurred while establishing a connection to SQL Server"

**Solution:**
- Verify SQL Server is running
- Check your connection string
- Try: `Server=localhost\\SQLEXPRESS` if using SQL Server Express
- Ensure Windows Firewall allows SQL Server connections

### Port already in use

**Error:** "Failed to bind to address"

**Solution:**
- Stop any running instances of the service
- Or change ports in `Properties/launchSettings.json`

### Migration fails

**Error:** "Unable to create migrations"

**Solution:**
```bash
# Make sure you're in the IdentityService directory
cd IdentityService

# Try removing and recreating migration
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Certificate errors (HTTPS)

**Error:** "The SSL connection could not be established"

**Solution:**
```bash
# Trust the development certificate
dotnet dev-certs https --trust
```

## Testing with cURL

If you prefer command line testing:

**Register:**
```bash
curl -k -X POST https://localhost:7xxx/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Jane",
    "lastName": "Smith",
    "email": "jane.smith@example.com",
    "password": "Test@123"
  }'
```

**Login:**
```bash
curl -k -X POST https://localhost:7xxx/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "jane.smith@example.com",
    "password": "Test@123"
  }'
```

**Test Auth (replace TOKEN):**
```bash
curl -k -X GET https://localhost:7xxx/api/auth/test-auth \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

Note: `-k` flag skips certificate validation for development.

## Password Requirements

Passwords must have:
- Minimum 8 characters
- At least one uppercase letter (A-Z)
- At least one lowercase letter (a-z)
- At least one number (0-9)
- At least one special character (@$!%*?&#)

**Valid examples:**
- `Test@123`
- `Password@123`
- `Secure#456`
- `MyPass$789`

## What's Next?

✅ Service is running  
✅ Database is created  
✅ User registration works  
✅ JWT authentication works  

**Next steps:**
1. Try registering multiple users with different roles
2. Test duplicate email rejection
3. Test invalid password formats
4. Test invalid login credentials
5. Review the README.md for more detailed information

## Stop the Service

Press `Ctrl+C` in the terminal where the service is running.

## Database Management

**View the database:**
- Use SQL Server Management Studio (SSMS)
- Connect to your SQL Server instance
- Find the `HotelIdentityDb_Dev` database
- Check the `Users` table

**Reset the database:**
```bash
dotnet ef database drop
dotnet ef database update
```

## Common URLs

- **Swagger UI:** https://localhost:7xxx/
- **API Base:** https://localhost:7xxx/api/
- **Register:** POST https://localhost:7xxx/api/auth/register
- **Login:** POST https://localhost:7xxx/api/auth/login
- **Test Auth:** GET https://localhost:7xxx/api/auth/test-auth

Replace `7xxx` with your actual HTTPS port.

## Success! 🎉

You now have a working Identity Service with:
- User registration
- Login with JWT tokens
- Password hashing
- Role-based user management
- Swagger API documentation

For more details, see [README.md](README.md) and [PROJECT_SUMMARY.md](PROJECT_SUMMARY.md).
