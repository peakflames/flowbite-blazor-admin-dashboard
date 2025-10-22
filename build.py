#!/usr/bin/env python3
"""
Build script for Flowbite Blazor Admin Dashboard
Supports: build, publish, watch, run commands
"""

import sys
import os
import platform
import subprocess
import urllib.request
from pathlib import Path
from typing import Optional, Dict

REQUIRED_DOTNET_VERSION = "9.0"
TAILWIND_VERSION = "v3.4.15"
TOOLS_DIR = Path("src/WebApp/tools")
DOTNET_DIR = Path("./dotnet")
PROJECT_PATH = "src/WebApp/WebApp.csproj"


def get_os_info() -> Dict[str, str]:
    """Detect OS and return tailwindcss download info"""
    system = platform.system()
    
    if system == "Linux":
        return {
            "url": f"https://github.com/tailwindlabs/tailwindcss/releases/download/{TAILWIND_VERSION}/tailwindcss-linux-x64",
            "exec_name": "tailwindcss",
            "os_name": "Linux"
        }
    elif system == "Darwin":
        return {
            "url": f"https://github.com/tailwindlabs/tailwindcss/releases/download/{TAILWIND_VERSION}/tailwindcss-macos-arm64",
            "exec_name": "tailwindcss",
            "os_name": "macOS"
        }
    elif system == "Windows":
        return {
            "url": f"https://github.com/tailwindlabs/tailwindcss/releases/download/{TAILWIND_VERSION}/tailwindcss-windows-x64.exe",
            "exec_name": "tailwindcss.exe",
            "os_name": "Windows"
        }
    else:
        print(f"Unsupported OS: {system}")
        sys.exit(1)


def setup_tailwindcss() -> None:
    """Check and download Tailwind CSS if needed"""
    os_info = get_os_info()
    tailwind_path = TOOLS_DIR / os_info["exec_name"]
    
    if tailwind_path.exists():
        print(f"Tailwind CSS executable already exists at {tailwind_path}")
        return
    
    print(f"Downloading Tailwind CSS executable for {os_info['os_name']}...")
    TOOLS_DIR.mkdir(parents=True, exist_ok=True)
    
    try:
        urllib.request.urlretrieve(os_info["url"], tailwind_path)
        
        # Make executable on Unix-like systems
        if platform.system() != "Windows":
            os.chmod(tailwind_path, 0o755)
        
        print(f"Tailwind CSS executable downloaded to {tailwind_path}")
    except Exception as e:
        print(f"Error downloading Tailwind CSS: {e}")
        sys.exit(1)


def get_dotnet_version() -> Optional[str]:
    """Get installed dotnet version, return None if not found"""
    try:
        result = subprocess.run(
            ["dotnet", "--version"],
            capture_output=True,
            text=True,
            check=True
        )
        return result.stdout.strip()
    except (subprocess.CalledProcessError, FileNotFoundError):
        return None


def version_greater_equal(current: str, required: str) -> bool:
    """Compare version numbers (major.minor)"""
    try:
        current_parts = [int(x) for x in current.split('.')[:2]]
        required_parts = [int(x) for x in required.split('.')[:2]]
        
        # Compare major version first, then minor
        if current_parts[0] > required_parts[0]:
            return True
        elif current_parts[0] == required_parts[0]:
            return current_parts[1] >= required_parts[1]
        else:
            return False
    except (ValueError, IndexError):
        # Fallback to string comparison if parsing fails
        return current >= required


def check_dotnet() -> Optional[str]:
    """Check if dotnet is installed and meets version requirements"""
    dotnet_version = get_dotnet_version()
    
    if dotnet_version:
        print(f"Found .NET version: {dotnet_version}")
        
        # Extract major.minor version
        version_parts = dotnet_version.split('.')[:2]
        current_version = '.'.join(version_parts)
        
        if version_greater_equal(current_version, REQUIRED_DOTNET_VERSION):
            print(f"Using system-installed .NET {dotnet_version}")
            return "dotnet"
        else:
            print(f"System .NET version {current_version} is older than required version {REQUIRED_DOTNET_VERSION}")
            return None
    else:
        print("No system .NET installation found")
        return None


def install_dotnet() -> str:
    """Install .NET SDK locally"""
    print(f"Installing .NET {REQUIRED_DOTNET_VERSION}...")
    
    try:
        if platform.system() == "Windows":
            # Use PowerShell on Windows
            install_script = "dotnet-install.ps1"
            urllib.request.urlretrieve(
                "https://dot.net/v1/dotnet-install.ps1",
                install_script
            )
            
            subprocess.run(
                ["powershell", "-ExecutionPolicy", "Bypass", "-File", install_script,
                 "-Channel", REQUIRED_DOTNET_VERSION, "-InstallDir", str(DOTNET_DIR)],
                check=True
            )
        else:
            # Use bash script on Unix-like systems
            install_script = "dotnet-install.sh"
            urllib.request.urlretrieve(
                "https://dot.net/v1/dotnet-install.sh",
                install_script
            )
            os.chmod(install_script, 0o755)
            
            subprocess.run(
                [f"./{install_script}", "-c", REQUIRED_DOTNET_VERSION, "-InstallDir", str(DOTNET_DIR)],
                check=True
            )
        
        dotnet_path = DOTNET_DIR / ("dotnet.exe" if platform.system() == "Windows" else "dotnet")
        
        # Verify installation
        result = subprocess.run(
            [str(dotnet_path), "--version"],
            capture_output=True,
            text=True,
            check=True
        )
        print(f"Using .NET version: {result.stdout.strip()}")
        
        return str(dotnet_path)
    
    except Exception as e:
        print(f"Error installing .NET: {e}")
        sys.exit(1)


def run_dotnet_command(dotnet_path: str, command: str) -> None:
    """Execute the appropriate dotnet command"""
    try:
        if command == "publish":
            print("Publishing project to ./dist...")
            subprocess.run(
                [dotnet_path, "publish", PROJECT_PATH, "-c", "Release", "-o", "dist"],
                check=True
            )
            print("Successfully published to ./dist")
        
        elif command == "watch":
            print("Starting project with hot reload...")
            print("Press Ctrl+C to stop watching...")
            subprocess.run(
                [dotnet_path, "watch", "--project", PROJECT_PATH]
            )
        
        elif command == "run":
            print("Running project...")
            print("Press Ctrl+C to stop...")
            subprocess.run(
                [dotnet_path, "run", "--project", PROJECT_PATH]
            )
        
        elif command == "build":
            print("Building project...")
            subprocess.run(
                [dotnet_path, "build", PROJECT_PATH],
                check=True
            )
            print("Successfully built project")
        
        else:
            print(f"Unknown command: {command}")
            print_usage()
            sys.exit(1)
    
    except KeyboardInterrupt:
        # Handle Ctrl+C gracefully for interactive commands
        print("\n\nShutdown requested. Exiting cleanly...")
        sys.exit(0)
    
    except subprocess.CalledProcessError:
        print(f"Error: Failed to {command} project")
        sys.exit(1)


def print_usage() -> None:
    """Print usage information"""
    print("Usage: python build.py [build|publish|watch|run]")
    print("  build   - Build the project (default)")
    print("  publish - Publish the project to ./dist")
    print("  watch   - Run with hot reload")
    print("  run     - Run the project")


def main() -> None:
    """Main entry point"""
    # Parse command argument
    command = sys.argv[1] if len(sys.argv) > 1 else "build"
    
    if command not in ["build", "publish", "watch", "run"]:
        print(f"Unknown command: {command}")
        print_usage()
        sys.exit(1)
    
    # Setup prerequisites
    print("Setting up build environment...")
    setup_tailwindcss()
    
    # Check and setup .NET
    dotnet_path = check_dotnet()
    if not dotnet_path:
        dotnet_path = install_dotnet()
    
    # Execute command
    run_dotnet_command(dotnet_path, command)


if __name__ == "__main__":
    main()
