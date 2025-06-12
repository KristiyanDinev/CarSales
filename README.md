# CarSales

**CarSales** is a web-based car listing and management system built with **ASP.NET Core MVC** and **MySQL**. It supports user registration, login, admin car management, and secure role-based access. Authentication is managed via ASP.NET Identity, with session tracking through cookies.

## Features

* User registration and login (with Identity)
* Admin panel to manage cars and users
* View car listings and detailed car info
* Rate-limited API endpoints for protection
* Role-based access control
* Cookie-based authentication

## Tech Stack

* ASP.NET Core MVC
* Entity Framework Core (MySQL)
* ASP.NET Identity
* MySQL
* Swagger (OpenAPI spec included)

---

## Installation

### Prerequisites

* [.NET SDK 8.0+](https://dotnet.microsoft.com/)
* [MySQL Server 8+](https://dev.mysql.com/downloads/)
* Visual Studio or VS Code

### 1. Clone the Repository

```bash
git clone https://github.com/KristiyanDinev/CarSales.git
cd CarSales
```

### 2. Configure the Database

Update the `appsettings.json` with your MySQL credentials:

```json
{
  "ConnectionString": "server=127.0.0.1;uid=root;pwd=root;database=CarsDB"
}
```

> Make sure the database `CarsDB` exists. You can create it using:

```sql
CREATE DATABASE CarsDB;
```

### 3. Apply Migrations

If using EF Core Migrations:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Run the Application

```bash
dotnet run
```

Navigate to `https://localhost:5001` (or the port printed in the console).

---

## Authentication

The app uses **cookie-based authentication**. On login, a cookie is created with the name `Authentication`.

* Default admin user:

  * **Email:** admin@example.com
  * **Password:** Admin123!
* Automatically created on first run with `Admin` role.

---

## API & OpenAPI

The application includes a Swagger-compatible **OpenAPI JSON** file defining all endpoints. You can use this for client generation, testing, or documentation.

---

## Admin Actions

After logging in as admin, you can:

* View, create, edit, and delete car listings
* Assign/remove admin roles from other users

---

## Testing

* Test rate limiting by calling endpoints rapidly (1 request/sec limit)
* Try logging in with invalid credentials to validate error handling

---

## Contact

For issues or contributions, feel free to open an issue or pull request.

