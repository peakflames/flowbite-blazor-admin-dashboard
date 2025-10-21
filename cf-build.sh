#!/bin/sh
set -e  # Exit on error

REQUIRED_VERSION="9.0"
DOTNET_PATH="./dotnet/dotnet"

# Function to compare version numbers
version_greater_equal() {
    printf '%s\n%s\n' "$1" "$2" | sort -V | head -n1 | grep -q "^$2$"
}

# Check if dotnet is already installed and accessible
if command -v dotnet >/dev/null 2>&1; then
    CURRENT_VERSION=$(dotnet --version | cut -d'.' -f1,2)
    echo "Found .NET version: $CURRENT_VERSION"
    
    if version_greater_equal "$CURRENT_VERSION" "$REQUIRED_VERSION"; then
        echo "Using system-installed .NET $CURRENT_VERSION"
        DOTNET_PATH="dotnet"
    else
        echo "System .NET version $CURRENT_VERSION is older than required version $REQUIRED_VERSION. Will install required version."
        INSTALL_DOTNET=true
    fi
else
    echo "No system .NET installation found"
    INSTALL_DOTNET=true
fi

# Install .NET if needed
if [ "$INSTALL_DOTNET" = true ]; then
    echo "Installing .NET $REQUIRED_VERSION..."
    curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
    chmod +x dotnet-install.sh
    ./dotnet-install.sh -c $REQUIRED_VERSION -InstallDir ./dotnet
    DOTNET_PATH="./dotnet/dotnet"
    echo "Using .NET version: $($DOTNET_PATH --version)"
fi

# Get current branch
BRANCH=$(git branch --show-current)
echo "Building branch: $BRANCH"

# Download and setup Tailwind CSS
echo "Downloading Tailwind CSS executable..."
mkdir -p src/WebApp/tools
cd src/WebApp/tools
curl -sLO https://github.com/tailwindlabs/tailwindcss/releases/latest/download/tailwindcss-linux-x64
chmod +x tailwindcss-linux-x64
mv tailwindcss-linux-x64 tailwindcss
cd ../../..
echo "Tailwind CSS executable ready"

# Main branch - use published packages
echo "Publishing DemoApp with published packages..."
$DOTNET_PATH publish src/WebApp/WebApp.csproj -c Release -o dist


if [ $? -ne 0 ]; then
    echo "Error: Failed to publish DemoApp"
    exit 1
fi

echo "Successfully published to ./dist"
