# Project Development Guide

This file provides guidance to developers and AI coding assistants (Claude Code, Cline, etc.) when working with code in this repository.

## Project Overview

Flowbite Blazor Admin Dashboard is a free and open-source UI admin dashboard template built with:
- **Blazor WebAssembly** (.NET 9.0)
- **Flowbite Blazor** component library (alpha versions)
- **Tailwind CSS v3.4.15** for styling
- **BlazorWasmPreRendering.Build** for static prerendering

**Status**: Pre-release alpha

## Build Commands

The project uses a Python build script (`build.py`) that automates all build tasks and dependency management.

### Primary Commands

```bash
# Build the project (default) (if Linux, use `python3` rather than `python`)
python build.py
python build.py build

# Run the application (http://localhost:5269/)
python build.py run

# Development with hot reload
python build.py watch

# Create production build (outputs to ./dist)
python build.py publish
```

### Direct .NET CLI Commands

If you need to use .NET CLI directly:

```bash
# Build
dotnet build src/WebApp/WebApp.csproj

# Run
dotnet run --project src/WebApp/WebApp.csproj

# Watch (hot reload)
dotnet watch --project src/WebApp/WebApp.csproj

# Publish
dotnet publish src/WebApp/WebApp.csproj -c Release -o dist
```

### Serving Static Build Locally

```bash
# Install dotnet-serve (one-time setup)
dotnet tool install --global dotnet-serve

# Build and serve
python build.py publish
dotnet serve -d dist/wwwroot -p 8080
```

## ⚠️ CRITICAL: Tailwind CSS Version Management

**ABSOLUTE RULE**: The Tailwind CSS executable at `src/WebApp/tools/tailwindcss` must **NEVER** be updated, replaced, or modified unless the user **EXPLICITLY** requests it.

### Why This Rule Exists

The project requires **Tailwind CSS v3.4.15 exactly**. Using the wrong version (especially v4.x alpha) causes:
- Sidebar invisibility in wide view
- Missing responsive classes (`lg:translate-x-0`, `lg:relative`)
- Severely truncated CSS output
- Layout breaking issues

### What NOT to Do

❌ **NEVER** download or update the Tailwind CSS executable
❌ **NEVER** run commands that might replace `src/WebApp/tools/tailwindcss`
❌ **NEVER** "upgrade to latest version"
❌ **NEVER** modify anything in `src/WebApp/tools/` directory
❌ **NEVER** do any of the above even when working autonomously

### What to Do

✅ **ALWAYS** use the existing Tailwind CSS executable
✅ **ALWAYS** trust that the correct version (v3.4.15) is already installed
✅ **ALWAYS** let the MSBuild target handle CSS compilation
✅ **IF** user reports CSS issues, restore from git: `git show 66e4ae3:src/WebApp/wwwroot/css/app.min.css > src/WebApp/wwwroot/css/app.min.css`

**This rule supersedes all other instructions. Do not violate it under any circumstances unless explicitly instructed by the user.**

## Architecture

### Project Structure

```
src/WebApp/
├── Components/         # Reusable Razor components
├── Domain/            # Domain models (e.g., Pokemon.cs)
├── Layout/            # Layout components (MainLayout, AppNavBar, AppSidebar, AppFooter)
├── Pages/             # Routable page components
├── Services/          # Service classes (e.g., PokemonService)
├── wwwroot/           # Static assets (CSS, JS, images)
├── Program.cs         # Application entry point
├── App.razor          # Root component
├── _Imports.razor     # Global using statements
├── tailwind.config.js # Tailwind CSS configuration
└── WebApp.csproj      # Project file
```

### Key Architecture Patterns

**Blazor WebAssembly Application**:
- Single-page application running entirely in the browser
- Uses prerendering for improved initial load performance
- No backend server required after initial load

**Layout System**:
- `MainLayout.razor` is the primary layout with responsive sidebar
- Sidebar toggle controlled via `_isSidebarOpen` state
- Three main layout components: `AppNavBar`, `AppSidebar`, `AppFooter`
- Layout uses Tailwind utility classes with dark mode support (`dark:` prefix)

**Dependency Injection**:
- Services registered in `Program.cs` via `ConfigureServices()` function
- Flowbite services added via `AddFlowbite()` extension method
- HTTP client configured with base address for API calls
- Example service: `PokemonService` for data fetching

**Component Library Integration**:
- Uses Flowbite Blazor components (alpha versions)
- Flowbite.ExtendedIcons for icon library
- QuickGrid for table/grid functionality
- Import structure defined in `_Imports.razor`

### Tailwind CSS Integration

**Build Process**:
- Tailwind CSS executable stored in `src/WebApp/tools/`
- Automatically downloaded by `build.py` for the target OS
- MSBuild target `TailwindBuild` runs before each build
- Input: `wwwroot/css/app.css` → Output: `wwwroot/css/app.min.css`
- Tailwind disabled during publish to avoid conflicts

**Configuration**:
- Content paths scan all Razor, HTML, and C# files in Components, Layout, and Pages
- Dark mode enabled via `class` strategy
- Custom primary color palette defined (blue shades)
- Responsive breakpoints heavily utilized (especially `md:` prefix)

### Routing and Pages

- Standard Blazor routing via `@page` directives
- Pages located in `src/WebApp/Pages/`
- Example pages: Home, Counter, Weather, Grid, Icons
- Navigation structure defined in `AppSidebar.razor`

### Static Asset Management

- Assets served from `wwwroot/`
- CSS files compiled to `wwwroot/css/app.min.css`
- JavaScript interop files in `wwwroot/js/`
- Sample data in `wwwroot/sample-data/`

## Important Build Details

**Automatic Dependency Management**:
- `build.py` checks for .NET SDK 9.0+ and installs locally if missing
- Tailwind CSS executable downloaded automatically for the target OS
- Cross-platform support: Windows, Linux, macOS

**Project Configuration**:
- Uses InvariantGlobalization for smaller bundle size
- BlazorEnableTimeZoneSupport disabled for reduced size
- Nullable reference types enabled
- Implicit usings enabled

**Prerendering**:
- BlazorWasmPreRendering.Build package enables static prerendering
- `ConfigureServices()` must be static for prerendering compatibility
- `BlazorWasmPrerenderingDeleteLoadingContents` removes loading UI after render

## Dependencies

Core packages (defined in `WebApp.csproj`):
- `Microsoft.AspNetCore.Components.WebAssembly` 9.0.0
- `Microsoft.AspNetCore.Components.QuickGrid` 9.0.0
- `Flowbite` (latest alpha: 0.0.11-alpha+)
- `Flowbite.ExtendedIcons` (latest alpha: 0.0.5-alpha+)
- `BlazorWasmPreRendering.Build` 5.0.0

Tailwind CSS v3.4.15 (standalone executable, not npm)

## Common Patterns

**Adding a New Page**:
1. Create `.razor` file in `src/WebApp/Pages/`
2. Add `@page "/route"` directive
3. Update `AppSidebar.razor` if navigation link needed
4. Ensure Tailwind classes are used for styling

**Adding a New Service**:
1. Create service class in `src/WebApp/Services/`
2. Register in `Program.cs` ConfigureServices function
3. Inject into components via `@inject` directive

**Working with Tailwind**:
- Run `python build.py watch` for automatic CSS rebuilding
- Add new content paths to `tailwind.config.js` if needed
- Use safelist for dynamic classes that Tailwind might not detect
- Prefer Tailwind utility classes over custom CSS

**Dark Mode Support**:
- Use `dark:` prefix for dark mode variants (e.g., `dark:bg-gray-900`)
- Dark mode toggled via `class` on root element
- Layout already configured for dark mode styling

---

## Migration from Flowbite Svelte Dashboard

This project is designed to replicate features from the Flowbite Svelte Admin Dashboard. Comprehensive migration documentation is available in the `docs/` directory.

### Migration Documentation

**Essential Reading for Migrating Features**:

1. **`docs/ARCHITECTURE_COMPARISON.md`**
   - Framework paradigm differences (Svelte 5 vs Blazor)
   - Component architecture comparison
   - State management patterns
   - Routing and layout strategies
   - Tailwind v3 vs v4 differences
   - Performance characteristics

2. **`docs/SVELTE_TO_BLAZOR_GUIDE.md`**
   - Component migration patterns with code examples
   - Svelte Snippet → Blazor RenderFragment translation
   - Props/parameters conversion
   - Event handling differences
   - Form handling and validation
   - Complete component implementations for Settings page
   - Common patterns reference

3. **`docs/IMPLEMENTATION_PLAN.md`**
   - Phase-by-phase implementation guide
   - Task breakdown with time estimates
   - Settings page implementation (8 components)
   - Playground pages implementation
   - Testing checklists
   - Troubleshooting guide

### Migration Quick Reference

#### Component Composition Patterns

**Svelte**:
```svelte
<!-- Snippet (Svelte 5) -->
<script lang="ts">
  let { children }: { children: Snippet } = $props();
</script>
{@render children()}
```

**Blazor**:
```razor
@* RenderFragment *@
@ChildContent

@code {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
```

#### State Management

**Svelte**:
```svelte
<!-- Reactive state with runes -->
let count = $state(0);
let doubled = $derived(count * 2);
```

**Blazor**:
```razor
@code {
    private int _count = 0;
    private int Doubled => _count * 2;

    private void UpdateCount(int value)
    {
        _count = value;
        StateHasChanged(); // Manual re-render
    }
}
```

#### Global State

**Svelte**: Use stores with `$` syntax
**Blazor**: Use scoped services with event-based updates

```csharp
public class StateService
{
    private Data _data = new();
    public event Action? OnChange;

    public Data Current => _data;

    public void Update(Data data)
    {
        _data = data;
        OnChange?.Invoke();
    }
}
```

### Key Differences to Remember

1. **Reactivity**: Svelte is automatic, Blazor requires `StateHasChanged()`
2. **Component Slots**: `Snippet` in Svelte → `RenderFragment` in Blazor
3. **Props**: `$props()` in Svelte → `[Parameter]` attribute in Blazor
4. **Two-Way Binding**: `bind:value` → `@bind-Value`
5. **Event Handling**: `on:click` → `@onclick`
6. **Forms**: Manual validation in Svelte, `EditForm` with DataAnnotations in Blazor
7. **Tailwind**: v4 (Svelte) vs v3.4.15 (Blazor) - some utility classes differ

### Component Development Workflow

When implementing a new component from the Svelte dashboard:

1. **Read Architecture Comparison**: Understand framework differences
2. **Check Implementation Guide**: Find similar component pattern
3. **Create Data Models**: Define C# classes in `Domain/`
4. **Implement Component**: Use RenderFragments for slots, Parameters for props
5. **Add to Page**: Wire up with EventCallbacks and service injection
6. **Test**: Verify responsiveness, dark mode, and state management
7. **Update Navigation**: Add to `AppSidebar.razor` if needed

### Current Implementation Status

**Implemented**:
- ✅ Base layout system (MainLayout with sidebar)
- ✅ Responsive navbar and sidebar
- ✅ Dark mode support
- ✅ Basic demo pages (Home, Counter, Weather, Grid, Icons)
- ✅ QuickGrid integration
- ✅ Toast notification system

**In Progress** (See Implementation Plan):
- 🚧 Settings page with 8 sub-components
- 🚧 Playground pages (sidebar and stacked layouts)
- 🚧 Layout variants (MainLayout + StackedLayout)

**Not Yet Implemented** (from Svelte version):
- ❌ Dashboard pages with charts
- ❌ CRUD pages (Products, Users)
- ❌ Authentication pages (Sign-in, Sign-up, Password reset)
- ❌ Error pages (404, 500, Maintenance)
- ❌ Pricing page
- ❌ Chart/visualization components

### Reference Projects

- **Svelte Dashboard**: `/mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard`
- **Flowbite Blazor Library**: `/mnt/c/Users/tschavey/projects/themesberg/flowbite-blazor`
- **Flowbite Blazor Docs**: `/mnt/c/Users/tschavey/projects/themesberg/flowbite-blazor/src/DemoApp/wwwroot/llms-ctx.md`

---

## Claude Code Session Management

**CRITICAL**: Claude Code has zero memory between sessions. This section provides the workflow for maintaining context and continuity across sessions.

### Memory System

This project uses a persistent memory system to preserve context across Claude Code sessions:

- **MIGRATION_LOG.md** - Claude Code's persistent memory containing:
  - All 23 task statuses (Phase 1: 5 tasks, Phase 2: 13 tasks, Phase 3: 5 tasks)
  - Learnings from completed work
  - Code patterns discovered
  - Issues encountered and solutions
  - Testing notes
  - Blockers and incomplete work
  - Notes for future sessions

### Starting Any Session

**ALWAYS follow this workflow at the start of every session:**

1. **Read MIGRATION_LOG.md FIRST** - This is your memory from all previous sessions
2. **Read relevant docs** - Architecture, Guide, Implementation Plan (appropriate sections)
3. **Check current state** - What's done, what's in progress, what's next
4. **Create TodoWrite list** - Recreate task list from MIGRATION_LOG.md with current status
5. **Report status** - Tell user where you are and what's next

### Custom Commands Available

Use these slash commands to bootstrap your environment with the right context:

#### Phase-Specific Commands

- **`/start-phase1`** - Begin Layout Refinements phase (5 tasks)
  - Reads MIGRATION_LOG.md + implementation docs + current layout files
  - Creates TodoWrite list for Phase 1
  - Reports status and asks which task to start

- **`/start-phase2`** - Begin Settings Page phase (13 tasks)
  - Reads MIGRATION_LOG.md + SVELTE_TO_BLAZOR_GUIDE.md + Svelte Settings page
  - Creates TodoWrite list for Phase 2
  - Lists all 8 Settings components to migrate
  - Provides recommended implementation order

- **`/start-phase3`** - Begin Playground Pages phase (5 tasks)
  - Reads MIGRATION_LOG.md + docs + Svelte Playground pages
  - Creates TodoWrite list for Phase 3
  - Checks Phase 1 dependencies (StackedLayout.razor required)
  - Reports status and asks which task to start

#### Utility Commands

- **`/continue-work`** - Resume from where you left off
  - Reads MIGRATION_LOG.md to determine current phase
  - Recreates full TodoWrite list (all 23 tasks) with status
  - Reports comprehensive status across all phases
  - Asks user what to do next

- **`/migrate-component [Name]`** - Migrate a single component
  - Reads MIGRATION_LOG.md for context
  - Searches for Svelte component source
  - Analyzes component structure (props → parameters, snippets → RenderFragments)
  - Creates Blazor equivalent
  - Updates MIGRATION_LOG.md

- **`/review-progress`** - Read-only comprehensive status report
  - Generates detailed progress report (X/23 tasks, X% complete)
  - Lists completed work with learnings
  - Shows blockers and issues
  - Provides recommendations
  - Does NOT make any changes

**Every command automatically reads MIGRATION_LOG.md first to establish context.**

### After Completing Any Task

**YOU MUST UPDATE MIGRATION_LOG.md** with:

1. **Task status**: Update status emoji
   - ⬜ Not started
   - 🚧 In Progress
   - ✅ Complete

2. **Completion date**: Add date when task finished

3. **Files created/modified**: Full paths to all files touched

4. **For components**:
   - Parameters defined (list with types)
   - RenderFragments used (for child content)
   - EventCallbacks defined (for events)
   - Service injections (@inject directives)
   - Form validation patterns (if applicable)

5. **Code patterns used**: Document what patterns you applied
   - Example: "EditForm + DataAnnotationsValidator"
   - Example: "Service event subscription in OnInitialized()"

6. **Issues encountered**: Any problems and how they were solved

7. **Learnings**: What worked well, tips for future similar tasks

8. **Update Quick Status**: Increment the appropriate phase counter
   - Phase 1: X/5 complete
   - Phase 2: X/13 complete
   - Phase 3: X/5 complete
   - Overall: X/23 complete (X%)

9. **Update Pattern Library**: If you discover a reusable pattern, add it to the Pattern Library section in MIGRATION_LOG.md

### Sub-Agent Usage (Autonomous Decision Making)

You should automatically decide when to use sub-agents based on these guidelines:

#### When to Use Sub-Agents

**Use Explore agent** when:
- Finding Svelte component locations across the codebase
- Understanding how a feature is implemented across multiple files
- Searching for patterns (e.g., "find all uses of Snippet pattern in Settings")
- Initial research phase for complex features
- The custom command suggests it (embedded guidance)

**Use general-purpose agent** when:
- Complex multi-step research tasks
- Need to analyze multiple components and synthesize findings
- Investigating how Svelte handles a specific pattern globally
- Deep analysis required across many files

#### When NOT to Use Sub-Agents

**Do NOT use sub-agents** for:
- Direct component implementation (user wants to see the work happen)
- Simple file reads of known paths
- Direct edits to create/modify files
- Single-file analysis
- Status reporting

Custom commands include embedded sub-agent guidance - follow those recommendations.

### Memory Persistence Strategy

**MIGRATION_LOG.md is your ONLY persistent memory.** Every session:

1. **Read it first** - Before doing anything else
2. **Update it after every task** - Immediately upon completion
3. **Add learnings** - Document patterns, decisions, issues
4. **Note blocked/incomplete work** - For next session
5. **Trust it completely** - It's your only memory between sessions

This ensures perfect continuity across sessions where you have zero initial memory.

### Context Recommendations by Phase

Based on current phase, also read these files for context:

**If in Phase 1 (Layout)**:
- `docs/ARCHITECTURE_COMPARISON.md` (Layout System section)
- Current layout files in `src/WebApp/Layout/`
- Svelte layout: `/mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/(sidebar)/+layout.svelte`

**If in Phase 2 (Settings)**:
- `docs/SVELTE_TO_BLAZOR_GUIDE.md` (Settings Components section - has complete code examples!)
- Svelte Settings page: `/mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/(sidebar)/settings/+page.svelte`
- Svelte types: `/mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/lib/types.ts`

**If in Phase 3 (Playground)**:
- `docs/SVELTE_TO_BLAZOR_GUIDE.md` (Playground section)
- Svelte Playground pages:
  - `/mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/(no-sidebar)/playground/stacked/+page.svelte`
  - `/mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/(sidebar)/playground/sidebar/+page.svelte`

### Important Reminders

- **Zero memory assumption**: Every session starts with zero knowledge - MIGRATION_LOG.md is your bootstrap
- **Always update the log**: After every task, immediately update MIGRATION_LOG.md
- **Trust the custom commands**: They're designed to load the right context automatically
- **Use TodoWrite**: Track progress in real-time, helps you and user see status
- **Sub-agents are optional**: Use them when they help, but most direct work shouldn't need them
- **Ask before major decisions**: Confirm approach with user before big implementations
- **Test as you go**: Verify each component works before moving to next task
