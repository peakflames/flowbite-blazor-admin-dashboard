# Repository Guidelines

## Project Structure & Module Organization
The Blazor WebAssembly app lives in `src/WebApp/`, with routable screens under `Pages/` (for example `Pages/Crud/Users.razor`) and reusable widgets in `Components/`. Layout scaffolding sits in `Layout/`, domain models in `Domain/`, and DI-friendly services in `Services/`. Static assets, compiled CSS, and sample datasets reside in `wwwroot/` (`wwwroot/css/`, `wwwroot/data/`). Keep the Tailwind CLI binary checked in under `tools/`. Repository-level docs (`CLAUDE.md`, `docs/*.md`) and the running status log in `MIGRATION_LOG.md` should be reviewed before starting new work.

## Build, Test, and Development Commands
Use the Python helper first: `python build.py` (or `python3 build.py`) to restore, compile, and wire Tailwind; `python build.py watch` enables hot reload; `python build.py publish` outputs a production build to `dist/`, which you can serve with `dotnet serve -d dist/wwwroot -p 8080`. Direct .NET CLI fallbacks include `dotnet build src/WebApp/WebApp.csproj`, `dotnet run --project src/WebApp/WebApp.csproj`, and `dotnet watch --project src/WebApp/WebApp.csproj`. Tailwind can be refreshed manually with `./tools/tailwindcss -i ./wwwroot/css/app.css -o ./wwwroot/css/app.min.css --watch`.

## Coding Style, Tooling & Tailwind Guardrails
Adhere to standard .NET formatting: 4-space indentation, `PascalCase` for components, services, and records, `camelCase` for private fields, `I`-prefixed interfaces. Nullable reference types and implicit usings are enforced; treat warnings as build-blocking. Run `dotnet format` before committing. Never modify or replace the Tailwind executable in `tools/`: the project must stay on Tailwind CSS v3.4.15 exactly. Let MSBuild or `build.py` handle CSS generation, and restore `wwwroot/css/app.min.css` from git if styling regresses.

## Testing Guidelines
Stand up tests in a sibling project such as `WebApp.Tests` using xUnit and bUnit for component checks. Favor descriptive method names (`ProductsPage_LoadsGridData`) and keep fixtures in `wwwroot/sample-data` synchronized with test expectations. Execute `dotnet test WebApp.sln` (or `python build.py test` once added) before opening a PR, aiming for smoke coverage on CRUD grids, settings modules, and layout toggles.

## Commit & Pull Request Guidelines
Follow Conventional Commits used in history (`feat:`, `docs:`, optional scopes like `feat(crud): …`) and keep each commit focused. PRs should summarize intent, reference related issues, attach UI screenshots or GIFs for visual changes, and list local verification (`python build.py`, `dotnet test`). Update `MIGRATION_LOG.md` with task status, affected files, and learnings after each substantive change to preserve continuity across sessions.
