# Changelog

All notable changes to Personality Engine are documented here.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
Versions follow [SemVer](https://semver.org/). GitHub Releases use the same notes.

## [Unreleased]

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

[Unreleased]: https://github.com/RossSim/personality-engine/compare/v0.1.0...HEAD
[0.1.0]: https://github.com/RossSim/personality-engine/releases/tag/v0.1.0
