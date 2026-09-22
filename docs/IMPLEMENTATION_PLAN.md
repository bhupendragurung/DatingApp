# Implementation Plan

Companion to [PROJECT_SCOPE.md](PROJECT_SCOPE.md). Each task is one small branch (`feature/`, `fix/`, `chore/`) with small commits. After every task the app builds, runs and the checklist below is ticked. Tests are written with the task they cover.

Task ids: `P<phase>.<n>`. Branch names are suggestions.

## Phase 0: Setup
Goal: repo and a running empty API.
- [x] P0.1 `chore/project-setup`: git repo, `.gitignore`, `CLAUDE.md`
- [x] P0.2 `docs/project-scope`: project scope
- [x] P0.3 `feature/api-scaffold`: API + solution, `/health` endpoint (on .NET 9 for now, move to .NET 10 after the Windows upgrade)
- [x] P0.4 `chore/dev-environment`: Docker Compose for PostgreSQL, README with run instructions, fill in CLAUDE.md commands
- [x] P0.5 `chore/ci-build`: GitHub Actions workflow that builds and tests on every push

Demo: `dotnet run` and `/health` returns Healthy; CI is green.

## Phase 1: Foundations (accounts and auth)
Goal: register, log in, call a protected endpoint.
- [x] P1.1 `feature/db-setup`: EF Core + PostgreSQL, `AppDbContext`, first migration
- [x] P1.2 `feature/identity-model`: `AppUser` and `AppRole` with Identity tables
- [ ] P1.3 `feature/error-handling`: global exception handler, one JSON error shape, stack trace in Development only
- [ ] P1.4 `feature/auth-register`: register endpoint, age 18+ and case-insensitive unique username validation, assign Member role
- [ ] P1.5 `feature/auth-login`: login endpoint, JWT with id, username and role claims
- [ ] P1.6 `feature/authorization-policies`: Admin and Moderator policies, a protected test endpoint
- [ ] P1.7 `feature/seed-data`: seed roles and the admin account from a data file; secrets via user-secrets
- [ ] P1.8 `feature/auth-tests`: tests for register validation and token issuance

Demo: register, log in, call a protected endpoint with the token; a Member gets 403 on an admin endpoint.

## Phase 2: Frontend foundation and member profiles
Goal: a usable React app you can sign in to and browse members.
- [ ] P2.1 `feature/client-scaffold`: Vite + React + TypeScript, routing, layout, Vitest
- [ ] P2.2 `feature/client-auth`: register and login pages, token storage, route guards, request client that attaches the token
- [ ] P2.3 `feature/client-feedback`: global loading indicator and central HTTP error handling
- [ ] P2.4 `feature/member-fields`: profile fields on the user (introduction, looking-for, interests, city, country), migration, more seed members
- [ ] P2.5 `feature/members-list-api`: paginated list with gender/age filters, sort, opposite-gender default
- [ ] P2.6 `feature/members-list-ui`: member cards, filter form, pagination
- [ ] P2.7 `feature/member-detail`: single profile endpoint and page
- [ ] P2.8 `feature/profile-edit`: edit own profile endpoint and form, unsaved-changes warning
- [ ] P2.9 `feature/last-active`: update last-active on authenticated requests
- [ ] P2.10 `feature/members-tests`: tests for pagination, filtering and default gender

Demo: register in the browser, browse and filter members, view and edit your profile.

## Phase 3: Photos
Goal: upload and manage photos.
- [ ] P3.1 `feature/photo-model`: photo entity and migration
- [ ] P3.2 `feature/blob-storage`: Azure Blob Storage service (Azurite locally)
- [ ] P3.3 `feature/photo-upload`: upload endpoint, first photo becomes main
- [ ] P3.4 `feature/photo-main-delete`: set main, delete non-main, remove from storage
- [ ] P3.5 `feature/photo-ui`: upload widget, manage photos, gallery on profile
- [ ] P3.6 `feature/photo-tests`: tests for main-photo and delete rules

Demo: upload photos, change the main photo, delete one, see the gallery.
Decision needed in this phase: photo cropping approach.

## Phase 4: Likes
- [ ] P4.1 `feature/like-model`: like entity and migration
- [ ] P4.2 `feature/like-api`: like endpoint (no self-like, no duplicates), liked and liked-by lists with pagination
- [ ] P4.3 `feature/like-ui`: like button on card and detail page, likes page
- [ ] P4.4 `feature/like-tests`

Demo: like from both places, see both lists.

## Phase 5: Messaging (REST)
- [ ] P5.1 `feature/message-model`: message entity and migration
- [ ] P5.2 `feature/message-send`: send endpoint (no self-messaging)
- [ ] P5.3 `feature/message-lists`: inbox, outbox, unread with pagination
- [ ] P5.4 `feature/message-delete`: per-side delete, permanent removal once both sides deleted
- [ ] P5.5 `feature/message-ui`: messages page and send form on a profile
- [ ] P5.6 `feature/message-tests`: tests for the delete rules

Demo: send messages between two users, delete from each side.

## Phase 6: Real-time
- [ ] P6.1 `feature/signalr-setup`: hub infrastructure, token authentication for connections
- [ ] P6.2 `feature/presence-hub`: online/offline tracking with multiple connections per user
- [ ] P6.3 `feature/presence-ui`: online indicators
- [ ] P6.4 `feature/chat-hub`: per-conversation group, live delivery, save messages
- [ ] P6.5 `feature/read-receipts`: mark read when both users are in the conversation
- [ ] P6.6 `feature/new-message-notify`: notify recipient online but not viewing the thread
- [ ] P6.7 `feature/chat-ui`: live chat panel
- [ ] P6.8 `feature/realtime-tests`

Demo: two browsers chat live with presence and read receipts.

## Phase 7: Admin
- [ ] P7.1 `feature/admin-users-api`: list users with roles
- [ ] P7.2 `feature/admin-roles-api`: edit roles, protect the seed admin
- [ ] P7.3 `feature/admin-ui`: users and roles page, moderation tab stub
- [ ] P7.4 `feature/admin-tests`

Demo: an admin changes another user's roles.

## Phase 8: Polish and deployment
- [ ] P8.1 `chore/cors-config`: CORS for development
- [ ] P8.2 `chore/serve-spa`: API serves the built React app with SPA fallback
- [ ] P8.3 `chore/azure-infra`: App Service, PostgreSQL and Blob Storage resources
- [ ] P8.4 `chore/config-secrets`: environment settings and Key Vault
- [ ] P8.5 `chore/cd-pipeline`: deploy from GitHub Actions
- [ ] P8.6 `docs/readme-final`: README with screenshots, architecture, and final CLAUDE.md review

Demo: the live app on Azure.

## Working rules
- Start each task with a 3-5 line plan, then implement only that task.
- Commit in small steps; open a PR (or merge locally) only when the app builds and runs.
- Tick the checkbox in this file in the same branch as the task.
