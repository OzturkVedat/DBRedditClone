# Reddit Clone Database

A basic Reddit clone back-end showcasing CRUD operations, raw SQL integration, and database triggers, with ASP.NET Core and PostgreSQL. The project includes Swagger UI for API testing and documentation.

---

## Features

- **CRUD Functionality**: Perform create, read, update, and delete operations on core entities like Posts, Comments, and Subreddits.
- **Raw SQL Scripts**: Use of raw SQL for precise database control.
- **Triggers**: PostgreSQL triggers for automated database tasks.
- **Swagger UI Integration**: Test and view the API endpoints interactively.
- **PostgreSQL Database**: Robust and open source relational database backend.

---

## Project Structure

- **Controllers**: Manage API endpoints.
- **Models**: Represent database entities, DTOs, etc.
- **SqlScripts**: PostgreSQL scripts that seeded into database upon application start.
- **Services**: Directory containing abstraction layers (services) between the API controllers and the database.

---

[SwaggerUI](./assets/swagger.png)

# Pre-requisites
- [PostgreSQL](https://www.postgresql.org/download/)
- pgAdmin (optional, but useful for database management)
- .NET SDK (8.0 or later)

## Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/OzturkVedat/DBRedditClone.git
   cd DBRedditClone
   ```
2. **Set up a Database**
    Create a new database for project. Modify the project's connection string within appsettings.json for local development.
3. **Run the application**
   Install dependencies and run the app with CLI (or just run it from Visual Studio):
   ```bash
   dotnet restore
   dotnet run
   ```
4. Access Swagger UI 
   Navigate to https://localhost:7245/swagger/index.html to test the API endpoint.

[CrowsFoot](.assets/crowsfoot.png)   
   
