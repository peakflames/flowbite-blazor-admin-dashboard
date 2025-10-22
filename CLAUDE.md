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
# Build the project (default)
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
