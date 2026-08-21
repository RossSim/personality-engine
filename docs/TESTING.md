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
dotnet run --project samples/AlmaTimeline -- --serve
dotnet run --project samples/UtilityTint
```

The samples are hosts, not providers. They are not packed with `PersonalityEngine.Core`. The timeline sample writes `samples/AlmaTimeline/index.html`. Serve it with `--serve` to check OCC events, set intensity, stagger 0–3s, and press **Run Test** (10s of 1s ticks, line chart plus a metrics×time table). Idle decay, save/load, host events, and Utility-AI tint: [`HOSTING.md`](HOSTING.md).

This repository is a C# library. It is not a Unity project.
