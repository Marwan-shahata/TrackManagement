# Track Management

A full-stack Track Management application for managing artists, music tracks, and track distribution across Digital Service Providers (DSPs).

The project was built as a full-stack coding challenge using **.NET 10** for the backend and **Angular** for the frontend.

## Tech Stack

### Backend

- .NET 10 Web API
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- OpenAPI
- Scalar API Reference
- Clean Architecture

### Frontend

- Angular
- TypeScript
- Angular Router
- HttpClient
- CSS

---

## Project Structure

```text
TrackManagement/
│
├── Backend/
│   ├── TrackManagement.API/
│   ├── TrackManagement.Application/
│   ├── TrackManagement.Domain/
│   ├── TrackManagement.Infrastructure/
│   └── TrackManagement.slnx
│
├── Frontend/
│   └── track-management-ui/
│
├── .gitignore
├── README.md
└── DECISIONS.md
```

## Backend Architecture

The backend follows a simplified Clean Architecture approach.

### TrackManagement.Domain

Contains the core domain models and enums, including:

- Artist
- Track
- DSP
- TrackDistribution
- TrackStatus
- DistributionStatus

### TrackManagement.Application

Contains application-level logic and contracts:

- DTOs
- Repository interfaces
- Service interfaces
- Track and artist business logic

### TrackManagement.Infrastructure

Contains infrastructure and persistence concerns:

- Entity Framework Core
- ApplicationDbContext
- Entity configurations
- Repository implementations
- Database migrations
- Automatic database seeding
- JWT token generation

### TrackManagement.API

Contains the HTTP/API layer:

- API controllers
- Dependency injection configuration
- JWT authentication
- Authorization
- CORS configuration
- Global exception handling
- OpenAPI configuration
- Scalar API reference

---

## Prerequisites

Before running the project, install:

- .NET 10 SDK
- SQL Server / SQL Server Express
- Node.js
- npm
- Angular CLI

You can verify the installed tools using:

```bash
dotnet --version
node --version
npm --version
ng version
```

---

# Backend Setup

## 1. Restore Backend Dependencies

From the repository root:

```bash
dotnet restore ./Backend/TrackManagement.slnx
```

## 2. Configure SQL Server

Open:

```text
Backend/TrackManagement.API/appsettings.json
```

Configure the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=TrackManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace:

```text
YOUR_SQL_SERVER
```

with your local SQL Server instance.

For example, a SQL Server Express instance may look like:

```text
DESKTOP-NAME\SQLEXPRESS
```

The database name used by the application is:

```text
TrackManagementDb
```

---

## 3. Configure JWT Signing Key

The JWT signing key is intentionally not stored in source control.

Navigate to the API project:

```bash
cd Backend/TrackManagement.API
```

Initialize .NET User Secrets if required:

```bash
dotnet user-secrets init
```

Set a development JWT signing key:

```bash
dotnet user-secrets set "Jwt:Key" "YOUR-DEVELOPMENT-JWT-KEY"
```

Use a sufficiently long development key.

You can verify the configured secret using:

```bash
dotnet user-secrets list
```

The remaining JWT configuration is stored in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "",
    "Issuer": "TrackManagement.API",
    "Audience": "TrackManagement.Client",
    "ExpiryMinutes": 60
  }
}
```

The signing key should not be committed to source control.

For a production application, secrets should be provided using an appropriate secret-management solution or environment configuration.

---

# Database Migrations

EF Core migrations are included in the repository.

The API automatically applies pending migrations when the application starts.

Therefore, under normal development setup, simply starting the API will create/update the database as required.

To apply migrations manually from the repository root:

```bash
dotnet ef database update --project ./Backend/TrackManagement.Infrastructure --startup-project ./Backend/TrackManagement.API
```

If the EF Core CLI tool is not installed:

```bash
dotnet tool install --global dotnet-ef
```

---

# Database Seeding

Sample data is automatically seeded during application startup.

The seed data contains at least:

- 3 Artists
- 8 Tracks
- Multiple genres
- Multiple track statuses
- 3 DSPs
- Sample track distribution records

The DSPs include:

- Spotify
- Apple Music
- YouTube

No manual SQL script is required to insert the initial development data.

---

# Run the Backend

From the repository root:

```bash
dotnet run --project ./Backend/TrackManagement.API
```

The application will display its HTTP/HTTPS addresses in the terminal.

For example:

```text
https://localhost:<HTTPS_PORT>
http://localhost:<HTTP_PORT>
```

Use the actual ports displayed by the application.

---

# Scalar API Reference

When running in the Development environment, the Scalar API reference is available at:

```text
https://localhost:<HTTPS_PORT>/scalar
```

The generated OpenAPI document is available at:

```text
https://localhost:<HTTPS_PORT>/openapi/v1.json
```

Scalar can be used to inspect and test the API endpoints.

---

# API Endpoints

## Artists

### Create Artist

```http
POST /api/artists
```

Example request:

```json
{
  "name": "John Artist",
  "email": "john@example.com"
}
```

### Get Artists

```http
GET /api/artists
```

---

## Tracks

### Create Track

```http
POST /api/tracks
```

### Get Tracks

```http
GET /api/tracks
```

Tracks can be filtered by artist, genre, or status.

Examples:

```http
GET /api/tracks?artistId=1
GET /api/tracks?genre=Rock
GET /api/tracks?status=Draft
```

Filters can also be combined.

### Get Track Details

```http
GET /api/tracks/{id}
```

The track detail response includes its DSP distribution information and distribution statuses.

### Distribute Track

```http
POST /api/tracks/{id}/distribute
```

Example:

```json
{
  "dspIds": [1, 2]
}
```

A track can be submitted to one or more DSPs.

### Update Track Status

```http
PATCH /api/tracks/{id}/status
```

Example:

```json
{
  "status": "Distributed"
}
```

This endpoint is protected using JWT authentication.

---

# JWT Authentication

The following endpoint requires authentication:

```http
PATCH /api/tracks/{id}/status
```

## Obtain a Development Token

Send:

```http
POST /api/auth/token
```

Request body:

```json
{
  "username": "admin",
  "password": "Admin123!"
}
```

A successful request returns:

```json
{
  "accessToken": "eyJ...",
  "expiresAt": "..."
}
```

Use the returned token as a Bearer token:

```http
Authorization: Bearer <access_token>
```

For example, when using Scalar, obtain the token first and use it in the Bearer authentication field before calling the protected endpoint.

The username and password above are demo credentials used only to demonstrate JWT authentication for this coding challenge.

A production implementation should use a proper identity provider or user store instead of hard-coded development credentials.

---

# Error Handling and Validation

The API performs validation and returns meaningful HTTP responses.

Examples include:

```text
400 Bad Request
```

for invalid input or invalid references.

```text
401 Unauthorized
```

when accessing the protected endpoint without a valid JWT.

```text
404 Not Found
```

when a requested resource cannot be found.

```text
409 Conflict
```

for conflicting operations such as duplicate ISRC values or duplicate track distributions.

Unexpected application errors are handled centrally by the global exception handler.

---

# Frontend Setup

Navigate to the Angular project:

```bash
cd Frontend/track-management-ui
```

Install dependencies:

```bash
npm install
```

---

## Configure Backend URL

Open:

```text
src/environments/environment.ts
```

Configure the API URL using the HTTPS port of the .NET API:

```typescript
export const environment = {
  apiUrl: 'https://localhost:<HTTPS_PORT>/api'
};
```

For example:

```typescript
export const environment = {
  apiUrl: 'https://localhost:7298/api'
};
```

The port must match the HTTPS port displayed when the backend starts.

---

# Run the Frontend

From:

```text
Frontend/track-management-ui
```

run:

```bash
ng serve
```

Open:

```text
http://localhost:4200
```

The application redirects to:

```text
http://localhost:4200/tracks
```

The backend should also be running while using the frontend.

---

# Frontend Features

## Track List

The Track List page displays:

- Track title
- Artist name
- Genre
- Release date
- Track status

Tracks can be filtered by status:

- All
- Draft
- Submitted
- Distributed

Selecting a track opens its detail page.

---

## Track Detail

Track details are available at:

```text
/tracks/{id}
```

The page displays:

- Track title
- Artist
- ISRC
- Genre
- Release date
- Track status
- DSP name
- DSP submission information
- Distribution status

Tracks without DSP distributions display an appropriate empty-state message.

---

# CORS

The backend allows requests from the Angular development server:

```text
http://localhost:4200
```

This configuration is intended for local development.

---

# Build Verification

Before submitting or running the project, both applications can be verified independently.

## Backend

From the repository root:

```bash
dotnet build ./Backend/TrackManagement.slnx
```

Expected result:

```text
Build succeeded.
0 Error(s)
```

## Frontend

Navigate to:

```bash
cd Frontend/track-management-ui
```

Then:

```bash
ng build
```

The Angular production build should complete without compilation errors.

---

# Running the Complete Application

A typical local development workflow is:

### Terminal 1 — Backend

From the repository root:

```bash
dotnet run --project ./Backend/TrackManagement.API
```

### Terminal 2 — Frontend

```bash
cd Frontend/track-management-ui
ng serve
```

Then open:

```text
http://localhost:4200/tracks
```

---

# Development Notes

- EF Core migrations are committed to the repository.
- Pending migrations are automatically applied on API startup.
- Development data is automatically seeded.
- JWT is used to protect the track status update endpoint.
- The JWT signing key is kept outside source control using .NET User Secrets.
- Scalar is used as the interactive API reference.
- The Angular frontend communicates with the .NET API through HttpClient.
- The frontend is organized by feature, with track-specific models, services, list, and detail components.