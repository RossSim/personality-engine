# Archetypes

[![License: MIT](https://img.shields.io/badge/license-MIT-yellow.svg)](LICENSE)

Preset catalogs for [Personality Engine](https://github.com/RossSim/personality-engine): profession, temperament, and fantasy-clan **mind seeds** that map into PE constructor arguments.

Personality Engine is runtime middleware (events in → snapshot out). Archetypes is **authoring data**: tables, builders, and lore that turn “village blacksmith” or “Philobrain scholar” into `OceanTraits`, Piaget stage, operant seeds, and which providers to enable. It does not add new psychology providers to PE.

```mermaid
flowchart LR
  preset[Archetype preset]
  builder[PresetBuilder]
  pe[Personality Engine]
  game[Your game host]
  preset --> builder --> pe --> game
```

## What this is

- Cited **defaults** per knob (traits, cognitive stage, training history), not a single IQ score
- Optional **jitter** for named heroes vs ambient NPCs
- **Fantasy** clan and profession ids — not real-world race or ethnicity presets in the public catalog

## What this is not

- Not a psychometric test, clinic, or personality type inventory (no MBTI)
- Not an `IAffectProvider` implementation (those stay in personality-engine)
- Not IQ or g-factor channels — use Piaget structure, Sternberg domains (when PE ships them), operant history, and trait bands instead

See [Disclaimer](DISCLAIMER.md).

## Status

**Skeleton.** Repo structure, roadmap, and design notes only. No NuGet package yet.

Depends on Personality Engine **0.6.1+** (`netstandard2.1`).

## Documentation

| Doc | What it is |
| --- | --- |
| [Roadmap](docs/ROADMAP.md) | Intended versions and scope |
| [Design](docs/DESIGN.md) | MindPreset shape, catalogs, guardrails |
| [Changelog](CHANGELOG.md) | Version notes |
| [Disclaimer](DISCLAIMER.md) | Entertainment middleware; not a test |

## Planned layout

```text
archetypes/
├── docs/
├── presets/          # JSON or embedded catalogs (profession, temperament, fantasy)
├── src/
│   Archetypes.Core/   # MindPreset, PresetBuilder → AffectEngine
│   Archetypes.Presets/
└── tests/
```

## License

[MIT](LICENSE). Personality Engine is also MIT; cite academic sources per preset knob in `docs/CITATIONS.md` when that file lands.
