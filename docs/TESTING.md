# Testing

From the repository root:

```bash
dotnet test
```

The core library targets `netstandard2.1`. Tests target `net8.0`, so a .NET 8 runtime is required.

Pull requests and pushes to `main` run the same `dotnet test` command on GitHub Actions, then run the console sample.

```bash
dotnet run --project samples/AlmaConsole
dotnet run --project samples/AlmaTimeline
```

The samples are hosts, not providers. They are not packed with `PersonalityEngine.Core`. The timeline sample writes `samples/AlmaTimeline/index.html` (open in a browser): 10s of 1s ticks, line chart plus a metrics×time table.

This repository is a C# library. It is not a Unity project.
