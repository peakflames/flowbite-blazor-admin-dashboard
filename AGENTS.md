# Repository Guidelines

## Project Structure & Module Organization
The Blazor WASM app lives in `src/WebApp`. Razor views reside in `Pages` organized by feature; reusable pieces in `Components`; layout chrome in `Layout`; domain records and providers in `Domain` and `Services`. Charts and seeded data stay in `Charts`. Static assets and Tailwind entry points are in `wwwroot`, and CLI tooling (Tailwind binary, configs) sits in `tools`. Use the root `build.py` for automation and consult `docs/` for reference notes.

## Build, Test, and Development Commands
Use the Python automation:
- `python build.py` — restore dependencies and build.
- `python build.py run` — start the app at `http://localhost:5269/`.
- `python build.py watch` — hot reload .NET and Tailwind.
- `python build.py publish` — emit a release build in `dist/`.
`.NET` CLI fallbacks (`dotnet build`, `dotnet watch --project src/WebApp/WebApp.csproj`, etc.) are acceptable when the script is unavailable.

## Coding Style & Naming Conventions
Adhere to standard .NET style: `PascalCase` for components, classes, and public members; `camelCase` for locals and parameters. Razor markup and C# blocks use 2-space indentation throughout the repo. Prefer partial class code-behind (`.razor.cs`) for heavy logic so markup stays declarative. Tailwind utilities pair with Flowbite components—order classes from layout → spacing → typography for readability. Run `dotnet format src/WebApp/WebApp.csproj` before opening a PR.

## Testing Guidelines
No automated test project ships yet. Before submitting, smoke-test dashboards with `python build.py watch` and confirm Tailwind rebuilds cleanly. When adding logic-heavy services, create or update unit tests in a future `tests/` project and run with `dotnet test`. Note manual verification in the PR until automated coverage is available.

## Commit & Pull Request Guidelines
Follow the Conventional Commit pattern seen in history (`feat(scope): summary`, `fix(scope): summary`). Use present tense and choose scopes that map to folders or UI features (`settings`, `product-drawer`, etc.). PRs should link issues, describe the change and validation, and attach before/after screenshots or recordings for UI work. Request at least one maintainer review and wait for CI (if enabled) before merging.

## UI Assets & Theming Tips
Tailwind config lives in `src/WebApp/tailwind.config.js`; PostCSS in `src/WebApp/postcss.config.js`. Place new icons, fonts, or sample data in `wwwroot` and reference them relatively. Bundle third-party JS or CSS through `wwwroot` and document new dependencies in the PR rationale.

## Reference Data

### Additianal Developer Guidance

1. **Additional Tips and Tricks**: `CLAUDE.md`

### Find Flowbite Svelte Admin Dashboard Source

Search for the Flowbite Svelte Admin Dashboard source in these locations:

1. **Entire Repository**: `/workspace/flowbite-svelte-admin-dashboard/`
2. **Components Directory**: `/workspace/flowbite-svelte-admin-dashboard/src/lib/`
3. **Route pages**: `/workspace/flowbite-svelte-admin-dashboard/src/routes/`

### Find Flowbite Svelte Source

Search for the Flowbite Svelte source in these locations:

1. **Entire Repository**: `/workspace/flowbite-svelte/`
2. **Components Directory**: `/workspace/flowbite-svelte/src/lib/`

### Find Flowbite Blazor Source

Search for the Flowbite Svelte source in these locations:

1. **Entire Repository**: `/workspace/flowbite-blazor/`
2. **Components & Icons Directory**: `/workspace/flowbite-blazor/src/Flowbite/`
3. **Additional Icons Directory**: `/workspace/flowbite-blazor/src/Flowbite.ExtendedIcons/`
4. **Component Documetation Playground**: `src/DemoApp/`

- `src/Flowbite/` — core component library.
- `src/Flowbite.ExtendedIcons/` — optional icon packs.
- `src/DemoApp/` — documentation playground; mirror every new component with a demo page.