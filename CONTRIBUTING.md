# Contributing to Flowbite Blazor Dashboard

## Prerequisites

- **Python 3.x** - Required to run the build script (uses only standard library, no pip packages needed)
- **.NET SDK 9.0+** - Will be automatically downloaded by the build script if not installed
- **Tailwind CSS** - Will be automatically downloaded by the build script if not present

## AI Assistant Integration

This project includes a `CLAUDE.md` file that provides architectural guidance to AI coding assistants and team members. If you're using the Cline AI assistant, you may want to create a symbolic link so it can access this documentation:

**Linux/macOS/WSL:**
```bash
mkdir -p .clinerules
ln -s ../CLAUDE.md .clinerules/AGENT.md
```

**Windows (Command Prompt - requires Administrator privileges):**
```cmd
mkdir .clinerules
mklink .clinerules\AGENT.md ..\CLAUDE.md
```

**Windows (PowerShell - requires Administrator privileges):**
```powershell
New-Item -ItemType Directory -Force -Path .clinerules
New-Item -ItemType SymbolicLink -Path .clinerules\AGENT.md -Target ..\CLAUDE.md
```

**Note**: On Windows, if you don't have administrator privileges, you can simply copy the file instead:
```powershell
Copy-Item CLAUDE.md .clinerules\AGENT.md
```
However, with a copy, you'll need to manually keep both files in sync when updates are made.

## Quick Start

The project includes a `build.py` script that automates the setup and build process:

1. **Build the project** (default):
   ```bash
   python build.py
   # or on Unix-like systems with executable permissions:
   ./build.py
   ```

2. **Run the application**:
   ```bash
   python build.py run
   ```
   Then open <http://localhost:5269/> in your browser.

3. **Development with hot reload**:
   ```bash
   python build.py watch
   ```
   Press `Ctrl+C` to stop watching.

4. **Create production build**:
   ```bash
   python build.py publish
   ```
   Output will be in the `./dist` directory.

The build script automatically:
- Detects your operating system (Windows, Linux, macOS)
- Downloads the appropriate Tailwind CSS executable if needed
- Checks for .NET SDK 9.0+ and installs it locally if not found
- Runs the requested build command

## Development Workflow

### Using the Build Script (Recommended)

The `build.py` script provides four commands:

- `python build.py` or `python build.py build` - Build the project
- `python build.py run` - Run the application
- `python build.py watch` - Run with hot reload for development
- `python build.py publish` - Create production build in `./dist`

### IDE Development

1. **Debug/Development** (default):
   - Use `dotnet watch` directly
   - Press F5 in Visual Studio Code to run and debug

### Static Build and Serve

To build and serve the WASM app as static files locally:

1. **Install dotnet-serve tool** (one-time setup):
   ```bash
   dotnet tool install --global dotnet-serve
   ```

2. **Publish the WASM app** using the build script:
   ```bash
   python build.py publish
   ```
   This creates a production-ready build in the `./dist` directory.

3. **Serve the static files locally**:
   ```bash
   dotnet serve -d dist/wwwroot -p 8080
   ```
   Then open <http://localhost:8080/> in your browser.

   **Note**: The `-d dist/wwwroot` flag points to the wwwroot directory inside the dist folder, which contains the actual static files. The `-p 8080` flag specifies the port (you can change this if needed).

4. **Stop the server**: Press `Ctrl+C` in the terminal to stop the server.

This workflow is useful for:
- Testing the production build locally
- Verifying the app works as static files before deployment
- Simulating the deployed environment

## Advanced Setup

### Manual Tailwind CSS Installation

If you prefer to manually install Tailwind CSS instead of using the build script:

**macOS:**
```bash
mkdir -p ./src/WebApp/tools && cd ./src/WebApp/tools && curl -sLO https://github.com/tailwindlabs/tailwindcss/releases/download/v3.4.15/tailwindcss-macos-arm64 && chmod +x tailwindcss-macos-arm64 && mv tailwindcss-macos-arm64 tailwindcss && cd ../../..
```

**Linux:**
```bash
mkdir -p ./src/WebApp/tools && cd ./src/WebApp/tools && curl -sLO https://github.com/tailwindlabs/tailwindcss/releases/download/v3.4.15/tailwindcss-linux-x64 && chmod +x tailwindcss-linux-x64 && mv tailwindcss-linux-x64 tailwindcss && cd ../../..
```

**Windows (PowerShell):**
```powershell
mkdir ./src/WebApp/tools -Force; `
cd ./src/WebApp/tools; `
Invoke-WebRequest -Uri "https://github.com/tailwindlabs/tailwindcss/releases/download/v3.4.15/tailwindcss-windows-x64.exe" `
   -OutFile "tailwindcss.exe" `
   -UseBasicParsing ; `
cd ../../..
```

### Using .NET CLI Directly

If you prefer to use .NET CLI commands directly instead of the build script:

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
