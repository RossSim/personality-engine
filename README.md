# Personality Engine

[![License: MIT](https://img.shields.io/badge/license-MIT-yellow.svg)](LICENSE)
[![Release](https://img.shields.io/github/v/release/RossSim/personality-engine)](https://github.com/RossSim/personality-engine/releases/latest)
[![Target](https://img.shields.io/badge/target-netstandard2.1-512BD4)](https://github.com/RossSim/personality-engine/releases)

A reusable C# socio-emotional engine for games. Events go in; a named-channel **affect snapshot** and optional **action weights** come out.

It is **modular middleware**, not a frozen psychology stack. Personality, mood, and emotion are the default layers. Hosts add, replace, or omit providers. Every provider cites a source. There is no LLM in the core.

Current library version: **0.1.0**. Downloads and notes: [Releases](https://github.com/RossSim/personality-engine/releases). History: [CHANGELOG.md](CHANGELOG.md).

## Install

**NuGet package** (from a GitHub Release asset):

```bash
dotnet add package PersonalityEngine.Core --source /path/to/downloaded/nupkg-folder
```

Or add a `PackageReference` after placing `PersonalityEngine.Core.0.1.0.nupkg` in a local feed.

**DLL:** unzip the `PersonalityEngine.Core.*.zip` asset from the [latest release](https://github.com/RossSim/personality-engine/releases/latest) and reference `PersonalityEngine.Core.dll` (`netstandard2.1`, Unity-consumable later; this repo is not a Unity project).

**From source:**

```bash
git clone https://github.com/RossSim/personality-engine.git
cd personality-engine
dotnet test
```

## Quick start

```csharp
using PersonalityEngine;
using PersonalityEngine.Providers.Erikson;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;
using PersonalityEngine.Providers.Peterson;
using PersonalityEngine.Providers.Piaget;
using PersonalityEngine.Providers.Skinner;

var mood = AlmaComposition.Create(OceanTraits.GebhardExample);
mood.Tick(WorldEvent.Tick);
mood.Tick(new WorldEvent(OccEmotion.JoyKind, 1f));

var meaning = PetersonComposition.Create(OceanTraits.GebhardExample);
meaning.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, 0.8f));

var operant = SkinnerComposition.Create(new[] { "forage", "idle" });
operant.Tick(new WorldEvent(OperantLearningProvider.EmitKind, 1f, "forage"));

var cognition = PiagetComposition.Create(CognitiveStage.ConcreteOperational);
cognition.Tick(new WorldEvent(PiagetEquilibrationProvider.EncounterKind, 0.2f));

var identity = EriksonComposition.Create(PsychosocialStage.IdentityVsRoleConfusion);
identity.Tick(new WorldEvent(EriksonPsychosocialProvider.ExploreKind, 1f));
```

Hosts compose only the providers they want. Missing channels are **absent**, not errors.

## Layers (v0.1)

| Layer | Role | First provider | Citation |
| --- | --- | --- | --- |
| Personality | Slow traits | Big Five / OCEAN | McCrae & Costa |
| Mood | Medium-term affect | PAD baseline + optional dynamics | Mehrabian; Gebhard 2005 |
| Emotion | Momentary affect | OCC (host-tagged types) | Ortony, Clore & Collins |
| Meaning (optional) | Known / unknown / knower | Peterson Maps of Meaning | Peterson (1999); CMT 2002 |
| Learning (optional) | Operant repertoire | Three-term contingency | Skinner (1953); Ferster & Skinner (1957) |
| Cognition (optional) | Schemas and stages | Equilibration | Piaget (1950, 1985) |
| Identity (optional) | Psychosocial crises | Eight ages | Erikson (1963, 1968) |

Gebhard ALMA (2005) is the **first wiring** among personality, mood, and emotion — not the definition of the engine. Numeric game knobs are labeled **project convention** and are not attributed to a paper.

Inspired by FAtiMA, built from scratch. Not a FAtiMA fork.

## Documentation

| Doc | What it is |
| --- | --- |
| [Charter](docs/CHARTER.md) | What is fixed vs modular |
| [Applying it in games](docs/APPLICATIONS.md) | Design-facing uses: RTS, RPG, FPS, sims, NPCs — not the C# API |
| [Architecture](docs/ARCHITECTURE.md) | Pipeline, snapshot keys, composition |
| [Citations](docs/CITATIONS.md) | Source registry |
| [Peterson](docs/peterson.md) · [Skinner](docs/skinner.md) · [Piaget](docs/piaget.md) · [Erikson](docs/erikson.md) · [OCC](docs/occ.md) | Academic review and in-module mapping |
| [Testing](docs/TESTING.md) | How to run the test suite locally |
| [Releasing](docs/RELEASING.md) | How versions and GitHub Releases are cut |
| [Changelog](CHANGELOG.md) | Notes for every version |

## Versioning and releases

Each published version has:

1. A `Version` in `PersonalityEngine.Core` (currently `0.1.0`)
2. A `CHANGELOG.md` section for that version
3. A git tag `vMAJOR.MINOR.PATCH`
4. A [GitHub Release](https://github.com/RossSim/personality-engine/releases) with those notes and downloadable `.nupkg` / `.zip` assets

Pushing a `v*` tag runs the release workflow. See [docs/RELEASING.md](docs/RELEASING.md).

## License

[MIT](LICENSE)

## Development

```bash
dotnet test
```

This repository is public and self-contained. Do not include private issue-tracker URLs, project keys, or ticket ids in commits, pull requests, issues, or releases. Do not name other private or internal projects.
