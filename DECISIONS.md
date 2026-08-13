# Development Decisions

This document summarizes how AI tools were used during the implementation, the main security considerations, and an example where an AI-generated suggestion required correction.

## 1. What did the AI generate, and what did I change?

AI tools like chat GPT and Claude were used as a development assistant throughout the task rather than to generate the entire project in one step.

They helped with:

- Entity Framework Core configuration and repository/service patterns.
- Some of API endpoint implementation.
- Global exception handling.
- Track list and track detail UI.
- Debugging suggestions during backend/frontend integration.
- README structure and documentation.

I reviewed and tested the generated suggestions incrementally before continuing to the next feature.

Several parts were adjusted during implementation.

For example, the Angular frontend was organized by feature:

```text
features/
└── tracks/
    ├── models/
    ├── services/
    ├── track-list/
    └── track-detail/
```

instead of placing all components and services into global folders.

The API URL was also moved from a hard-coded value inside `TrackApiService` to:

```text
src/environments/environment.ts
```

so the API configuration is separated from the service implementation.

I also added and verified automatic EF Core migrations and database seeding so the application can initialize its development database when the API starts.

Each major feature was tested independently before moving to the next one.

---

## 2. What security issues did I find or introduce, and how did I handle them?

### JWT signing key

The initial JWT implementation stored the development signing key directly in `appsettings.json`.

Although this worked locally, committing a signing secret to source control is not appropriate.

I removed the actual signing key from the committed configuration and used .NET User Secrets for local development:

```bash
dotnet user-secrets set "Jwt:Key" "YOUR-DEVELOPMENT-JWT-KEY"
```

The application still keeps non-sensitive JWT settings such as issuer, audience, and expiration time in `appsettings.json`.

In a production environment, the signing key should be provided by an appropriate secrets management mechanism.

### Demo credentials

The token endpoint uses hard-coded development credentials:

```text
admin
Admin123!
```

This was intentionally kept simple because the task requires JWT protection but does not require a complete user management or identity system.

These credentials are only suitable for demonstrating authentication in this coding task.

A production system should use a proper identity provider or secure user store with appropriately hashed passwords.

### CORS

CORS is restricted to the Angular development origin:

```text
http://localhost:4200
```

instead of allowing requests from every origin.

### Input validation

API inputs are validated before processing, including cases such as:

- Invalid artist references.
- Invalid track statuses.
- Empty DSP selections.
- Invalid DSP references.
- Duplicate ISRC values.
- Duplicate track distributions.

The API returns meaningful HTTP status codes such as `400`, `401`, `404`, and `409`.

---

## 3. Example of something the AI got wrong

One issue occurred while configuring the Angular application.

The initial configuration used:

```typescript
provideZoneChangeDetection({ eventCoalescing: true })
```

However, the generated Angular project was configured to run without Zone.js.

This caused the application to fail at runtime with:

```text
NG0908: In this configuration Angular requires Zone.js
```

Instead of adding Zone.js just to make the suggested configuration work, I checked the generated Angular configuration and removed `provideZoneChangeDetection`.

The application then started correctly using its zoneless configuration.

A second related issue appeared when track data was loaded asynchronously. The API request succeeded, but the UI did not immediately refresh until another user interaction occurred.

Because the application was running zoneless, I explicitly notified Angular after the HTTP response using:

```typescript
ChangeDetectorRef.markForCheck()
```

I verified the fix by changing the track status filter repeatedly and confirming that the displayed results updated immediately without requiring an additional click.

This was a useful example of why AI-generated code still needs to be understood, tested, and adapted to the actual framework version and project configuration.

---

## Summary

AI was useful for accelerating implementation and suggesting common patterns, but its output was treated as a starting point rather than accepted automatically.

The development process was:

```text
Implement
→ Build
→ Test
→ Inspect behavior
→ Correct when necessary
→ Commit
```

This approach helped keep the implementation aligned with the task requirements while still verifying framework behavior, security decisions, and integration between the Angular frontend and .NET backend.