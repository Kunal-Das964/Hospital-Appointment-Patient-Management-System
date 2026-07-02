# 🏥 Hospital Appointment & Patient Management System

> A fully working REST API backend built with **C# | ASP.NET Core Web API | ADO.NET | SQL Server**  
> following **Clean Architecture** principles across 4 separate layers.

---

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Folder Structure](#folder-structure)
- [Database Design](#database-design)
- [API Endpoints](#api-endpoints)
- [Setup & Installation](#setup--installation)
- [Running the Application](#running-the-application)
- [Testing with Postman](#testing-with-postman)
- [Assumptions Made](#assumptions-made)

---

## Project Overview

City General Hospital requires a backend system to manage:

- 👤 **Patient Registrations** — register, update, deactivate (soft delete)
- 🩺 **Doctor Profiles** — store and query by specialization & availability
- 📅 **Appointment Bookings** — book, cancel, and report on appointments

---

## Tech Stack

| Layer | Technology |
|---|---|
| Language | C# (.NET 8.0) |
| API Framework | ASP.NET Core Web API |
| Data Access | ADO.NET (Stored Procedures only) |
| Database | Microsoft SQL Server |
| API Testing | Postman |

---

## Architecture

The solution follows **Clean Architecture** with 4 separate projects:

```
HospitalManagement (Solution)
│
├── HospitalManagement.Domain          ← Business rules, Entities, Exceptions
├── HospitalManagement.Application     ← DTOs, Interfaces
├── HospitalManagement.Infrastructure  ← ADO.NET Repositories, Services
└── HospitalManagement.API             ← Controllers, Middleware, Program.cs
```

### Layer Responsibilities

**Domain Layer**
- Contains `Patient`, `Doctor`, `Appointment` entities
- `Patient` and `Doctor` inherit from `BaseEntity` (shared fields)
- Age calculation lives here — NOT in DB or API
- Typed exceptions: `DomainException`, `PastAppointmentDateException`, `InvalidStatusTransitionException`

**Application Layer**
- DTOs for all request/response models with Data Annotations
- `IPatientRepository` and `IAppointmentRepository` interfaces
- `IPatientService`, `IDoctorService`, `IAppointmentService` interfaces
- Note: `IDoctorRepository` is intentionally omitted (per assignment requirement)

**Infrastructure Layer**
- ADO.NET implementations using Stored Procedures only — zero inline SQL
- `PatientRepository`, `AppointmentRepository`
- `PatientService`, `DoctorService`, `AppointmentService`

**API Layer**
- REST Controllers with correct HTTP methods and status codes
- `GlobalExceptionMiddleware` — structured error responses
- `RequestLoggingMiddleware` — logs method, path, response time globally

---

## Folder Structure

```
HospitalManagement/
│
├── HospitalManagement.sln
│
├── HospitalManagement.API/
│   ├── Controllers/
│   │   ├── PatientsController.cs
│   │   ├── DoctorsController.cs
│   │   └── AppointmentsController.cs
│   ├── Middleware/
│   │   ├── GlobalExceptionMiddleware.cs
│   │   └── RequestLoggingMiddleware.cs
│   ├── appsettings.json
│   └── Program.cs
│
├── HospitalManagement.Domain/
│   ├── Entities/
│   │   ├── BaseEntity.cs
│   │   ├── Patient.cs
│   │   ├── Doctor.cs
│   │   └── Appointment.cs
│   ├── Enums/
│   │   └── AppointmentStatus.cs
│   └── Exceptions/
│       ├── DomainException.cs
│       ├── PastAppointmentDateException.cs
│       └── InvalidStatusTransitionException.cs
│
├── HospitalManagement.Application/
│   ├── DTOs/
│   │   ├── Patient/
│   │   │   ├── CreatePatientDto.cs
│   │   │   ├── UpdatePatientDto.cs
│   │   │   └── PatientResponseDto.cs
│   │   ├── Doctor/
│   │   │   ├── CreateDoctorDto.cs
│   │   │   └── DoctorResponseDto.cs
│   │   └── Appointment/
│   │       ├── BookAppointmentDto.cs
│   │       ├── AppointmentResponseDto.cs
│   │       └── AppointmentReportDto.cs
│   └── Interfaces/
│       ├── IPatientRepository.cs
│       ├── IAppointmentRepository.cs
│       ├── IPatientService.cs
│       ├── IDoctorService.cs
│       └── IAppointmentService.cs
│
├── HospitalManagement.Infrastructure/
│   ├── Repositories/
│   │   ├── PatientRepository.cs
│   │   └── AppointmentRepository.cs
│   └── Services/
│       ├── PatientService.cs
│       ├── DoctorService.cs
│       └── AppointmentService.cs
│
└── Database/
    ├── 01_Schema.sql
    ├── 02_StoredProcedures.sql
    └── 03_SampleData.sql
```

---

## Database Design

### Tables

| Table | Key Fields |
|---|---|
| `Patients` | PatientId, PatientCode, FullName, DateOfBirth, Gender, PhoneNumber, Email, IsActive |
| `Doctors` | DoctorId, DoctorCode, FullName, Specialization, PhoneNumber, ConsultationFee, IsAvailable |
| `Appointments` | AppointmentId, PatientId (FK), DoctorId (FK), AppointmentDate, Status, CancelledAt |

### Key Design Decisions

- `IsActive` flag on Patients — **soft delete** (never hard deleted)
- `Email` uses **Filtered Unique Index** — allows multiple NULLs, blocks duplicate non-NULL values
- `Appointments.Status` — CHECK constraint: `Scheduled | Completed | Cancelled`
- **Referential Integrity** — FK constraints between all 3 tables
- **Indexes** on `DoctorId`, `AppointmentDate`, `Status` for query performance

### Stored Procedures (15 total)

| Stored Procedure | Purpose |
|---|---|
| `sp_RegisterPatient` | Register with duplicate phone/email check |
| `sp_UpdatePatient` | Update patient details |
| `sp_DeactivatePatient` | Soft delete |
| `sp_GetAllActivePatients` | All active patients |
| `sp_GetPatientById` | Single patient |
| `sp_GetDoctors` | Filter by specialization & availability |
| `sp_GetDoctorById` | Single doctor |
| `sp_BookAppointment` | Book with transaction + rollback |
| `sp_CancelAppointment` | Cancel with timestamp |
| `sp_GetUpcomingAppointments` | Next 7 days |
| `sp_GetAppointmentsByDoctor` | Doctor's appointments |
| `sp_GetConsolidatedReport` | Full report |
| `sp_GetDoctorAppointmentCounts` | Doctors with > 2 appointments |
| `sp_GetRevenueBySpecialization` | Revenue grouped by specialization |
| `sp_GetDuplicateBookings` | Same doctor, same date |

---

## API Endpoints

### Patients

| Method | Endpoint | Description | Status Code |
|---|---|---|---|
| `POST` | `/api/patients` | Register new patient | `201` |
| `GET` | `/api/patients` | Get all active patients with age | `200` |
| `PUT` | `/api/patients/{id}` | Update patient details | `204` |
| `DELETE` | `/api/patients/{id}` | Deactivate patient (soft delete) | `204` |

### Doctors

| Method | Endpoint | Description | Status Code |
|---|---|---|---|
| `GET` | `/api/doctors` | Get all doctors | `200` |
| `GET` | `/api/doctors?specialization=X&isAvailable=true` | Filter doctors | `200` |

### Appointments

| Method | Endpoint | Description | Status Code |
|---|---|---|---|
| `POST` | `/api/appointments` | Book appointment | `201` |
| `PATCH` | `/api/appointments/{id}/cancel` | Cancel appointment | `204` |
| `GET` | `/api/appointments/upcoming` | Next 7 days appointments | `200` |
| `GET` | `/api/appointments/doctor/{id}` | Appointments by doctor | `200` |

### Reports

| Method | Endpoint | Description | Status Code |
|---|---|---|---|
| `GET` | `/api/appointments/report` | Consolidated report | `200` |
| `GET` | `/api/appointments/report/doctor-counts` | Doctor counts (> 2 only) | `200` |
| `GET` | `/api/appointments/report/revenue` | Revenue by specialization | `200` |
| `GET` | `/api/appointments/report/duplicates` | Duplicate bookings | `200` |

### HTTP Status Codes

| Code | When |
|---|---|
| `201 Created` | Successful creation |
| `200 OK` | Successful GET |
| `204 No Content` | Successful update / delete / cancel |
| `400 Bad Request` | Validation failure, past date, invalid status |
| `404 Not Found` | Patient or appointment does not exist |
| `409 Conflict` | Duplicate phone/email, unavailable doctor |
| `500 Internal Server Error` | Unhandled exceptions |

---

## Setup & Installation

### Prerequisites

- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- [Postman](https://www.postman.com/downloads/)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Step 1 — Database Setup

Open SSMS and run the scripts **in order**:

```
1. Database/01_Schema.sql           → Creates HospitalDB, tables, indexes
2. Database/02_StoredProcedures.sql → Creates all 15 stored procedures
3. Database/03_SampleData.sql       → Inserts sample doctors, patients, appointments
```

### Step 2 — Update Connection String

Open `HospitalManagement.API/appsettings.json` and update:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=HospitalDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Replace `YOUR_SERVER_NAME` with your SQL Server instance name  
> (e.g. `localhost`, `.\SQLEXPRESS`, `DESKTOP-ABC\SQLEXPRESS`)

### Step 3 — Open Solution

```
File → Open → Project/Solution → HospitalManagement.sln
```

### Step 4 — Set Startup Project

Right-click `HospitalManagement.API` → **Set as Startup Project**

---

## Running the Application

```bash
# Press Ctrl+F5 in Visual Studio
# OR from terminal:
cd HospitalManagement.API
dotnet run
```

Swagger UI will open automatically at:
```
https://localhost:{port}/swagger
```

---

## Testing with Postman

1. Open Postman
2. **Import** `HospitalManagement.postman_collection.json`
3. Set environment variable:
   ```
   baseUrl = https://localhost:{your-port}
   ```
4. Run requests and verify status codes

---

## Assumptions Made

| # | Assumption |
|---|---|
| 1 | `IDoctorRepository` not created — assignment only requires Patient & Appointment data access abstracted behind interfaces |
| 2 | Email uses **Filtered Unique Index** to allow multiple NULLs while blocking duplicate non-NULL emails |
| 3 | **Age calculation** is in C# Domain (`Patient.Age` property) — DB only returns `DateOfBirth` |
| 4 | **Cancelled appointments** excluded from revenue reports — only Scheduled & Completed count |
| 5 | Doctor write operations (register/update) not exposed via API — only read operations available |
| 6 | Appointment status only changes through specific actions — `BookAppointment` sets Scheduled, `CancelAppointment` sets Cancelled |
| 7 | `FormattedDate` returned as `dd MMM yyyy hh:mm tt` format — computed in Domain layer |
| 8 | Doctor hard delete is allowed — only Patient requires soft delete (IsActive flag) |

---

## Domain Rules

All business logic is in the **Domain layer** — not in DB or API:

```csharp
// Age — calculated in Patient entity
public int Age => /* uses DateOfBirth */

// Status check — in Appointment entity  
public bool CanBeCancelled() => Status == AppointmentStatus.Scheduled;

// Past date check — in Appointment entity
public bool IsInPast() => AppointmentDate < DateTime.Now;

// Readable format — in Appointment entity
public string FormattedDate => AppointmentDate.ToString("dd MMM yyyy hh:mm tt");
```

---

## Submission Checklist

- [x] Source Code — `HospitalManagement.zip`
- [x] SQL Scripts — `Database/` folder (3 files)
- [x] Postman Collection — `HospitalManagement.postman_collection.json`
- [x] Project Document — `HospitalManagement_ProjectDocument.pdf`
- [x] README — `README.md`
