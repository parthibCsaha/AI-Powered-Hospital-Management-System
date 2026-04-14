# 🏥 Hospital Management System — Backend API

A production-ready RESTful API for managing hospital operations, built with **ASP.NET Core 8** and **Clean Architecture** principles. Features include patient management, doctor scheduling, appointment booking, medical records, room & admission tracking, role-based access control, and **AI-powered medical report summarization** via Groq.

---

## ✨ Features

- **Patient Management** — Register, update, and track patients with full medical history
- **Doctor & Schedule Management** — Manage doctors, specializations, and weekly schedules
- **Appointments** — Book, update, cancel, and track appointment statuses
- **Medical Records** — Create and retrieve patient medical records with diagnoses, prescriptions, and lab results
- **Room & Admission Management** — Track room availability, patient admissions, and discharges
- **Department Management** — Organize doctors and resources by department
- **Dashboard Analytics** — Hospital-wide statistics at a glance
- **Authentication & Authorization** — JWT-based auth with role-based access control and refresh tokens
- **User Profile** — `/me` endpoint for authenticated users to view their own profile
- **AI-Powered Reports** — Summarize medical records and patient history in 15 languages using Groq AI (LLaMA 3.3 70B)
- **Health Checks** — Built-in `/health` endpoint for monitoring database connectivity
- **API Seed Script** — Python script to populate all tables with realistic hospital data

---

## 🏗️ Architecture

The project follows **Clean Architecture** with four layers:

```
HMS-Backend/
├── Domain/            # Entities, Enums, Core business rules
│   ├── Entities/      # User, Patient, Doctor, DoctorSchedule, Appointment,
│   │                  # MedicalRecord, Department, Room, Admission
│   ├── Enums/         # UserRole, Gender, BloodGroup, RoomType, AppointmentStatus
│   └── Common/        # BaseEntity, ISoftDeletable
│
├── Application/       # DTOs, Interfaces, Services, Validators, Mappings
│   ├── DTOs/          # Request/Response DTOs for all resources + AI reports
│   ├── Interfaces/    # Service contracts (IAuthService, IAiReportService, etc.)
│   ├── Services/      # Business logic layer
│   ├── Validators/    # FluentValidation rules
│   ├── Mappings/      # AutoMapper profiles
│   └── Common/        # ApiResponse, PagedResult, PaginationParams
│
├── Infrastructure/    # EF Core DbContext, Repositories, Auth, AI integration
│   ├── Persistence/   # ApplicationDbContext, UnitOfWork, Repositories, Seeding
│   └── Services/      # AuthService, JwtTokenService, GroqAiReportService
│
└── API/               # Controllers, Middleware, Extensions
    ├── Controllers/   # 11 controllers (Auth, Patients, Doctors, Appointments, etc.)
    ├── Middleware/     # GlobalExceptionMiddleware, RequestLoggingMiddleware
    └── Extensions/    # Swagger, JWT, CORS configuration
```

### Key Patterns

- **Repository & Unit of Work** for data access
- **CQRS-lite** with service layer abstraction
- **Global Query Filters** for soft-delete across all entities
- **Global Exception Handling** middleware
- **Structured Request Logging** middleware
- **Typed HttpClient** for Groq AI integration

---

## 🛠️ Tech Stack

| Layer            | Technology                                |
|------------------|-------------------------------------------|
| Framework        | ASP.NET Core 8.0                          |
| Language         | C# 12                                     |
| Database         | PostgreSQL 16 + EF Core 8                 |
| Auth             | JWT Bearer + Refresh Tokens + BCrypt      |
| Validation       | FluentValidation 12                       |
| Mapping          | AutoMapper 15                             |
| Logging          | Serilog (Console + File sinks)            |
| API Docs         | Swagger / OpenAPI (Swashbuckle)           |
| Health Checks    | EF Core Database Health Check             |
| AI               | Groq API — LLaMA 3.3 70B Versatile       |
| Containerization | Docker + Docker Compose                   |

---

## 📋 Prerequisites

- [Docker](https://docs.docker.com/get-docker/) & [Docker Compose](https://docs.docker.com/compose/) (recommended)
- _OR_ [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) + [PostgreSQL 16](https://www.postgresql.org/download/)
- _Optional:_ [Python 3](https://www.python.org/downloads/) + `requests` library (for the seed script)

---

## 🚀 Getting Started

### Option A: Docker (Recommended)

```bash
git clone https://github.com/parthibCsaha/Hospital-Management-System.git
cd HMS

# Configure environment
cp .env.example .env
# Edit .env with your credentials

# Start everything
docker compose up --build
```

The API will be available at **http://localhost:5104**.

### Option B: Local Development

```bash
# 1. Clone
git clone https://github.com/parthibCsaha/Hospital-Management-System.git
cd HMS

# 2. Update connection string in HMS-Backend/appsettings.json
# 3. Run
dotnet run --project HMS-Backend
```

The app will automatically:

- Apply EF Core migrations (creates all tables)
- Seed initial data (departments, rooms, admin user)

### Explore the API

Navigate to **http://localhost:5104** to open Swagger UI with full endpoint documentation and a built-in "Try it out" feature.

---

## 📚 API Endpoints

All endpoints are prefixed with `/api/v1`.

### 🔐 Auth (`/api/v1/auth`)

| Method | Endpoint         | Description              | Auth     |
|--------|------------------|--------------------------|----------|
| POST   | `/register`      | Register new user        | Public   |
| POST   | `/login`         | Login & receive JWT      | Public   |
| POST   | `/refresh-token` | Refresh an expired JWT   | Public   |
| POST   | `/logout`        | Logout (revoke token)    | Required |
| GET    | `/me`            | Get current user profile | Required |

---

### 👤 Patients (`/api/v1/patients`)

| Method | Endpoint    | Description        | Auth                |
|--------|-------------|--------------------|---------------------|
| GET    | `/`         | List all patients  | Admin, Receptionist |
| GET    | `/{id}`     | Get patient by ID  | Required            |
| POST   | `/`         | Register patient   | Admin, Receptionist |
| PUT    | `/{id}`     | Update patient     | Admin, Receptionist |
| DELETE | `/{id}`     | Delete patient     | Admin               |

---

### 🩺 Doctors (`/api/v1/doctors`)

| Method | Endpoint                      | Description               | Auth     |
|--------|-------------------------------|---------------------------|----------|
| GET    | `/`                           | List all doctors          | Required |
| GET    | `/{id}`                       | Get doctor by ID          | Required |
| GET    | `/by-department/{deptId}`     | Doctors by department     | Required |
| GET    | `/{id}/schedules`             | Get doctor's schedule     | Required |
| POST   | `/`                           | Add doctor                | Admin    |
| PUT    | `/{id}`                       | Update doctor             | Admin    |
| PUT    | `/{id}/schedules`             | Upsert weekly schedule    | Admin    |
| DELETE | `/{id}`                       | Delete doctor             | Admin    |

---

### 📅 Appointments (`/api/v1/appointments`)

| Method | Endpoint                    | Description              | Auth                        |
|--------|-----------------------------|--------------------------|-----------------------------|
| GET    | `/`                         | List all appointments    | Admin, Doctor, Receptionist |
| GET    | `/{id}`                     | Get appointment by ID    | Required                    |
| GET    | `/patient/{patientId}`      | By patient               | Required                    |
| GET    | `/doctor/{doctorId}?date=`  | By doctor & date         | Required                    |
| POST   | `/`                         | Book appointment         | Admin, Receptionist, Patient|
| PUT    | `/{id}`                     | Update appointment       | Admin, Receptionist         |
| PATCH  | `/{id}/status`              | Update status            | Admin, Doctor, Receptionist |
| DELETE | `/{id}`                     | Delete appointment       | Admin                       |

---

### 📁 Medical Records (`/api/v1/medical-records`)

| Method | Endpoint               | Description          | Auth          |
|--------|------------------------|----------------------|---------------|
| GET    | `/`                    | List all records     | Admin, Doctor |
| GET    | `/{id}`                | Get record by ID     | Required      |
| GET    | `/patient/{patientId}` | Records by patient   | Required      |
| POST   | `/`                    | Create record        | Admin, Doctor |
| PUT    | `/{id}`                | Update record        | Admin, Doctor |
| DELETE | `/{id}`                | Delete record        | Admin         |

---

### 🏢 Departments (`/api/v1/departments`)

| Method | Endpoint | Description        | Auth     |
|--------|----------|--------------------|----------|
| GET    | `/`      | List all depts     | Required |
| GET    | `/{id}`  | Get department     | Required |
| POST   | `/`      | Create department  | Admin    |
| PUT    | `/{id}`  | Update department  | Admin    |
| DELETE | `/{id}`  | Delete department  | Admin    |

---

### 🛏️ Rooms (`/api/v1/rooms`)

| Method | Endpoint                  | Description              | Auth     |
|--------|---------------------------|--------------------------|----------|
| GET    | `/?availableOnly=true`    | List rooms (filterable)  | Required |
| GET    | `/{id}`                   | Get room by ID           | Required |
| POST   | `/`                       | Create room              | Admin    |
| PUT    | `/{id}`                   | Update room              | Admin    |
| DELETE | `/{id}`                   | Delete room              | Admin    |

---

### 🏨 Admissions (`/api/v1/admissions`)

| Method | Endpoint                 | Description             | Auth                        |
|--------|--------------------------|-------------------------|-----------------------------|
| GET    | `/`                      | List all admissions     | Admin, Doctor, Receptionist |
| GET    | `/{id}`                  | Get admission by ID     | Required                    |
| GET    | `/patient/{patientId}`   | Admissions by patient   | Required                    |
| POST   | `/`                      | Admit patient           | Admin, Receptionist         |
| PATCH  | `/{id}/discharge`        | Discharge patient       | Admin, Doctor, Receptionist |

---

### 📊 Dashboard (`/api/v1/dashboard`)

| Method | Endpoint | Description         | Auth                        |
|--------|----------|---------------------|-----------------------------|
| GET    | `/stats` | Hospital-wide stats | Admin, Doctor, Receptionist |

---

### 🤖 AI Reports (`/api/v1/ai-reports`)

| Method | Endpoint                           | Description                       | Auth     |
|--------|------------------------------------|-----------------------------------|----------|
| GET    | `/languages`                       | List 15 supported languages       | Required |
| POST   | `/summarize/record/{recordId}`     | AI-summarize a medical record     | Required |
| POST   | `/summarize/patient/{patientId}`   | AI-summarize full patient history | Required |

**Request body** (both POST endpoints):

```json
{
  "language": "bn"
}
```

**Response**:

```json
{
  "success": true,
  "message": "Report summarized successfully.",
  "data": {
    "summary": "... AI-generated summary in chosen language ...",
    "language": "Bengali",
    "model": "llama-3.3-70b-versatile",
    "tokensUsed": 847,
    "generatedAt": "2026-04-14T04:15:00Z"
  }
}
```

#### Supported Languages

| Code | Language   | Native     |
|------|------------|------------|
| en   | English    | English    |
| bn   | Bengali    | বাংলা      |
| hi   | Hindi      | हिन्दी       |
| es   | Spanish    | Español    |
| fr   | French     | Français   |
| ar   | Arabic     | العربية     |
| zh   | Chinese    | 中文        |
| pt   | Portuguese | Português  |
| de   | German     | Deutsch    |
| ja   | Japanese   | 日本語      |
| ko   | Korean     | 한국어      |
| ru   | Russian    | Русский    |
| tr   | Turkish    | Türkçe     |
| ur   | Urdu       | اردو       |
| ta   | Tamil      | தமிழ்      |

---

### 🏥 Health Check

| Method | Endpoint  | Description                 | Auth   |
|--------|-----------|-----------------------------|--------|
| GET    | `/health` | Database connectivity check | Public |

---

## 👥 User Roles

| Role           | Description                        |
|----------------|------------------------------------|
| `Admin`        | Full system access                 |
| `Doctor`       | Medical records, appointments      |
| `Nurse`        | Patient care, records              |
| `Receptionist` | Appointments, patient registration |
| `Patient`      | View own records                   |

---

## 🌱 Seed Data

### Automatic Seeding (on first run)

The app automatically seeds:

- **10+ Departments** — Cardiology, Neurology, Orthopedics, Pediatrics, Dermatology, etc.
- **28+ Rooms** — General Wards, Private, Semi-Private, ICU, CCU, Operating Theatres
- **Admin User** — `admin@hospital.com` / `Admin@1234`

### API Seed Script (`seed_data.py`)

A comprehensive Python script populates all tables with realistic hospital data via the API:

```bash
pip install requests
python3 seed_data.py
```

> **Requires** the API to be running on `http://localhost:5104`.

The script seeds data in dependency order:

| #  | Table           | Records Created                              |
|----|-----------------|----------------------------------------------|
| 1  | Users           | 15 users (8 Doctors, 2 Nurses, 2 Receptionists, 3 Patients) |
| 2  | Departments     | 8 departments (creates missing, reuses existing) |
| 3  | Doctors         | 8 doctors with specializations + weekly schedules (Mon–Fri) |
| 4  | Rooms           | 12 rooms (creates missing, reuses existing)  |
| 5  | Patients        | 10 patients with full profiles               |
| 6  | Appointments    | 10 appointments scheduled for the coming week |
| 7  | Medical Records | 8 detailed records with diagnoses, prescriptions, and lab results |
| 8  | Admissions      | 4 active admissions (ICU, Private, General)  |

---

## 🔧 Configuration

| Setting                  | Location                      | Description                      |
|--------------------------|-------------------------------|----------------------------------|
| Database Connection      | `.env` / `appsettings.json`   | PostgreSQL connection string     |
| JWT Secret & Expiry      | `.env` / `appsettings.json`   | Token signing key and TTL (60m)  |
| Groq API Key             | `.env` → `GROQ_API_KEY`       | API key for AI report summaries  |
| Serilog Log Levels       | `appsettings.json`            | Minimum log level overrides      |
| CORS Policy              | `ServiceExtensions.cs`        | Allowed origins for frontend     |

### Environment Variables (`.env`)

```env
# PostgreSQL
POSTGRES_USER=postgres
POSTGRES_PASSWORD=changeme
POSTGRES_DB=hmsdb

# JWT
JWT_SECRET_KEY=replace-with-a-strong-secret-key-at-least-32-chars

# Groq AI
GROQ_API_KEY=your-groq-api-key-here
```

---

## 🐳 Docker

### Commands

```bash
# Start services (PostgreSQL + API)
docker compose up --build

# Stop services
docker compose down

# Reset database (wipe volumes)
docker compose down -v && docker compose up --build
```

### Services

| Service | Port | Description              |
|---------|------|--------------------------|
| `api`   | 5104 | HMS Backend API          |
| `db`    | 5433 | PostgreSQL 16 (Alpine)   |

### Architecture

```
┌─────────────────────────────────────────┐
│              Docker Compose             │
│                                         │
│  ┌──────────┐       ┌───────────────┐   │
│  │ postgres │◄──────│   api         │   │
│  │ :5433    │  EF   │   :5104       │   │
│  │          │ Core  │               │   │
│  └──────────┘       │  ASP.NET 8    │   │
│                     │  + Groq AI    │   │
│                     └───────────────┘   │
└─────────────────────────────────────────┘
```

---

## 📂 Project Structure

```
HMS/
├── HMS-Backend/               # ASP.NET Core 8 Web API
│   ├── API/                   # Controllers, Middleware, Extensions
│   ├── Application/           # DTOs, Interfaces, Services, Validators
│   ├── Domain/                # Entities, Enums
│   ├── Infrastructure/        # EF Core, Repositories, Auth, AI
│   ├── Program.cs             # Application entry point
│   ├── appsettings.json       # Configuration
│   └── HMS-Backend.csproj     # Project dependencies
├── HMS.sln                    # Solution file
├── Dockerfile                 # Multi-stage build (SDK → Runtime)
├── docker-compose.yml         # PostgreSQL + API orchestration
├── seed_data.py               # Python API seeder script
├── smoke_test.py              # API smoke test script
├── .env.example               # Environment variable template
├── .gitignore
└── README.md
```

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).
