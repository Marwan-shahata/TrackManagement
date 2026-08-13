# Track Management

A full-stack Track Management application for managing artists, music tracks, and distribution statuses across Digital Service Providers (DSPs) such as Spotify, Apple Music, and YouTube.

The application is built with:

* .NET 10 Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* Scalar API Reference
* Angular
* Clean Architecture

## Project Structure

```text
TrackManagement/
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
├── README.md
└── DECISIONS.md
```

## Backend Architecture

The backend follows a simplified Clean Architecture structure:

* **Domain** — entities and enums
* **Application** — DTOs, service interfaces, repository interfaces, and business logic
* **Infrastructure** — EF Core, repositories, database configuration, migrations, seeding, and JWT token generation
* **API** — controllers, dependency injection, authentication, exception handling, OpenAPI, and Scalar

## Prerequisites

Install the following:

* .NET 10 SDK
* SQL Server
* Node.js and npm
* Angular CLI

Check the installed versions:

```bash
dotnet --version
node --version
npm --version
ng version
```

## Backend Setup

Navigate to the repository root and restore the backend dependencies:

```bash
dotnet restore ./Backend/TrackManagement.slnx
```

Update the SQL Server connection string in:

```text
Backend/TrackManagement.API/appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=TrackManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Adjust the SQL Server instance name if required.

## Database Migrations

EF Core migrations are included in the repository.

The API automatically applies pending migrations when the application starts.

To apply migrations manually, navigate to the repository root and run:

```bash
dotnet ef database update \
  --project ./Backend/TrackManagement.Infrastructure \
  --startup-project ./Backend/TrackManagement.API
```

If the EF CLI tool is not installed:

```bash
dotnet tool install --global dotnet-ef
```

The application also automatically seeds sample data including:

* 3 artists
* 8 tracks across different genres and statuses
* 3 DSPs
* Sample track distribution records

The DSP seed data includes Spotify, Apple Music, and YouTube.

## Run the Backend

From the repository root:

```bash
dotnet run --project ./Backend/TrackManagement.API
```

The terminal will display the HTTP and HTTPS URLs used by the API.

The Scalar API reference is available at:

```text
https://localhost:<HTTPS_PORT>/scalar
```

The generated OpenAPI document is available at:

```text
https://localhost:<HTTPS_PORT>/openapi/v1.json
```

## JWT Authentication

The endpoint below is protected with JWT authentication:

```http
PATCH /api/tracks/{id}/status
```

A development token can be obtained using:

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

A successful request returns an access token.

Example:

```json
{
  "accessToken": "eyJ...",
  "expiresAt": "..."
}
```

Use the token as a Bearer token:

```http
Authorization: Bearer <access_token>
```

The hard-coded development credentials are included only to demonstrate JWT authentication for this coding task. A production system should use a proper identity provider or user store and should never store production credentials or signing secrets in source control.

## API Endpoints

### Artists

```http
POST /api/artists
GET  /api/artists
```

### Tracks

```http
POST  /api/tracks
GET   /api/tracks
GET   /api/tracks/{id}
POST  /api/tracks/{id}/distribute
PATCH /api/tracks/{id}/status
```

Track filtering is supported using:

```http
GET /api/tracks?artistId=1
GET /api/tracks?genre=Rock
GET /api/tracks?status=Draft
```

Filters can also be combined.

## Frontend Setup

Navigate to the Angular project:

```bash
cd Frontend/track-management-ui
```

Install dependencies:

```bash
npm install
```

Configure the backend API URL in:

```text
src/environments/environment.ts
```

Example:

```ts
export const environment = {
  apiUrl: 'https://localhost:<HTTPS_PORT>/api'
};
```

Use the same HTTPS port displayed when the .NET API starts.

Run Angular:

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

## Frontend Features

### Track List

The Track List displays:

* Track title
* Artist name
* Genre
* Release date
* Track status

Tracks can be filtered by:

* All
* Draft
* Submitted
* Distributed

### Track Detail

Selecting a track opens its detail page:

```text
/tracks/{id}
```

The page displays:

* Title
* Artist
* ISRC
* Genre
* Release date
* Track status
* DSP distribution information
* DSP distribution status

## Build Verification

Backend:

```bash
dotnet build ./Backend/TrackManagement.slnx
```

Frontend:

```bash
cd Frontend/track-management-ui
ng build
```

Both commands should complete successfully before running or submitting the project.
