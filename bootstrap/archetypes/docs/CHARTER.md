# Archetypes charter

`archetypes` is a companion **preset library** for games: profession, temperament, and fantasy-clan **mind seeds** that map into Personality Engine constructor arguments.

**Home:** https://github.com/RossSim/archetypes (public MIT)

**Depends on:** [Personality Engine](https://github.com/RossSim/personality-engine) 0.6.1+ (`OceanTraits`, Piaget stage, Erikson stage, operant seeds, enabled providers).

Personality Engine is runtime middleware (events in → snapshot out). Archetypes is **authoring data**: tables, builders, and lore. It does not add `IAffectProvider` implementations.

## What is fixed

- **Shape:** preset id in; PE constructor args out (`OceanTraits`, optional stages, operant seeds, enabled provider ids).
- **No new psychology in this repo.** Providers stay in [personality-engine](https://github.com/RossSim/personality-engine).
- **No IQ, g, or WAIS-style composites.** Ability differences use structure (Piaget), training (Skinner), and trait bands.
- **Public catalog:** fantasy clans and generic professions only. No real-world race, ethnicity, or national cognitive rank tables. No MBTI.
- **Per-knob citations.** Fiction vs science split on clan presets. Project-convention numbers labeled as such.
- **Not a clinic.** This is entertainment and research middleware, not a psychometric test or a medical device. See [`DISCLAIMER.md`](../DISCLAIMER.md).
- **GitHub stays public.** Do not put private issue-tracker URLs, project keys, or ticket ids in this repository, GitHub pull requests, issues, commit messages, Releases, or release notes.
- **Docs live in this repository.** README, `docs/`, and the changelog are the product docs. Do not maintain a separate wiki.
- **This repository is self-contained.** Do not name other private or internal projects, studios, or repos.

## Sequencing (catalog-first)

Author **tables before code**. Profession and clan catalogs (JSON or markdown with fiction / knobs / citations) come first so `MindPreset` is inferred from real entries. `PresetBuilder` and NuGet ship only after two catalogs show which fields Personality Engine can actually consume.

Do not invent knobs Personality Engine cannot take yet (Sternberg, RIASEC, Bandura wait for those providers in personality-engine).

Intended later versions: [`ROADMAP.md`](ROADMAP.md). That order is not a rewrite of this charter.
