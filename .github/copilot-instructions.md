# Copilot Instructions for ABC

ABC is an ABA (Applied Behavior Analysis) data collection app for therapists and parents. It tracks observations of children's antecedents, behaviors, and consequences.

## Architecture

This is a .NET Aspire-orchestrated solution with a React frontend and HotChocolate GraphQL API backed by PostgreSQL.

**Projects:**

- **ABC.AppHost** — Aspire orchestrator. Wires up PostgreSQL, the Management API, Kong gateway (Docker mode), and the React app. Supports Azure deployment, local Docker, or external PostgreSQL via feature flags (`UseAzurePostgres`, `UseDockerPostgres`, `UseExternalPostgres`). External PostgreSQL is the development default and requires no container runtime.
- **ABC.Management.Api** — ASP.NET Core GraphQL API using HotChocolate 15. Handles auth (ASP.NET Identity + JWT), CRUD for domain entities, and observation lifecycle. GraphQL types live in `Types/`. Uses Mediator pattern (source-generated) for command dispatch.
- **ABC.Management.Domain** — Domain entities, value objects, and FluentValidation validators. No infrastructure dependencies.
- **ABC.PostGreSQL** — EF Core data layer. `ABCContext` extends `IdentityDbContext<ApplicationUser>`. Includes `UnitOfWork`, repository access, and validation services that check uniqueness via `EF.Functions.ILike`.
- **ABC.SharedKernel** — Shared abstractions (interfaces, base types).
- **ABC.SharedEntityFramework** — Shared EF Core infrastructure (generic repository).
- **ABC.ServiceDefaults** — Aspire service defaults (OpenTelemetry, health checks, resilience).
- **ABC.React** — React 18 SPA with TypeScript, webpack, Tailwind CSS, Apollo Client 4.
- **ABC.Tests** — Single test project covering API, Domain, and PostgreSQL layers.

**Request flow:** React → (Kong gateway in local dev, optional) → HotChocolate GraphQL API → Mediator command/handler → UnitOfWork/Repository → PostgreSQL.

## Build, Test, and Run

### .NET

```powershell
# Build the entire solution
dotnet build ABC.slnx

# Run all tests
dotnet test ABC.Tests

# Run a single test by name
dotnet test ABC.Tests --filter "FullyQualifiedName~YourTestName"

# Exclude PostgreSQL integration tests (they need Docker)
dotnet test ABC.Tests --filter "Category!=integration"

# Run the Aspire AppHost (starts all services)
dotnet run --project ABC.AppHost
```

### React (from `ABC.React/`)

```powershell
npm install
npm run build        # webpack production build
npm test             # jest
npm run test:watch   # jest in watch mode
npm run test:coverage
npm start            # webpack-dev-server (port injected by Aspire)
```

### EF Core Migrations (from `ABC.PostGreSQL/`)

```powershell
dotnet ef migrations add <MigrationName> --startup-project ..\ABC.Management.Api
dotnet ef database update --startup-project ..\ABC.Management.Api
```

## Key Conventions

### Command/Handler Pattern (Mediator)

Commands are record types with static factory methods in `ABC.Management.Api/Commands/`. Handlers in `Handlers/` receive commands via source-generated Mediator and return `BaseResponseCommand<T>`, which wraps a result value and a list of errors.

GraphQL mutations call `ExecuteHandler()` (in `Extensions/BaseResponseCommandExtensions.cs`), which dispatches the command and copies any errors into the GraphQL error context.

### Validation Pipeline (Decorators)

Create commands pass through decorator classes in `Decorators/` that run FluentValidation validators before the handler executes. Domain validators in `ABC.Management.Domain/Validators/` enforce business rules (required fields, uniqueness by name). Uniqueness checks use services in `ABC.PostGreSQL/ValidationServices/` that query with `EF.Functions.ILike` for case-insensitive matching.

### GraphQL API Shape

- Types are defined in `ABC.Management.Api/Types/` — one file per entity (e.g., `Antecedents.cs`, `Children.cs`, `Observations.cs`).
- Antecedent, Behavior, and Consequence have Create, Read, Delete (no Update).
- Child and Observation have full CRUD.
- Observation is an aggregate with a lifecycle: `StartObservation` → updates (antecedents, behaviors, consequences, notes) → `EndObservation`. Updates are blocked after the observation is ended. Ending requires at least one antecedent, behavior, and consequence.

### Authorization

- ASP.NET Identity with Admin/User roles. JWT tokens stored in `localStorage["abc_token"]`.
- External auth: Google OAuth and Azure Entra ID (single-tenant). Dev-only fake external login in `Types/DevAuth.cs`.
- Child and Observation queries/mutations enforce ownership: non-admin users only see their own data (`UserId` from JWT claims).

### Entity Framework

- `ABCContext` uses `UseNpgsql` with snake_case naming convention (`EFCore.NamingConventions`).
- `Observation.When` (a `DateTimeRange` value object) is stored as an owned JSON column (`ToJson()`).
- Child ↔ ChildCondition is a many-to-many relationship.

### React Frontend

- Apollo Client 4: hooks and `ApolloProvider` are imported from `@apollo/client/react`, not `@apollo/client`. `MockedProvider` is removed; tests use `MockLink` + `ApolloProvider`.
- GraphQL operations are in `src/graphql/operations/`.
- Auth context in `src/context/AuthContext.tsx` provides `isAdmin`, `isAuthenticated`, login/logout.
- Webpack dev server proxies `/api` to the Management API (target URL from Aspire env vars), stripping the `/api` prefix. Apollo Client URI is `/api/graphql`.

### Testing

- **Framework:** xUnit + Reqnroll (BDD `.feature` files) + Shouldly assertions + FakeItEasy mocks + Bogus for test data.
- **Fixtures:** `ApiStartupFixture` and `DomainStartupFixture` use FakeItEasy fakes. `PostgresStartupFixture` uses Testcontainers with `EnsureCreatedAsync()` (not `MigrateAsync()`) to avoid `PendingModelChangesWarning`.
- **Integration tests:** PostgreSQL tests in `ABC.Tests/PostgreSQL/` require Docker and are tagged with `@integration`. Exclude with `--filter "Category!=integration"`.
- **E2E tests:** Playwright browser tests in `ABC.Tests/E2E/` are tagged with `@e2e`. They require the app to be running and Playwright browsers installed. Exclude with `--filter "Category!=e2e"`.
- **Structure:** Tests organized as `ABC.Tests/Api/`, `ABC.Tests/Domain/`, `ABC.Tests/PostgreSQL/`, `ABC.Tests/E2E/`, each with `Features/` (`.feature` files) and `StepDefinitions/`.

### E2E Browser Tests (Playwright)

E2E tests use Playwright with Reqnroll BDD, matching the same `.feature` file approach as unit and integration tests.

**Prerequisites:**
1. Build the test project: `dotnet build ABC.Tests`
2. Install browsers (one-time): `pwsh ABC.Tests/bin/Debug/net10.0/playwright.ps1 install`
3. Start the app: `dotnet run --project ABC.AppHost`

**Run commands:**
```powershell
# Run only E2E tests
dotnet test ABC.Tests --filter "Category=e2e"

# Run all tests EXCEPT E2E
dotnet test ABC.Tests --filter "Category!=e2e"

# Exclude both E2E and integration tests
dotnet test ABC.Tests --filter "Category!=e2e&Category!=integration"

# Run headed (visible browser) for debugging
$env:E2E_HEADED="true"; dotnet test ABC.Tests --filter "Category=e2e"

# Target a different base URL
$env:E2E_BASE_URL="http://localhost:5000"; dotnet test ABC.Tests --filter "Category=e2e"
```

**Conventions:**
- All E2E feature files use the `@e2e` tag
- Auth uses `DevExternalLogin` (dev-only mutation) — the app must be running in Development mode
- `PlaywrightHooks.cs` manages browser lifecycle and captures screenshots on failure to `bin/screenshots/`
- Shared steps (auth, navigation) are in `E2E/StepDefinitions/` and reused across features
- Entity-specific steps follow the naming pattern `{Entity}CrudStepDefinitions.cs`

### Package Management

- Central Package Management via `Directory.Packages.props` — all NuGet versions are pinned there.
- The Aspire SDK version in `ABC.AppHost.csproj` must match the Aspire package versions in `Directory.Packages.props`.
- .NET SDK version is pinned in `global.json`.

### Kong Gateway (Local Dev Only)

Kong OSS 3.9 runs as a Docker container sharing the Postgres instance (dedicated `kong` database). Config is seeded via Admin API in `ABC.AppHost/kong/start.sh`. The React app sends an `apikey` header via Apollo Client's `authLink`. Note: Kong's Docker image doesn't include curl/wget — use `kong` CLI tools for config operations.
