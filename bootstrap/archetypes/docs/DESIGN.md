# Design

How Archetypes sits beside Personality Engine without forking psychology.

## Catalog-first

Tables come first. A profession or clan file in `presets/` is a **row**: fiction, knobs Personality Engine can already take, and per-knob citations. `MindPreset` and `PresetBuilder` are inferred from those rows after two catalogs exist. Do not invent knobs PE cannot consume yet.

## Split of responsibility

| Layer | Personality Engine | Archetypes |
| --- | --- | --- |
| Runtime tick | `AffectEngine.Tick` | — |
| Cited theory | `IAffectProvider` implementations | — |
| Starting profile | Constructor args | Catalog tables, later `MindPreset` |
| Lore names | — | `philobrain-scholar`, `trog-warrior` |
| Builder (later) | `AlmaComposition.Create(...)` | `PresetBuilder.Build(preset)` |

## Catalog row (now)

Every public entry should be able to carry:

- `id`, `category` (`profession`, `clan`, later `temperament`)
- `traits` — five OCEAN 0..1, or a documented band plus a midpoint
- `operantSeeds` — action-id → strength for training history
- `enabledProviderIds` — which PE providers this seed expects
- `citations` — per knob: paper **or** `project convention`
- optional `cognitiveStage`, `identityStage`
- optional `jitter` notes (named vs ambient)
- a short **fiction** blurb separate from knobs

Markdown or JSON is fine until the builder exists.

## MindPreset (after catalogs)

Expected builder shape once tables prove the fields:

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

`CitationRef` ties each knob to a paper or labels it **project convention**. Drop or add fields if the catalogs show the record is wrong.

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
| Won’t try the puzzle | Self-efficacy channel when PE ships a Bandura provider |
| Curious vs rigid | OCEAN Openness |
| Trained for the job | Operant history + Conscientiousness |

## Tiers and jitter

- **Named** — full preset composition
- **Ambient** — personality + mood only, ± jitter on traits
- **Crowd** — shared district seed (Personality Engine applications notes: cost of one instance per walker)

## Multiplayer note

Presets produce local PE state. Games replicate persist blobs or authoritative channels on the server — Archetypes does not handle netcode.
