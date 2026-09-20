# Dating App

A full-stack dating app built in small phases. See [docs/PROJECT_SCOPE.md](docs/PROJECT_SCOPE.md) and [docs/IMPLEMENTATION_PLAN.md](docs/IMPLEMENTATION_PLAN.md).

## Prerequisites
- .NET SDK (see `global.json`)
- Docker Desktop (for PostgreSQL)

## Run locally
```bash
cp .env.example .env          # then edit the password
docker compose up -d db       # start PostgreSQL on localhost:5432
dotnet run --project src/Api  # start the API
```
Check it: `curl http://localhost:5000/health` (the port is printed on startup).

Stop the database: `docker compose down` (add `-v` to delete its data).

## Notes
- Secrets live in `.env` (git-ignored) or user-secrets, never in source.
