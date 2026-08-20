# Architecture

Charter: [`CHARTER.md`](CHARTER.md). Design uses: [`APPLICATIONS.md`](APPLICATIONS.md). Citations: [`CITATIONS.md`](CITATIONS.md).

The library is a **composition root** plus **providers**. The core never hard-codes “personality is five OCEAN floats, mood is PAD, emotion is OCC.” Those are the first providers in the default composition.

## Pipeline

```
WorldEvent  →  AffectEngine.Tick(event, dt)
                    │
                    ├─ each enabled IAffectProvider (order is composition order)
                    │     reads AffectSnapshot, writes AffectDelta
                    │
                    └─ AffectSnapshot  →  optional IActionWeighter  →  action weights
```

Hosts send typed events (and delta time). They read a snapshot and, if they asked for it, weights over candidate actions. They do not own the psychology.

## Providers

An `IAffectProvider` declares:

- a stable id (`ocean`, `pad-mood`, `occ`, `hexaco`, …)
- a **layer** (open set; well-known starters: `Personality`, `Mood`, `Emotion`)
- a **citation** (paper / book; project conventions flagged separately)
- `Contribute(event, dt, snapshot) → AffectDelta`

Providers are **additive**. Two personality providers can both run. A mapping (OCEAN→PAD) is itself a provider with its own citation, not hidden glue.

### Replace vs supplement

| Host intent | Composition |
| --- | --- |
| Replace OCEAN with HEXACO | HEXACO personality provider only |
| Supplement OCEAN | OCEAN + HEXACO (or Dark Triad, etc.), each namespaced in the snapshot |
| Keep OCEAN, change mood math | OCEAN + a different mood provider and/or mapping |
| Add a layer | e.g. a `Values` provider; existing providers keep working |

Downstream code looks up **named channels**. Missing channels are absent, not errors. Action weighters must tolerate a host that omitted mood or emotion.

## Snapshot

`AffectSnapshot` is a bag of named channels, not a fixed struct:

- key: `layer.provider.channel` (example: `personality.ocean.openness`, `mood.pad.pleasure`)
- value: typically a float in a documented range for that provider
- metadata: which providers ran this tick

Optional **projectors** may derive a convenience view (for example PAD) when the required inputs exist. A projector is cited; it is not a back-door requirement that every composition speak PAD.

## Default composition (v0.1)

Order:

1. `OceanPersonality` — McCrae & Costa Big Five
2. `OceanToPadMapping` — Gebhard ALMA 2005 baseline on `mood.pad.*`
3. `PadMood` — current PAD on `mood.pad-mood.*`, pulled toward that baseline with exponential decay over `dt` (rate is project convention)
4. `OccEmotion` — OCC appraisal of events (later slice; not required to compile the first test)

Optional supplement ([`peterson.md`](peterson.md)):

5. `StabilityPlasticityProvider` — DeYoung, Peterson & Higgins (2002) metatraits
6. `OrderChaosMeaningProvider` — Peterson (1999) / CMT meaning layer (`meaning.peterson-maps.*`)
7. `PetersonMeaningWeighter` — explore vs defend vs integrate vs withdraw

Optional supplement ([`skinner.md`](skinner.md)) — **new `learning` layer**, not a personality provider:

8. `OperantLearningProvider` — Skinner (1953) / Ferster & Skinner (1957) operant strengths
9. `OperantWeighter` — strength × deprivation × SD

Optional supplement ([`piaget.md`](piaget.md)) — **new `cognition` layer**, not a personality or learning provider:

10. `PiagetEquilibrationProvider` — Piaget schemas, equilibration, host-set stages (`cognition.piaget-equilibration.*`)
11. `PiagetCognitionWeighter` — play vs imitate vs accommodate vs explore

Optional supplement ([`erikson.md`](erikson.md)) — **new `identity` layer**, not a personality, cognition, or learning provider:

12. `EriksonPsychosocialProvider` — eight ages, syntonic/dystonic ratio, ego identity (`identity.erikson-psychosocial.*`)
13. `EriksonIdentityWeighter` — explore vs commit vs care vs withdraw

The first green test is (1)+(2): Gebhard’s numeric example. `PadMood` is optional: omit it and mood stays the static mapped baseline. OCC ships as a follow-on provider. The provider contract is `IAffectProvider` plus named-channel snapshots.

## Out of scope for the core

- Unity scene objects or host-engine bindings
- LLM calls, prompt templates, or token I/O
- A FAtiMA port or binary compatibility with FAtiMA-Toolkit

Those may wrap this library later. They are not providers inside it.
