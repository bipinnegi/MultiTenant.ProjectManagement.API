🚀 Project Management REST API
ASP.NET Core | Azure | CI/CD | Secure Backend System

A production-ready Project Management REST API built using ASP.NET Core that demonstrates real-world backend engineering practices including authentication, authorization, cloud deployment, and CI/CD automation.

This project is designed to mirror how APIs are built in professional software teams, not tutorial-style demos.

🧠 Overview

This API enables secure project and task management for multiple users using JWT authentication and role-based access control.
It is fully deployed on Microsoft Azure and integrated with GitHub Actions for CI/CD, making it a complete end-to-end backend system.

🎯 Key Objectives

Build a secure ASP.NET Core REST API

Follow clean architecture & separation of concerns

Implement industry-standard authentication

Deploy to cloud with CI/CD

Demonstrate backend skills expected from professional developers

✨ Features

🔐 JWT Authentication & Authorization

🧑‍💼 Role-Based Access Control (Admin / User)

📁 Project Management (CRUD)

✅ Task Management (CRUD)

🔒 Fully Protected API Endpoints

🗄️ Entity Framework Core + Azure SQL

☁️ Cloud Deployment on Microsoft Azure

🔄 Automated CI/CD using GitHub Actions

🧪 Fully Tested using Postman

🛠️ Tech Stack
Backend

ASP.NET Core Web API

C#

Entity Framework Core

Security

JWT (JSON Web Tokens)

Role-Based Authorization

Database

Azure SQL Database

SQL Server

DevOps & Tools

GitHub

GitHub Actions (CI/CD)

Postman

Visual Studio

🏗️ Architecture (Clean & Scalable)

The project follows a layered architecture to ensure maintainability and scalability.

┌──────────────────────────┐
│        API Layer         │
│  (Controllers / Routes)  │
└─────────────▲────────────┘
              │
┌─────────────┴────────────┐
│    Business Logic Layer  │
│       (Services)         │
└─────────────▲────────────┘
              │
┌─────────────┴────────────┐
│   Data Access Layer      │
│  (Repositories / EF)    │
└─────────────▲────────────┘
              │
┌─────────────┴────────────┐
│      Azure SQL DB        │
└──────────────────────────┘

Why this architecture?

Clear separation of concerns

Easy testing and debugging

Scales well for enterprise applications

🔐 Authentication & Authorization Flow
Client
  │
  ├── POST /login
  │       │
  │       ├── Validate credentials
  │       └── Generate JWT
  │
  ├── Request with JWT
  │       │
  │       ├── Token validation middleware
  │       ├── Role authorization
  │       └── Access controller
  │
  └── Secure API Response

Key Points:

JWT is sent via Authorization: Bearer <token>

Middleware validates token & role

Unauthorized users are blocked automatically

🔁 API Request Flow (Example)
Client (Postman / Frontend)
        │
        ▼
API Controller
        │
        ▼
Service Layer (Business Logic)
        │
        ▼
Repository (EF Core)
        │
        ▼
Azure SQL Database

📬 API Endpoints (Sample)
Authentication

POST /api/auth/register

POST /api/auth/login

Projects

GET /api/projects

POST /api/projects

PUT /api/projects/{id}

DELETE /api/projects/{id}

Tasks

GET /api/tasks

POST /api/tasks

PUT /api/tasks/{id}

DELETE /api/tasks/{id}

🔒 All endpoints (except login/register) are JWT protected.

🧪 Testing

All APIs tested using Postman

Covered:

Auth success/failure

Role-based access

CRUD operations

Unauthorized access handling

📌 Postman collection available on request.

☁️ Deployment & CI/CD
Deployment

Hosted on Microsoft Azure

Uses Azure App Services

Connected to Azure SQL Database

CI/CD

GitHub Actions pipeline:

Builds project on every push

Validates code

Deploys automatically to Azure

This reflects real DevOps workflows used in production teams.

🚀 Why This Project Stands Out

This project demonstrates:

Practical ASP.NET Core backend skills

Secure API development

Cloud & CI/CD knowledge

Professional architecture

Industry-ready coding standards

This is not a tutorial project — it represents how backend systems are built in real companies.

👨‍💻 About the Developer

Bipin
Computer Science & Engineering Student

Skills

ASP.NET Core

REST APIs

C#

Azure & Azure SQL

GitHub & CI/CD

Postman

Angular / React (Frontend)

🎯 Actively seeking ASP.NET Core / Backend Internship opportunities

🔗 Project Links

GitHub Repository: add link

Live API (Azure): add link

🧭 Future Enhancements

Multi-Tenant Support

Refresh Tokens

Rate Limiting

Centralized Logging

Swagger UI Enhancements

⭐ Final Note for Reviewers

If you are reviewing this project as a recruiter or senior developer:

This project reflects real backend engineering capability, not academic-level work.
