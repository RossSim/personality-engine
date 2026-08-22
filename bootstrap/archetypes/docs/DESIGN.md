# Design

How Archetypes sits beside Personality Engine without forking psychology.

## Split of responsibility

| Layer | Personality Engine | Archetypes |
| --- | --- | --- |
| Runtime tick | `AffectEngine.Tick` | — |
| Cited theory | `IAffectProvider` implementations | — |
| Starting profile | Constructor args | `MindPreset` tables |
| Lore names | — | `philobrain-scholar`, `trog-warrior` |
| Builder | `AlmaComposition.Create(...)` | `PresetBuilder.Build(preset)` |

## MindPreset (planned)

Conceptual shape for 0.1:

```csharp
public sealed record MindPreset(
    string Id,
    string Category,              // profession, temperament, clan
    OceanTraits Traits,
    CognitiveStage? Stage,
    PsychosocialStage? IdentityStage,
    IReadOnlyDictionary<string, float>? OperantSeeds,
    string[] EnabledProviderIds,
    IReadOnlyList<CitationRef> Rationale);
```

`CitationRef` ties each knob to a paper or labels it **project convention**.

## Fantasy vs science docs

Each clan preset should split:

1. **Fiction** — what players see in the world (“Philobrain clan prizes hypotheticals”)
2. **Knobs** — Piaget formal operational, high Openness, strong explore operants
3. **Citations** — Piaget 1950; McCrae & Costa 2008; project convention for operant strengths

Avoid one bibliography backing the whole archetype.

## Cognitive difference without IQ

| Player-visible behavior | Knob |
| --- | --- |
| Won’t follow hypothetical clues | `CognitiveStage.Preoperational`, `hypothetical` flag off |
| Repeats old tactic | Skinner strength on `repeat-protocol` |
| Won’t try the puzzle | Self-efficacy channel when PE ships Bandura provider |
| Curious vs rigid | OCEAN Openness |
| Trained for the job | Operant history + Conscientiousness |

## Tiers and jitter

- **Named** — full preset composition
- **Ambient** — personality + mood only, ± jitter on traits
- **Crowd** — shared district seed (see PE APPLICATIONS.md cost notes)

## Multiplayer note

Presets produce local PE state. Games replicate `AffectPersist` or authoritative channels on the server — Archetypes does not handle netcode.
