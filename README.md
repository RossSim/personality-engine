# personality-engine

A reusable C# socio-emotional engine for games. Public MIT. [Charter](docs/CHARTER.md).

The library is **modular middleware**: events in, an affect snapshot and optional action weights out. Stable personality, mood, and emotion are the default layers, not the only ones. Each layer is a citable **provider** that a host can replace, supplement, or omit. Further layers (values, relationships, and so on) plug in the same way.

Default v0.1 composition follows Gebhard ALMA (2005) wiring — Big Five / OCEAN, PAD mood, OCC emotion — as the first providers, not as a freeze on theory. There is no LLM in the core.

| | |
| --- | --- |
| **Repo** | https://github.com/textide/personality-engine |
| **Jira** | [PE board](https://prayingforradar.atlassian.net/jira/software/projects/PE/summary) · charter [PE-1](https://prayingforradar.atlassian.net/browse/PE-1) |
| **Docs** | [Charter](docs/CHARTER.md) · [Architecture](docs/ARCHITECTURE.md) · [Citations](docs/CITATIONS.md) · [Peterson module](docs/peterson.md) · [Skinner module](docs/skinner.md) · [Cursor start](docs/CURSOR_START.md) |

## Status

`PersonalityEngine.Core` (`netstandard2.1`) includes provider interfaces ([PE-6](https://prayingforradar.atlassian.net/browse/PE-6)), OCEAN → PAD (Gebhard’s example), a Peterson **meaning** supplement ([PE-7](https://prayingforradar.atlassian.net/browse/PE-7)), and a Skinner **learning** layer ([PE-8](https://prayingforradar.atlassian.net/browse/PE-8)): operant strengths under a three-term contingency.

## Build / test

```bash
dotnet test
```

Target framework: `netstandard2.1` (Unity-consumable later; this repo is not a Unity project).

```csharp
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Peterson;
using PersonalityEngine.Providers.Skinner;

var meaning = PetersonComposition.Create(OceanTraits.GebhardExample);
meaning.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, 0.8f));

var operant = SkinnerComposition.Create(new[] { "forage", "idle" });
operant.Tick(new WorldEvent(OperantLearningProvider.EmitKind, 1f, "forage"));
```
