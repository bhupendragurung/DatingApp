# Project Scope: Dating App

## 1. Overview
A full-stack dating web application built as a portfolio project on the latest tech. Users register, build a profile with photos, browse and like other members, message them, and chat in real time. Admins manage users and roles.

The project is delivered in small vertical slices, each on its own branch and each leaving a working, demoable product.

## 2. Goals
- Ship a working app after every phase, not a big-bang release.
- Show real-world engineering practice: feature branches, small commits, tests for core logic, secrets kept out of source, and deployment.
- Demonstrate a modern full-stack: .NET 10 API, React SPA, real-time with SignalR, and cloud hosting on Azure.

## 3. Tech stack
| Area | Choice |
|---|---|
| Backend | .NET 10 Web API, EF Core, ASP.NET Core Identity + JWT |
| Database | PostgreSQL (Azure Database for PostgreSQL in production) |
| Real-time | SignalR |
| Frontend | React + TypeScript, Vite, React Router, TanStack Query, Vitest |
| Photo storage | Azure Blob Storage |
| Hosting | Azure App Service (API serves the built SPA) |
| Secrets | user-secrets locally, App Service settings or Key Vault in production |

## 4. In scope

### Accounts and auth
- Register: username, known-as, gender, date of birth (18+ enforced), city, country, password.
- Login with a token carrying user id, username and roles.
- Roles: Member (default), Moderator, Admin.
- Usernames are unique, case-insensitively.

### Member profiles and browsing
- Browse members with filters (gender, min/max age), sort (last active, newest) and pagination.
- Default filter shows the opposite gender.
- View a full member profile.
- Edit own profile (introduction, looking-for, interests, city, country) with an unsaved-changes warning.
- "Last active" updates on each authenticated request.

### Photos
- Upload photos to Azure Blob Storage; the first photo becomes the main photo.
- Set any photo as main.
- Delete a non-main photo (removes it from the database and storage). The main photo cannot be deleted.
- Photo gallery on the profile.

### Likes
- Like another member (no self-like, no duplicates).
- Lists: members I liked, members who liked me, paginated.
- Like button works on both the member card and the member detail page.

### Messaging
- Send a message (no self-messaging).
- Inbox, outbox and unread lists, paginated.
- Delete a message per side; it is permanently removed only after both sides have deleted it.

### Real-time
- Presence: online/offline events, supporting multiple connections per user.
- Live chat per conversation, with read receipts and a new-message notification when the recipient is online but not viewing the thread.
- Token authentication for the SignalR connection.

### Admin
- List users with their roles.
- Edit a user's roles. The seed admin's Admin role cannot be removed.
- Photo moderation tab exists as a stub only.

### Cross-cutting
- Global exception handling with one JSON error shape (stack traces in Development only).
- CORS for local development.
- Client: global loading indicator, central HTTP error handling, token attached to requests.
- Seed data: roles, an admin account and sample members, loaded from a data file.
- Secrets never committed to source.
- CI pipeline and deployment to Azure App Service.

## 5. Out of scope (unless requested later)
- Photo moderation and approval workflow
- Mutual-match detection, unlike/toggle, blocking and reporting
- Typing indicator
- Multi-instance presence (backplane); the app runs on a single instance
- Mobile apps, payments, email/SMS notifications, social login
- Face-aware photo auto-crop (to be decided in the Photos phase)

## 6. Phases
Each phase is one or more small branches and ends with a demo.

| # | Phase | Demo |
|---|---|---|
| 0 | Setup: repo, API scaffold, health endpoint | API runs and `/health` responds |
| 1 | Foundations: schema, register/login, JWT, roles, seed data | Register, log in, call a protected endpoint |
| 2 | Member profiles and browsing | Browse, filter, view and edit profiles |
| 3 | Photos | Upload, set main, delete, gallery |
| 4 | Likes | Like from both surfaces, view lists |
| 5 | Messaging (REST) | Send, list, delete messages |
| 6 | Real-time | Presence, live chat, read receipts |
| 7 | Admin | Manage users and roles |
| 8 | Polish and deployment | Live on Azure with CI |

## 7. Ways of working
- One branch per small slice: `feature/<name>`, `fix/<name>`, `chore/<name>`.
- Short plan before each slice; small commits with clear messages.
- The app builds and runs after every slice.
- Automated tests for core logic: token issuance, pagination and filtering, message deletion rules.
- `CLAUDE.md` is kept current at the end of each phase.

## 8. Success criteria
- Every in-scope feature above works end to end and is deployed on Azure.
- No secrets in source control.
- Core logic has automated tests, and CI passes on the main branch.
- Git history shows small, reviewable changes per slice.

## 9. Risks and open decisions
- **Local environment:** the .NET 10 compiler does not run on the current Windows build (10.0.21996). Options are upgrading Windows or building in Docker.
- **Photo cropping:** Azure Blob Storage has no face-aware crop, so choose client-side, server-side center-crop or none.
- **Auth transport:** JWT bearer is the default; httpOnly cookies are safer against XSS but complicate the SignalR handshake.
- **Presence scaling:** in-memory presence works on one instance only; adopt Azure SignalR Service if scaling out.
