# Roadmap

Archetypes maps **preset ids** into Personality Engine compositions. Direction only — not a contract. Patch releases fix docs and presets without schema breaks.

Current status: **0.0.0 skeleton** (docs and repo layout; no library artifact).

Personality Engine home: https://github.com/RossSim/personality-engine

## Timeline

```mermaid
flowchart LR
  v00["0.0 skeleton"] --> v01["0.1 core"]
  v01 --> v02["0.2 professions"]
  v02 --> v03["0.3 fantasy clans"]
  v03 --> v10["1.0 stable preset schema"]
```

## Intended versions

| Version | What a host would get |
| --- | --- |
| 0.0 | README, roadmap, design, disclaimer, empty repo layout |
| 0.1 | `MindPreset` record, `PresetBuilder` → `AffectEngine`, tests against hand-authored PE seeds |
| 0.2 | Profession catalog (smith, scout, clerk, …) from OCEAN job correlates + operant seeds — not profession→IQ |
| 0.3 | Temperament catalog (Thomas & Chess bands) + jitter options |
| 0.4 | Fantasy clan presets as compositions (cognitive stage + operant history + traits) |
| 0.5 | Embedded JSON presets + `docs/CITATIONS.md` per knob |
| 1.0 | Frozen `MindPreset` schema and builder contract; NuGet `Archetypes.Core` |

## Depends on Personality Engine

| PE capability | Archetypes use |
| --- | --- |
| `OceanTraits`, compositions | Trait bands in presets |
| Piaget `CognitiveStage` | Clan cognitive architecture |
| Skinner operant bags | Profession training history |
| Optional layers | Preset lists which providers to enable |
| Future: Holland RIASEC, Sternberg domains | Vocation and ability presets when PE ships providers |

Track PE provider work in the personality-engine repo and private tracker. This repo consumes PE APIs only.

## Not on this roadmap

- Real-world race, ethnicity, or national cognitive rank tables
- IQ, g, or WAIS-style composite scores
- MBTI or four-letter type inventories
- New affect providers (file those in personality-engine)
- Unity samples (games reference both NuGet packages)

## Controversy guardrails (product)

- Public catalog: fantasy clans and generic professions only
- Every preset documents **per-knob** citations in fiction vs science sections
- Ability differences = structure + training + trait bands, not “less intelligent people”

See [Design](DESIGN.md).
