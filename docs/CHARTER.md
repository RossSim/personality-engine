# Personality Engine charter

`personality-engine` is a reusable C# socio-emotional engine for games.

**Home:** https://github.com/textide/personality-engine (public MIT)
**Board:** https://prayingforradar.atlassian.net/jira/software/projects/PE/summary
**Jira:** project `PE`, charter ticket [PE-1](https://prayingforradar.atlassian.net/browse/PE-1)

This is a standalone library: not an AviationStuff `Assets/Systems/` folder, and not a Unity project. Games consume it later (Unity, other engines, labs) through a `netstandard2.1` API.

Inspired by FAtiMA, built from scratch. The product is **modular middleware**, not a single frozen psychology stack.

## What is fixed

- **Shape:** events in; an affect snapshot and optional action weights out.
- **No LLM in the core.** Language models may sit outside as a host, never as a required provider.
- **Every provider cites a source.** Coefficients, thresholds, and mappings that are project conventions are labeled as such (same rule as GE3).
- **Hosts compose; they do not fork** the library to add a theory, a layer, or a method.

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

Gebhard ALMA (2005) is the **first wiring** among those three (OCEAN → PAD mood → OCC emotion). It is one cited composition, not the definition of the engine.

### Allowed without a charter change

- **New layers** — values, relationships, morality, motives, **learning** (operant), or anything a host needs that is not personality, mood, or emotion.
- **Alternate providers on an existing layer** — e.g. HEXACO instead of, or beside, OCEAN.
- **Supplemental sources or methods on the same layer** — e.g. OCEAN + Dark Triad; PAD plus extra mood axes; a second OCC variant; a non-ALMA personality→mood mapping.
- **Cited mappings between layers** — ALMA’s OCEAN→PAD is the first mapping provider, not the only allowed glue.

A host may enable a subset. Downstream code must not assume OCEAN, PAD, or OCC are present.

## First coding slice

Prove the pipeline with the default ALMA-style composition. First numeric test (see [PE-4](https://prayingforradar.atlassian.net/browse/PE-4), [PE-5](https://prayingforradar.atlassian.net/browse/PE-5)): Gebhard’s example `O=0.4 C=0.8 E=0.6 A=0.3 N=0.4` → `P=0.38 A=-0.08 D=0.50` (slightly relaxed). That test belongs to the OCEAN and PAD providers plus their mapping. It does not freeze the set of theories.

## Jira conventions (mirror AV / GE3 / KAN)

- **Epic** = milestone or coding branch name. When Done, seal it — do not parent new tickets onto it.
- **Story** = shippable slice under an epic.
- **Task** = small distinct work; **Bug** = defects; **Feature** = broader functionality when a Story is too narrow.
- JQL: `project = PE`

The current coding epic is [PE-3](https://prayingforradar.atlassian.net/browse/PE-3). Provider interfaces: [PE-6](https://prayingforradar.atlassian.net/browse/PE-6). This charter ticket ([PE-1](https://prayingforradar.atlassian.net/browse/PE-1)) is meta, not a milestone. After it is accepted, do not parent implementation work here.

Moved from AviationStuff [AV-6](https://prayingforradar.atlassian.net/browse/AV-6).
