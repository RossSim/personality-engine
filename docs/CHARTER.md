# Personality Engine charter

`personality-engine` is a reusable C# socio-emotional engine for games.

**Home:** https://github.com/RossSim/personality-engine (public MIT)

This is a standalone C# library, not a Unity project. Games consume it later (Unity, other engines, labs) through a `netstandard2.1` API.

Inspired by FAtiMA, built from scratch. The product is **modular middleware**, not a single frozen psychology stack.

## What is fixed

- **Shape:** events in; an affect snapshot and optional action weights out.
- **No LLM in the core.** Language models may sit outside as a host, never as a required provider.
- **Every provider cites a source.** Coefficients, thresholds, and mappings that are project conventions are labeled as such.
- **Not a clinic.** The library is game/research middleware. It is not a psychometric test or a medical device. See [`DISCLAIMER.md`](../DISCLAIMER.md).
- **Hosts compose; they do not fork** the library to add a theory, a layer, or a method.
- **GitHub stays public.** Do not put private issue-tracker URLs, project keys, or ticket ids in this repository, GitHub pull requests, issues, commit messages, Releases, or release notes.
- **Docs live in this repository.** README, `docs/`, and the changelog are the product docs. Do not maintain a separate wiki.
- **This repository is self-contained.** Do not name other private or internal projects, studios, or repos.

## What is modular

The engine is a **composition of providers** grouped into **layers**. Layers and providers can be added, replaced, or run side by side. The charter names starter layers so v0.1 has a default composition. It does not cap the set of layers or lock a layer to one paper.

### Starter layers (default composition, not a ceiling)

| Layer | Role | First provider (v0.1) | Primary citations |
| --- | --- | --- | --- |
| Stable personality | Slow traits | Big Five / OCEAN | McCrae & Costa |
| Mood | Medium-term affect | PAD | Mehrabian; Gebhard ALMA 2005 |
| Emotion | Momentary affect | OCC | Ortony, Clore & Collins |
| Meaning (optional) | Known / unknown / knower | Peterson Maps of Meaning | Peterson (1999); Peterson & Flanders (2002) |
| Learning (optional) | Operant repertoire | Skinner three-term contingency | Skinner (1953); Ferster & Skinner (1957) |
| Cognition (optional) | Schemas and stages | Piaget equilibration | Piaget (1950, 1952, 1985); Inhelder & Piaget (1958) |
| Identity (optional) | Psychosocial crises | Erikson eight ages | Erikson (1963, 1968, 1982) |

Gebhard ALMA (2005) is the **first wiring** among those three (OCEAN → PAD mood → OCC emotion). It is one cited composition, not the definition of the engine.

### Allowed without a charter change

- **New layers** — values, relationships, morality, motives, **learning** (operant), **cognition** (Piaget), **identity** (Erikson), or anything a host needs that is not personality, mood, or emotion.
- **Alternate providers on an existing layer** — e.g. HEXACO instead of, or beside, OCEAN.
- **Supplemental sources or methods on the same layer** — e.g. OCEAN + Dark Triad; PAD plus extra mood axes; a second OCC variant; a non-ALMA personality→mood mapping.
- **Cited mappings between layers** — ALMA’s OCEAN→PAD is the first mapping provider, not the only allowed glue.

A host may enable a subset. Downstream code must not assume OCEAN, PAD, or OCC are present.

## First coding slice (shipped)

The pipeline with the default ALMA-style composition is in the library. First numeric test: Gebhard’s example `O=0.4 C=0.8 E=0.6 A=0.3 N=0.4` → `P=0.38 A=-0.08 D=0.50` (slightly relaxed). That test belongs to the OCEAN and PAD mapping providers. `PadMood` and `OccEmotion` sit on that baseline in the default composition. None of this freezes the set of theories.

Current published library: **0.6.1** (plain-language front door, diagrams, playable examples host). Next coding work is a new milestone, not a rewrite of this charter.
