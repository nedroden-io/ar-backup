#!/bin/zsh
# Build the solution

dotnet build ArBackup.sln
if [ $? -ne 0 ]; then
  echo "Build failed. Exiting."
  exit 1
fi

# Run the CLI project with all passed arguments

dotnet run --project ArBackup.Cli/ArBackup.Cli.csproj -- "$@"

