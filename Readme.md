# 🔧 Mechanic Shop Management System

A backend **Mechanic Shop Management System** built with **ASP.NET Core** and designed around Clean Architecture, Domain-Driven Design, and CQRS.

The project models the core operations of a mechanic shop, including customers, vehicles, repair tasks, parts, and the relationships between them.

The main goal of the project is not simply to build CRUD endpoints, but to apply modern backend development and software architecture concepts in a realistic business domain.

> **Learning Note:**
> This project was guided by a **Metigator course**. The course provided the initial direction and architectural guidance, while the project was implemented and extended as a hands-on learning experience to deepen my understanding of the concepts and technologies involved.

---

## 🎯 Project Goals

This project was built to practice and apply:

* Clean Architecture
* Domain-Driven Design (DDD)
* CQRS
* MediatR
* Entity Framework Core
* FluentValidation
* Dependency Injection
* Authentication & Authorization
* Caching
* Structured Logging
* Cross-cutting concerns
* Domain modeling
* Business logic
* API design
* Database design

The focus throughout the project is on understanding **why a particular architectural or technical decision is made**, not simply implementing patterns for the sake of using them.

---

## 🏗️ Architecture

The solution follows **Clean Architecture**, separating the application into four main layers:

```text
                    ┌──────────────────────┐
                    │      API Layer       │
                    │ Controllers / HTTP   │
                    └──────────┬───────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │ Application Layer    │
                    │ CQRS / MediatR       │
                    │ Commands / Queries   │
                    │ Validation / DTOs    │
                    └──────────┬───────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │    Domain Layer      │
                    │ Entities / Business  │
                    │ Rules / Domain Logic │
                    └──────────────────────┘
                               ▲
                               │
                    ┌──────────┴───────────┐
                    │ Infrastructure Layer │
                    │ EF Core / Identity   │
                    │ Caching / Logging    │
                    └──────────────────────┘
```

### Domain

Contains the core business model and domain rules.

The domain is designed to be independent of infrastructure concerns.

### Application

Contains the application's use cases.

CQRS is used to separate:

* Commands — operations that change state
* Queries — operations that retrieve data

MediatR is used to dispatch requests to their corresponding handlers.

### Infrastructure

Contains implementation details such as:

* Entity Framework Core
* Database configuration
* ASP.NET Core Identity
* Authentication infrastructure
* Caching
* Logging
* External infrastructure concerns

### API

The API layer acts as the entry point into the application.

It is responsible for:

* HTTP endpoints
* Request/response handling
* Authentication configuration
* API configuration
* Dependency injection registration

---

## 🧩 Domain-Driven Design

The project applies several tactical DDD concepts.

The domain model is designed around the actual business concepts of a mechanic shop rather than treating the application as a collection of database tables.

Examples include:

* Customers
* Vehicles
* Repair Tasks
* Parts
* Business relationships between these entities
* Domain-level business rules

Entities encapsulate behavior where appropriate rather than exposing all state for arbitrary modification.

Factory methods and domain operations are used to keep object creation and business rules inside the domain.

---

## 🔀 CQRS

The application uses **Command Query Responsibility Segregation (CQRS)**.

Instead of treating every operation as a generic service method, the application separates operations into two categories.

### Commands

Commands represent operations that modify application state.

Examples:

```text
Create Customer
Update Customer
Delete Customer
Create Vehicle
Create Repair Task
Add Part
```

### Queries

Queries are responsible for retrieving information without modifying application state.

Examples:

```text
Get Customer
Get Customers
Get Vehicle
Get Repair Tasks
```

MediatR is used to dispatch these requests to their handlers.

---

## ⚙️ MediatR Pipeline Behaviors

Cross-cutting concerns are implemented through MediatR pipeline behaviors.

Examples include:

* Validation
* Performance monitoring
* Logging
* Exception handling

This keeps these concerns out of individual command and query handlers.

Conceptually:

```text
HTTP Request
     │
     ▼
   MediatR
     │
     ▼
┌───────────────┐
│ Validation    │
├───────────────┤
│ Performance   │
├───────────────┤
│ Logging       │
├───────────────┤
│ Exception     │
├───────────────┤
│ Handler       │
└───────────────┘
     │
     ▼
Application Result
```

---

## ✅ Validation

Request validation is handled using **FluentValidation**.

Validation is separated from the handlers so that business use cases remain focused on their actual responsibilities.

This also allows validation to be applied consistently through the MediatR pipeline.

---

## 🗄️ Entity Framework Core

The project uses **Entity Framework Core** for data access.

The application follows a Code First approach with:

* Entity configurations
* Relationships
* Migrations
* Database seeding
* LINQ queries
* Projection
* Change tracking
* Asynchronous database operations

The Application layer communicates with the database through an abstraction rather than depending directly on the concrete infrastructure implementation.

---

## 🔐 Authentication & Authorization

The application uses ASP.NET Core Identity and JWT-based authentication.

The security layer is responsible for:

* User authentication
* Identity management
* JWT token generation
* Authorization
* Access control

The current user is also exposed through an application abstraction so that application and infrastructure components can access the authenticated user's identity without directly depending on HTTP-specific concerns.

---

## ⚡ Caching

The project uses **HybridCache** for caching scenarios.

Caching is treated as an infrastructure concern so that application business logic does not become tightly coupled to a specific caching implementation.

The goal is to reduce unnecessary database access while maintaining a clean separation between business logic and infrastructure.

---

## 📊 Logging & Performance

Structured logging is implemented using **Serilog**.

The application also contains performance monitoring through a MediatR pipeline behavior.

This allows request execution time and other useful information to be captured without adding performance-measuring code to every handler.

---

## 🧱 Result Pattern

The application uses a `Result`-based approach for communicating operation outcomes.

Instead of relying exclusively on exceptions for expected business failures, operations can explicitly return success or failure results.

Conceptually:

```text
Operation
   │
   ├── Success → Result<T>
   │
   └── Failure → Error
```

This makes expected application and domain failures explicit and easier to handle consistently.

---

## 📁 Solution Structure

```text
MechanicShop/
│
├── MechanicShop.Domain/
│   ├── Entities/
│   ├── Common/
│   └── ...
│
├── MechanicShop.Application/
│   ├── Common/
│   │   ├── Behaviors/
│   │   ├── Interfaces/
│   │   └── ...
│   ├── Features/
│   │   ├── Customers/
│   │   ├── Vehicles/
│   │   ├── RepairTasks/
│   │   └── ...
│   └── ...
│
├── MechanicShop.Infrastructure/
│   ├── Data/
│   ├── Identity/
│   ├── Services/
│   └── ...
│
└── MechanicShop.Api/
    ├── Controllers/
    ├── Extensions/
    ├── Middleware/
    └── ...
```

> The exact structure may evolve as the project continues to develop.

---

## 🛠️ Technology Stack

| Technology                | Purpose                    |
| ------------------------- | -------------------------- |
| **C#**                    | Programming language       |
| **.NET / ASP.NET Core**   | Backend framework          |
| **Entity Framework Core** | ORM / data access          |
| **SQL Server**            | Relational database        |
| **MediatR**               | CQRS / request dispatching |
| **FluentValidation**      | Request validation         |
| **ASP.NET Core Identity** | Identity management        |
| **JWT**                   | Authentication             |
| **HybridCache**           | Application caching        |
| **Serilog**               | Structured logging         |
| **Swagger / OpenAPI**     | API documentation          |

---

## 🚀 Getting Started

### Prerequisites

Make sure you have:

* .NET SDK
* SQL Server
* Git

Clone the repository:

```bash
git clone https://github.com/Mahmoud-Mshrf/MechanicShop_Management_System.git
```

Navigate into the project:

```bash
cd MechanicShop_Management_System
```

Restore dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

Apply the database migrations:

```bash
dotnet ef database update
```

Run the API:

```bash
dotnet run --project MechanicShop.Api
```

Then open the Swagger UI using the URL displayed by ASP.NET Core.

---

## 🧪 Project Status

This project is an ongoing learning and portfolio project.

The architecture and implementation may continue to evolve as I deepen my understanding of:

* Domain-Driven Design
* Clean Architecture
* CQRS
* ASP.NET Core
* EF Core
* Distributed systems
* Performance
* Testing
* Production backend development

---

## 📚 Learning Resource

The project was **guided by a Metigator course**, which provided architectural direction and helped establish the foundation for exploring Clean Architecture, DDD, CQRS, and modern ASP.NET Core development.

The implementation is also used as a personal learning exercise to experiment with concepts, make architectural decisions, debug problems, and understand the trade-offs involved in building a real-world backend system.

---

## 👨‍💻 Author

**Mahmoud Mshrf**

.NET Backend Developer focused on building maintainable and scalable backend systems with C# and ASP.NET Core.

* GitHub: [Mahmoud-Mshrf](https://github.com/Mahmoud-Mshrf)

---

## ⭐ Purpose of This Repository

This project represents my transition from learning individual technologies to **combining them into a structured backend application**.

The most important goal is not the number of technologies used, but understanding how they work together:

```text
Domain
   ↓
Application
   ↓
Infrastructure
   ↓
API
```

while keeping business logic independent, application use cases explicit, and infrastructure concerns separated from the core domain.

---

**Built with C#, ASP.NET Core, Clean Architecture, DDD, and CQRS.**
