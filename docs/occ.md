# OCC emotion

First emotion-layer provider: Ortony, Clore & Collins (1988) **OCC** types as named channels. Layer: `emotion`.

This slice is **host-tagged eliciting conditions**, not a full OCC goal / standard / attitude network. The host decides that an event is desirable, prospect-based, or an attribution; the provider stores intensities and decays them. A later provider may infer those conditions from richer snapshot state without a charter change.

## What is in the module

| Provider | Layer | What it does | Source |
| --- | --- | --- | --- |
| `OccEmotion` (`occ`) | emotion | 16 OCC types as `emotion.occ.*` channels | Ortony, Clore & Collins (1988) |
| `OccToPadMapping` (`occ-to-pad`) | mood | Optional ALMA-style overlay: OCC intensities → PAD | Gebhard (2005) for the *wiring*; coefficients are **project convention** |

`OccEmotion` does **not** require PadMood. Omit either provider and its channels stay absent.

## Psychology (this slice)

OCC groups emotions by eliciting conditions. Implemented here:

| Group | Types | Host event |
| --- | --- | --- |
| Well-being | joy, distress | `occ.joy`, `occ.distress` |
| Prospect-based | hope, fear, satisfaction, fears-confirmed, relief, disappointment | `occ.hope`, `occ.fear`, `occ.satisfaction`, `occ.fears-confirmed`, `occ.relief`, `occ.disappointment` |
| Attribution | pride, shame, admiration, reproach | `occ.pride`, `occ.shame`, `occ.admiration`, `occ.reproach` |
| Fortune-of-others | happy-for, pity (sorry-for), resentment, gloating | `occ.happy-for`, `occ.pity`, `occ.resentment`, `occ.gloating` |

The host still tags the eliciting condition. `WorldEvent.Target` may name the other person; snapshot channels stay **global** (`emotion.occ.happy-for`), not per-other keys. Pairwise liking belongs on the optional [`dyad.md`](dyad.md) relationship provider, not in this emotion slice.

Not in this slice: attraction (love, hate), compound well-being/attribution (gratification, remorse, gratitude, anger), inferring fortune-of-others from untyped events or from liking, and automatic appraisal from untyped `WorldEvent`s.

Intensities are 0..1 eliciting potentials (**project convention**). Exponential decay toward 0 is **project convention** (OCC treats emotions as momentary; it does not publish this engine’s time constant).

## ALMA wiring

Gebhard ALMA (2005) is the **first** composition that lets OCC influence PAD mood. `OccToPadMapping` writes `mood.occ-to-pad.*`. When `PadMood` is also enabled, it adds that overlay to current mood without accumulating it into the baseline. Hosts may omit the mapping and keep emotion and mood separate.
