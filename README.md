
# ASP.NET Web API Project

## Overview

This project is an ASP.NET Web API built using C#. It implements the Repository Pattern, Unit of Work Pattern, and Specification Pattern. Authentication and authorization are handled using ASP.NET Identity and JWT (JSON Web Token), with CORS policy applied to enable cross-origin requests.

## Features

- **Repository Pattern**: To abstract data access logic and promote testability.
- **Unit of Work Pattern**: To manage database transactions across multiple repositories.
- **Specification Pattern**: For applying complex filtering and querying logic.
- **Authentication & Authorization**:
  - **ASP.NET Identity**: User management and security.
  - **JWT**: Token-based authentication for securing endpoints.
- **CORS Policy**: Configured to allow cross-origin resource sharing, enabling requests from different domains.

## Table of Contents

- [Technologies](#technologies)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [API Endpoints](#api-endpoints)
- [Authentication](#authentication)
- [Running the Project](#running-the-project)
- [License](#license)

## Technologies

- **.NET 6 / .NET 7** (depending on the version)
- **C#**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **ASP.NET Identity**
- **JWT for Authentication**
- **SQL Server** (or any other supported RDBMS)

## Project Structure

```
/src
  /Controllers      # API Controllers for handling requests
  /Models           # Entity Models
  /Repositories     # Repository interfaces and implementations
  /Specifications   # Specification pattern logic
  /UnitOfWork       # Unit of Work implementation
  /DTOs             # Data Transfer Objects
  /Services         # Business logic services
  /Migrations       # Entity Framework migrations
  /Security         # JWT and Identity configuration
  /Middleware       # Custom middlewares (e.g., CORS, error handling)
```

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (6.0+)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or another configured RDBMS)
- [Postman](https://www.postman.com/) (for API testing)

### Setup

1. **Clone the repository**:
    ```bash
    git clone https://github.com/your-repo.git
    cd your-repo
    ```

2. **Install dependencies**:
    ```bash
    dotnet restore
    ```

3. **Database setup**:
    - Update `appsettings.json` with your database connection string.
    - Run migrations to update the database schema:
      ```bash
      dotnet ef database update
      ```

4. **Run the project**:
    ```bash
    dotnet run
    ```

## Configuration

### appsettings.json

- **Connection Strings**: Update the `DefaultConnection` to point to your SQL Server instance.
- **JWT Configuration**: Ensure that your `JWTSettings` have a valid `Issuer`, `Audience`, and `Secret`.

### CORS Policy

By default, the CORS policy allows requests from specific origins. You can configure this in `Startup.cs` or `Program.cs`.

```csharp
services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://example.com")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
```

## API Endpoints

| HTTP Method | Endpoint               | Description                  |
|-------------|------------------------|------------------------------|
| `POST`      | `/api/auth/register`    | Register a new user          |
| `POST`      | `/api/auth/login`       | Log in and get a JWT token   |
| `GET`       | `/api/products`         | Get all products             |
| `POST`      | `/api/products`         | Create a new product         |
| `PUT`       | `/api/products/{id}`    | Update an existing product   |
| `DELETE`    | `/api/products/{id}`    | Delete a product             |

For more endpoints, refer to the [API Documentation](#).

## Authentication

This API uses JWT tokens for authentication. After registering or logging in, you'll receive a token that should be included in the `Authorization` header of your requests.

```bash
Authorization: Bearer <your-token>
```

## Running the Project

1. Run the project using the following command:
    ```bash
    dotnet run
    ```

2. The API will be available at `https://localhost:5001` or `http://localhost:5000`.

## License

This project is licensed under the MIT License.
