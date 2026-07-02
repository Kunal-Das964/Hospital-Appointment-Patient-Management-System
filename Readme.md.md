# 🏥 Hospital Appointment & Patient Management System

A comprehensive hospital management solution built with **ASP.NET Core Web API**, **ADO.NET**, and **SQL Server**. The application enables hospitals to manage patients, doctors, and appointments through a RESTful API while following clean architecture principles.

## Project Structure

```text
Hospital Appointment & Patient Management System/
├── HospitalAppointmentAPI/
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   ├── Repository/
│   ├── Services/
│   ├── Interfaces/
│   ├── Middleware/
│   ├── Exceptions/
│   ├── Helpers/
│   ├── appsettings.json
│   └── Program.cs
├── SQLScripts/
├── PostmanCollection/
└── HospitalAppointmentAPI.sln
```

## Features

### Patient Management
- Register new patients
- Update patient information
- Soft delete patient records
- Calculate patient age automatically
- Enforce unique phone numbers and email addresses

### Doctor Management
- Manage doctor profiles
- Track specialization, consultation fee, and availability
- Search doctors by specialization

### Appointment Management
- Book appointments
- Prevent booking with unavailable doctors
- Cancel scheduled appointments
- View upcoming appointments
- Retrieve appointments for a specific doctor

### Reports
- Appointment summary
- Total appointments per doctor
- Consultation revenue by specialization
- Duplicate appointment detection
- Upcoming appointments within 7 days

## Tech Stack

| Layer | Technology |
|--------|------------|
| Backend | ASP.NET Core Web API |
| Language | C# |
| Database | SQL Server |
| Data Access | ADO.NET |
| API Testing | Postman |
| Documentation | Swagger |

## Getting Started

1. Clone the repository.
2. Create the SQL Server database using the SQL scripts.
3. Update `appsettings.json` with your connection string.
4. Run:

```bash
dotnet restore
dotnet build
dotnet run
```

## Project Highlights

- RESTful API
- ADO.NET
- SQL Server Stored Procedures
- Repository Pattern
- Dependency Injection
- Middleware
- Global Exception Handling
- SQL Transactions
- Soft Delete
- Clean Architecture

## License

This project is intended for educational purposes.
