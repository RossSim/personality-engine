# Hosting Personality Engine

How a game (or other host) ticks, saves, and folds weights into an existing chooser. This is not a psychology paper. Numeric helpers here are **project convention** unless a citation says otherwise.

Rebuild the same composition before `Import` (same traits, same providers, same decay rates). Persist does not store constructor arguments.

## Idle tick

`AffectEngine.Tick(dt)` decays without a host event. It is the same as `Tick(WorldEvent.Tick, dt)`. Call it from the game loop when nothing happened this frame.

```csharp
engine.Tick(deltaTime);                 // idle
engine.Tick(ev, deltaTime);             // event + decay
```

## Persist

`Export` / `Import` round-trip **snapshot channels plus per-provider internal bags**. Snapshot floats alone are not enough:

- `PadMood` keeps internal P/A/D. Snapshot `mood.pad-mood.*` already includes the OCC overlay. Restoring those as internal mood would double-count overlay on the next tick.
- `OccEmotion` decays a private intensity map. Restoring channels without that map freezes decay.
- `OperantLearningProvider` keeps strengths, ratio counters, SD, and deprivation privately.

`AffectPersist` is a POCO (`Version`, `Channels`, `Providers`). The core does not take a JSON dependency. Hosts may serialize it with `System.Text.Json` or another serializer.

Unknown provider ids and unknown bag keys are **ignored** on load. Extra snapshot channel keys are **kept** (the snapshot is an open bag). Missing bags leave that provider at constructor defaults.

v1 shape:

```
Version: 1
Channels: { "layer.provider.channel": float, ... }
Providers: {
  "pad-mood": { "internal.pleasure", "internal.arousal", "internal.dominance", "seeded" },
  "occ": { "emotion.occ.joy": float, ... },
  "skinner-operant": { deprivation, optional sd, strength:* , ratio:*, next-vr:* }
}
```

Providers that are not stateful (for example OCEAN) are reconstructed from the host’s composition, then rewrite their channels on the next tick.

```csharp
var blob = engine.Export();
// host writes JSON
other.Import(blob);
other.Tick(0f); // or Tick(WorldEvent.Tick)
```
