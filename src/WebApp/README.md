# WebApp

## Development Setup

1. Install standalone Tailwind CSS CLI executable:

   Mac/Linux:

   ```bash
   mkdir ./tools && cd ./tools && curl -sLO https://github.com/tailwindlabs/tailwindcss/releases/latest/download/tailwindcss-macos-arm64  && chmod +x tailwindcss-macos-arm64 && mv tailwindcss-macos-arm64 tailwindcss
   ```

   Windows:

   ```pwsh
   mkdir ./tools -Force; `
   cd ./tools; `
   Invoke-WebRequest -Uri "https://github.com/tailwindlabs/tailwindcss/releases/latest/download/tailwindcss-windows-x64.exe" `
      -OutFile "tailwindcss.exe" `
      -UseBasicParsing ; `
   cd ..

   ```

1. Build the solution

   ```bash
   dotnet build
   ```

1. Run the Blazor WASM Static Web App

   ```bash
   dotnet run
   ```

   Then open <http://localhost:5269/> in your browser.

## Development Workflow

### Local Development

The solution is configured for two development modes:

1. Debug/Development (default):
   - Use `dotnet watch`
   - F5 to run and debug
