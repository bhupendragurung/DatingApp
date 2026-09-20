# Dating App

Portfolio dating app built fresh on the latest tech, in small vertical slices. Feature checklist and phases: `~/.claude/plans/understand-this-project-and-tidy-mitten.md` (running checklist, tick items off as they ship).

## Tech stack
Pin exact versions in this file when each piece is scaffolded.
- Backend: .NET 10 Web API, EF Core, ASP.NET Core Identity + JWT bearer
- Database: PostgreSQL (Azure Database for PostgreSQL in prod)
- Real-time: SignalR (presence + chat hubs)
- Frontend: React + TypeScript, Vite, React Router, TanStack Query, Vitest
- Photos: Azure Blob Storage
- Hosting: Azure App Service (API serves the built SPA, with SPA fallback routing)

## Structure (created as needed, not up front)
- `src/Api/` - controllers, hubs, middleware, DI setup
- `src/Domain/` - entities and business rules, no framework dependencies
- `src/Infrastructure/` - EF Core, migrations, Blob Storage, seed data
- `tests/` - unit and integration tests
- `client/` - React app

## Commands
Fill in once scaffolded: build, run API, run client, test, add migration, seed.

## Workflow (important)
- Work in small slices. Write a short plan for the slice, then implement only that.
- One branch per slice, named `feature/<short-name>` (or `fix/`, `chore/`).
- Small commits with clear messages. Commit or push only when the user asks.
- The app must build and run after every slice. Never scaffold later phases early.
- Add tests for core logic as it lands (token issuance, pagination/filtering, message soft/hard delete).

## Architecture rules
- Domain must not reference Infrastructure or Api.
- Controllers stay thin: validate, call a service, map to a DTO. Never return entities.
- Use async/await with `CancellationToken` for I/O.
- Forbidden: secrets in source or `appsettings.json`, `DateTime.Now` (use UTC), business logic in controllers.

## Conventions
- Errors: one consistent JSON shape via global exception handling; stack traces only in Development.
- Secrets: environment variables, user-secrets locally, App Service settings/Key Vault in prod.
- Naming: `PascalCase` C# types, DTOs suffixed `Dto`, async methods suffixed `Async`.
- Usernames are unique case-insensitively. Registration enforces age 18+.

## Known bugs to avoid repeating
- Gender filter defaults to the opposite gender (original had a `"fale"` typo).
- Like button must work on both member card and member detail.
- Message is hard-deleted only after BOTH sender and recipient have deleted it.
- Never allow deleting the current main photo or removing the seed admin's Admin role.

## Glossary
Member (user profile), Like, Message (soft-deleted per side), Presence (online/offline, multi-connection), Main photo, Moderator/Admin roles.

## Out of scope (unless asked)
Photo moderation workflow (admin tab is a stub), mutual matches, unlike, blocking, typing indicator, multi-instance presence backplane.
