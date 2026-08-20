# Testing

From the repository root:

```bash
dotnet test
```

The core library targets `netstandard2.1`. Tests target `net8.0`, so a .NET 8 runtime is required.

Pull requests and pushes to `main` run the same `dotnet test` command on GitHub Actions, then run the console sample.

```bash
dotnet run --project samples/AlmaConsole
```

The sample is a host, not a provider. It is not packed with `PersonalityEngine.Core`.

This repository is a C# library. It is not a Unity project.
