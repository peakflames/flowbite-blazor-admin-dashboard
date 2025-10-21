# Contributing to Flowbite Blazor Dashboard

## Development Setup

1. Install standalone Tailwind CSS CLI executable:

   Mac:

   ```bash
   mkdir ./src/WebApp/tools && cd ./src/WebApp/tools && curl -sLO https://github.com/tailwindlabs/tailwindcss/releases/latest/download/tailwindcss-macos-arm64 && chmod +x tailwindcss-macos-arm64 && mv tailwindcss-macos-arm64 tailwindcss && cd ../../..
   ```

   Linux:

   ```bash
   mkdir ./src/WebApp/tools && cd ./src/WebApp/tools && curl -sLO https://github.com/tailwindlabs/tailwindcss/releases/latest/download/tailwindcss-linux-x64 && chmod +x tailwindcss-linux-x64 && mv tailwindcss-linux-x64 tailwindcss && cd ../../..
   ```

   Windows:

   ```pwsh
   mkdir ./src/WebApp/tools -Force; `
   cd ./src/WebApp/tools; `
   Invoke-WebRequest -Uri "https://github.com/tailwindlabs/tailwindcss/releases/latest/download/tailwindcss-windows-x64.exe" `
      -OutFile "tailwindcss.exe" `
      -UseBasicParsing ; `
   cd ../../..

   ```

1. Build the solution

   ```bash
   cd ./src/WebApp
   dotnet build
   ```

1. Run the Blazor WASM Static Web App

   ```bash
   cd ./src/WebApp
   dotnet run
   ```

   Then open <http://localhost:5269/> in your browser.

## Development Workflow

### Local Development

The solution is configured for two development modes:

1. Debug/Development (default):
   - Use `dotnet watch`
   - F5 to run and debug
