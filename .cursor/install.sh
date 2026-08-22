#!/usr/bin/env bash
# Idempotent Cloud Agent setup for the Personality Engine C# library.
# Installs the .NET 8 SDK (tests target net8.0) if missing, then restores
# and builds the solution so the working tree is ready for `dotnet test`.
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

sudo_cmd=""
if [ "$(id -u)" -ne 0 ]; then
  sudo_cmd="sudo"
fi

if ! command -v dotnet >/dev/null 2>&1; then
  echo "Installing .NET 8 SDK..."
  export DEBIAN_FRONTEND=noninteractive
  $sudo_cmd apt-get update -qq
  $sudo_cmd apt-get install -y -qq dotnet-sdk-8.0
fi

export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

echo "Using $(dotnet --version) SDK"

# Warm NuGet restore + build so the first `dotnet test` is fast and offline-safe.
dotnet restore PersonalityEngine.sln
dotnet build PersonalityEngine.sln -c Release --no-restore
