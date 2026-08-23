# Dyad (relationship)

Optional **relationship** layer: pairwise liking toward a named other. OCC treats liking/disliking as an **attitude** that fortune-of-others assumes. This provider stores that attitude. It does not fire happy-for, pity, resentment, or gloating. The host still tags those eliciting conditions. The citation is OCC (1988) for liking as attitude; range, bumps, and decay are **project convention**.

Layer: `relationship`. Off the default ALMA composition: omit the provider and the channels stay absent.

| Provider | Layer | What it does | Source |
| --- | --- | --- | --- |
| `DyadProvider` (`dyad`) | relationship | `relationship.dyad.liking:{otherId}` in [-1, 1] | OCC 1988 (liking as attitude). Range, bumps, decay: **project convention** |
| `DyadWeighter` | action weights | Tints `approach:{other}` / `avoid:{other}` | Mix with fortune-of-others: **project convention** |

## What is in this slice

- Host events `dyad.like` / `dyad.dislike` (`HostEvents.Like` / `Dislike`) with `Target` = other id. Intensity adds to or subtracts from liking, clamped to [-1, 1].
- Independent others: liking for `ally` does not move liking for `rival`.
- Slow exponential decay toward 0 over `dt` (**project convention**; default 0.05 /s). Below a floor the channel writes 0 and drops.
- Persist via `IStatefulProvider`. Rebuild the same composition, then `Import`.
- `DyadComposition.Create` / `CreateWithAlma`, like Skinner, not wired into `AlmaComposition`.
- Fortune-of-others channels are **global**, so a happy-for pulse slightly tints every `approach:{id}`, not only the named Target. Pairwise bias still comes from liking.

## What stays out

- A full social network, Heider balance, or reputation sim
- Inferring fortune-of-others from the sign of liking
- Attraction OCC types (love, hate) as extra emotion channels
- Compound OCC (anger, gratitude)

Hosts that want a social pulse still call `HostEvents.HappyFor(other)` themselves after they have decided the other is liked.
