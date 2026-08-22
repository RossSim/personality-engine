# Changelog

All notable changes to Personality Engine are documented here.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
Versions follow [SemVer](https://semver.org/). GitHub Releases use the same notes.

## [Unreleased]

## [0.6.1] - 2026-08-21

Plain-language front door, diagrams, and a playable examples host. Core API is unchanged.

### Added

- Examples page and HTML host (`samples/Examples`): three game stories from real ticks (raid, shopkeeper visits, person-to-nation)

### Changed

- Plain-language applications intro for designers, programmers, and people using an AI to make a game
- Mermaid diagrams on the applications, hosting, and architecture pages (tables kept)
- README opening matches that same plain-language explanation
- Charter and start notes: current published library is 0.6.1

## [0.6.0] - 2026-08-21

Compound OCC: host-tagged gratification, gratitude, anger, and remorse.

### Added

- OCC well-being+attribution compounds: gratification, gratitude, anger, remorse (host-tagged; same decay and persist path as other OCC channels)
- `HostEvents.Anger` / `Gratitude` keep the other id as `Target`; `Gratification` / `Remorse` are self-attribution
- AlmaTimeline checkboxes for fortune-of-others and the four compounds

### Changed

- Charter and start notes: current published library is 0.6.0

## [0.5.2] - 2026-08-21

Public disclaimer and citation hygiene. MIT is unchanged.

### Added

- [`DISCLAIMER.md`](DISCLAIMER.md): not a psychometric test or medical device; cited authors have not endorsed the library; MIT grant unchanged
- LICENSE and DISCLAIMER packed with the Core nupkg and release zip

### Changed

- Bibliographic citations completed (McCrae & Costa 2008; Mehrabian 1974/1996; Gebhard AAMAS 2005 DOI; FAtiMA 2005/2014/2022 papers; named-but-not-implemented HEXACO, Dark Triad, Maslow, Marcia, Heider)

## [0.5.1] - 2026-08-21

### Fixed

- Restored the 0.4.0 changelog heading so 0.5.0 notes do not swallow the previous cut

## [0.5.0] - 2026-08-21

Social affect: fortune-of-others OCC, optional dyad liking, and a tint that does not steal Pick.

### Added

- OCC fortune-of-others types: happy-for, pity, resentment, gloating (host-tagged; same decay and persist path as other OCC channels)
- `HostEvents.HappyFor` / `Pity` / `Resent` / `Gloat` wrap those kinds and keep the other id as `Target`
- Optional `relationship` layer: `DyadProvider` pairwise liking toward a named other (`relationship.dyad.liking:{id}`), persist, `HostEvents.Like` / `Dislike`
- `DyadWeighter` tints `approach:{other}` / `avoid:{other}`; host Pick stays
- Console sample: `samples/SocialTint`
- Relationship notes: [`docs/dyad.md`](docs/dyad.md)

### Changed

- Charter and start notes: current published library is 0.5.0

## [0.4.0] - 2026-08-21

Host I/O: persist, idle ticks, named events, and a Utility-AI tint that does not steal Pick.

### Added

- Persist: `Export` / `Import` round-trip snapshot channels plus PadMood, OCC, and Skinner internal state
- `AffectEngine.Tick(dt)` idle overload (same as `Tick(WorldEvent.Tick, dt)`)
- `HostEvents` catalog (need-met, harm, threat, threat-passed, self-credit, self-blame) wrapping existing OCC kinds
- `UtilityTintWeighter` and `HostChooser` so a host Utility AI keeps Pick; PE adds a small additive tint
- Console sample: `samples/UtilityTint`
- Hosting notes: [`docs/HOSTING.md`](docs/HOSTING.md)

### Changed

- Charter and start notes treat the first coding slice as shipped; current published library is 0.4.0
- Charter and start notes: product docs live in this repository; the private tracker is Jira only

## [0.3.0] - 2026-08-20

First in-repo hosts and PR test CI. OCC decay no longer leaves a stale last pulse in the snapshot.

### Added

- CI: run the test suite on pull requests and `main` pushes
- Console sample host (`samples/AlmaConsole`): ticks the default composition and prints named-channel snapshot values
- Timeline host (`samples/AlmaTimeline`): 10s run at 1s ticks, HTML chart and values table, OCC checkboxes, intensity, stagger, and Run Test (`--serve`)

### Changed

- OCC channels write 0 when intensity decays below the floor, so a snapshot does not keep a stale last pulse

## [0.2.0] - 2026-08-20

Default ALMA-style composition now includes mood dynamics and OCC emotion. Both stay optional: omit a provider and its channels stay absent.

### Added

- PAD mood dynamics: current mood decays toward the Gebhard-mapped baseline (`PadMood`; decay rate is project convention)
- OCC emotion: host-tagged OCC types with optional ALMA-style OCC→PAD overlay (`OccEmotion`, `OccToPadMapping`)
- Testing notes: `dotnet test`, `netstandard2.1` core, `net8.0` test host
- Design-facing applications page: how to include the engine in games (RTS, RPG, FPS, sims, NPCs), with fifty concrete uses

### Changed

- Public docs describe this library on its own. Do not name other private or internal projects.

## [0.1.0] - 2026-08-20

First public library cut. `PersonalityEngine.Core` targets `netstandard2.1`.

### Added

- Provider pipeline: events in; named-channel affect snapshot and optional action weights out
- OCEAN personality and Gebhard ALMA (2005) OCEAN→PAD mapping, with Gebhard’s numeric example as the first test
- Peterson meaning / metatraits module (`meaning` layer)
- Skinner operant learning module (`learning` layer)
- Piaget equilibration module (`cognition` layer); stages are host-set
- Erikson psychosocial identity module (`identity` layer); ages are host-set
- Charter, architecture, and citation registry

### Notes

- OCC emotion and PAD mood *dynamics* are chartered for the default composition; they are not required to use the providers above
- Numeric gains, 0..1 flags, and stage clocks are **project convention**, not psychometrics

[Unreleased]: https://github.com/RossSim/personality-engine/compare/v0.6.1...HEAD
[0.6.1]: https://github.com/RossSim/personality-engine/releases/tag/v0.6.1
[0.6.0]: https://github.com/RossSim/personality-engine/releases/tag/v0.6.0
[0.5.2]: https://github.com/RossSim/personality-engine/releases/tag/v0.5.2
[0.5.1]: https://github.com/RossSim/personality-engine/releases/tag/v0.5.1
[0.5.0]: https://github.com/RossSim/personality-engine/releases/tag/v0.5.0
[0.4.0]: https://github.com/RossSim/personality-engine/releases/tag/v0.4.0
[0.3.0]: https://github.com/RossSim/personality-engine/releases/tag/v0.3.0
[0.2.0]: https://github.com/RossSim/personality-engine/releases/tag/v0.2.0
[0.1.0]: https://github.com/RossSim/personality-engine/releases/tag/v0.1.0
