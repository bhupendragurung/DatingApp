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
- Build: `dotnet build`
- Run API: `dotnet run --project src/Api` (health check at `/health`)
- Start database: `docker compose up -d db` (needs `.env`, copy from `.env.example`)
- Still to add: run client, test, add migration, seed.
- Add migration: `dotnet ef migrations add <Name> --project src/Infrastructure --startup-project src/Api`
- Apply migrations: `dotnet ef database update --project src/Infrastructure --startup-project src/Api`
- Environment note: currently on .NET 9 (`global.json`, `UseAppHost=false`) because the dev machine's Windows build (10.0.21996) can't run .NET 10 tooling (Docker Desktop works). Revert to .NET 10 after upgrading Windows.

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
### Patterns We Use
- Primary constructors for DI
- Records for DTOs and commands
- Result<T> pattern for error handling (no exceptions for flow control)
- File-scoped namespaces
- Always pass CancellationToken to async methods

### Patterns We DON'T Use (Never Suggest)
- Repository pattern (use EF Core directly)
- AutoMapper (write explicit mappings)
- Exceptions for business logic errors
- Stored procedures

## Validation
- All request validation in FluentValidation validators
- Validators auto-registered via assembly scanning
- Validation runs in Mediator pipeline behavior

## Conventions
- Errors: one consistent JSON shape via global exception handling; stack traces only in Development.
- Secrets: environment variables, user-secrets locally, App Service settings/Key Vault in prod.
- Naming: `PascalCase` C# types, DTOs suffixed `Dto`, async methods suffixed `Async`.
- Usernames are unique case-insensitively. Registration enforces age 18+.



## Git Workflow
- Branch naming: `feature/`, `bugfix/`, `hotfix/`
- Commit format: `type: description` (feat, fix, refactor, test, docs)
- Always create a branch before changes
- Run tests before committing
