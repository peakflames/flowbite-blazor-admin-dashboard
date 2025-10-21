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

### Static Build and Serve

To build and serve the WASM app as static files locally:

1. **Install dotnet-serve tool** (one-time setup):

   ```bash
   dotnet tool install --global dotnet-serve
   ```

2. **Publish the WASM app to a static directory**:

   ```bash
   dotnet publish src/WebApp/WebApp.csproj -c Release -o dist
   ```

   This creates a production-ready build in the `dist` directory.

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
