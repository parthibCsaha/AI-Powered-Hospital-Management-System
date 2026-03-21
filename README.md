# 🏥 Hospital Management System — Backend API

A production-ready RESTful API for managing hospital operations, built with **ASP.NET Core 8** and **Clean Architecture** principles.

## ✨ Features

- **Patient Management** — Register, update, and track patients with full medical history
- **Doctor & Schedule Management** — Manage doctors, specializations, and weekly schedules
- **Appointments** — Book, update, cancel, and track appointment statuses
- **Medical Records** — Create and retrieve patient medical records
- **Room & Admission Management** — Track room availability, patient admissions, and discharges
- **Department Management** — Organize doctors and resources by department
- **Dashboard Analytics** — Hospital-wide statistics at a glance
- **Authentication & Authorization** — JWT-based auth with role-based access control and refresh tokens

## 🏗️ Architecture

The project follows **Clean Architecture** with four layers:

```
HMS-Backend/
├── Domain/            # Entities, Enums, Core business rules
├── Application/       # DTOs, Interfaces, Services, Validators, Mappings
├── Infrastructure/    # EF Core DbContext, Repositories, Auth services
└── API/               # Controllers, Middleware, Extensions
```

### Key Patterns
- **Repository & Unit of Work** for data access
- **CQRS-lite** with service layer abstraction
- **Global Query Filters** for soft-delete across all entities
- **Global Exception Handling** middleware
- **Structured Request Logging** middleware

## 🛠️ Tech Stack

| Layer          | Technology                              |
|----------------|----------------------------------------|
| Framework      | ASP.NET Core 8.0                       |
| Database       | PostgreSQL + EF Core 8                 |
| Auth           | JWT Bearer + Refresh Tokens + BCrypt   |
| Validation     | FluentValidation                       |
| Mapping        | AutoMapper 15                          |
| Logging        | Serilog (Console + File sinks)         |
| API Docs       | Swagger / OpenAPI (Swashbuckle)        |
| Health Checks  | EF Core Database Health Check          |

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) (running on `localhost:5432`)

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/<your-username>/HMS.git
cd HMS
```

### 2. Configure the database
Update the connection string in `HMS-Backend/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=hmsdb;Username=postgres;Password=<your-password>"
}
```

### 3. Run the application
```bash
dotnet run --project HMS-Backend
```

The app will automatically:
- Apply EF Core migrations (creates all tables)
- Seed initial data (10 departments, 28 rooms, admin user)

### 4. Open Swagger UI
Navigate to **http://localhost:5104** to explore the API.

## 📚 API Endpoints

### 🔐 Auth
| Method | Endpoint                   | Description          | Auth     |
|--------|----------------------------|----------------------|----------|
| POST   | `/api/v1/auth/register`    | Register new user    | Public   |
| POST   | `/api/v1/auth/login`       | Login                | Public   |
| POST   | `/api/v1/auth/refresh-token` | Refresh JWT token  | Public   |
| POST   | `/api/v1/auth/logout`      | Logout (revoke token)| Required |

### 👤 Patients
| Method | Endpoint                   | Description          |
|--------|----------------------------|----------------------|
| GET    | `/api/v1/patients`         | List all patients    |
| GET    | `/api/v1/patients/{id}`    | Get patient details  |
| POST   | `/api/v1/patients`         | Register patient     |
| PUT    | `/api/v1/patients/{id}`    | Update patient       |

### 🩺 Doctors
| Method | Endpoint                                | Description               |
|--------|-----------------------------------------|---------------------------|
| GET    | `/api/v1/doctors`                       | List all doctors          |
| GET    | `/api/v1/doctors/{id}`                  | Get doctor details        |
| GET    | `/api/v1/doctors/by-department/{deptId}`| Doctors by department     |
| GET    | `/api/v1/doctors/{id}/schedules`        | Get doctor's schedule     |
| POST   | `/api/v1/doctors`                       | Add doctor                |
| PUT    | `/api/v1/doctors/{id}`                  | Update doctor             |

### 📅 Appointments
| Method | Endpoint                                     | Description              |
|--------|----------------------------------------------|--------------------------|
| GET    | `/api/v1/appointments`                       | List all appointments    |
| GET    | `/api/v1/appointments/{id}`                  | Get appointment details  |
| GET    | `/api/v1/appointments/patient/{patientId}`   | By patient               |
| GET    | `/api/v1/appointments/doctor/{doctorId}`     | By doctor                |
| POST   | `/api/v1/appointments`                       | Book appointment         |
| PUT    | `/api/v1/appointments/{id}`                  | Update appointment       |
| PATCH  | `/api/v1/appointments/{id}/status`           | Update status            |

### 📁 Medical Records · 🏢 Departments · 🛏️ Rooms · 📊 Dashboard
Full CRUD available — explore via **Swagger UI** at `http://localhost:5104`.

## 👥 User Roles

| Role           | Description                          |
|----------------|--------------------------------------|
| `Admin`        | Full system access                   |
| `Doctor`       | Medical records, appointments        |
| `Nurse`        | Patient care, records                |
| `Receptionist` | Appointments, patient registration   |
| `Patient`      | View own records                     |

## 🌱 Default Seed Data

On first run, the app seeds:
- **10 Departments** — Cardiology, Neurology, Orthopedics, Pediatrics, etc.
- **28 Rooms** — General Wards, Private, ICU, Operating Theatres
- **Admin User** — `admin@hospital.com` / `Admin@1234`

## 🔧 Configuration

| Setting                  | Location              | Description                      |
|--------------------------|-----------------------|----------------------------------|
| Database Connection      | `appsettings.json`    | PostgreSQL connection string     |
| JWT Secret & Expiry      | `appsettings.json`    | Token signing key and TTL        |
| Serilog Log Levels       | `appsettings.json`    | Minimum log level overrides      |
| CORS Policy              | `ServiceExtensions.cs`| Allowed origins for frontend     |

## 📄 License

This project is open source and available under the [MIT License](LICENSE).
