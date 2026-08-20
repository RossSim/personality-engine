# Skinner module

Supplemental providers for B. F. Skinner’s **experimental analysis of behavior** and radical-behaviorist philosophy. They do **not** live on the personality, mood, emotion, or meaning layers. Skinner treated “personality” as a **repertoire of operants** shaped by contingencies, not as inner traits that cause action (*Science and Human Behavior*, 1953; *Beyond Freedom and Dignity*, 1971).

Ticket: [PE-8](https://prayingforradar.atlassian.net/browse/PE-8). Layer: `learning`.

This module can run **alone** or **beside** OCEAN and Peterson. Combining them is a host choice. The theories disagree about causes (traits / meaning vs. history of reinforcement); the engine does not paper over that. Same honesty rule as GE3’s Maslow vs expected-utility tension.

## What is in the module

| Provider | Layer | What it does | Source |
| --- | --- | --- | --- |
| `OperantLearningProvider` (`skinner-operant`) | learning | Per-action operant strength under a three-term contingency; CRF / FR / VR | Skinner (1938, 1953); Ferster & Skinner (1957) |
| `OperantWeighter` | action weights | Weight ≈ strength × deprivation × SD control | Skinner (1953). Mix coefficients: **project convention** |

## Psychology (experimental analysis)

**Operant vs respondent.** *The Behavior of Organisms* (1938) distinguishes behavior **emitted** and selected by its consequences (operant) from elicited reflexes (respondent / Pavlovian). This engine models operants, not CS–US pairing.

**Three-term contingency** (*Science and Human Behavior*, 1953): discriminative stimulus (SD) → response → consequence.

| Consequence | Effect on future responding |
| --- | --- |
| Positive / negative **reinforcement** | Strengthens the operant |
| **Punishment** | Weakens the operant |
| **Extinction** (response, no reinforcer) | Weakens the operant |
| Neutral | No change |

Rate of responding is Skinner’s preferred measure of strength. The engine stores a 0..1 **strength** per action id as a game-ready proxy (**project convention**, not a published matching-law equation). Herrnstein’s matching law (1961) is a later quantitative extension and is **not** attributed to Skinner here.

**Schedules** (Ferster & Skinner, 1957):

- **CRF** (continuous) — every `skinner.emit` is reinforced
- **FR-n** — every nth emit is reinforced
- **VR** — emit reinforced after a varying count around a mean

FI / VI interval schedules are documented, not implemented in this slice.

**Establishing operations.** Deprivation and satiation change how effective a reinforcer is and how likely the related operant is (Skinner, 1953, deprivation/satiation). Channel: `learning.skinner-operant.deprivation`.

**Discriminative control.** An SD signals that a contingency is in effect. Channel: `learning.skinner-operant.sd`. If never set, the weighter does not suppress; if set low, weights are reduced by a **project-convention** multiplier.

## Philosophy

- **Radical behaviorism:** private events (thinking, feeling) are behavior to be explained by the same contingencies, not unmoved movers. They are not denied; they are not given causal privilege (Skinner, 1953, “Private events in a natural science”).
- **Autonomous man:** *Beyond Freedom and Dignity* (1971) argues that “freedom” and “dignity” as inner origination block a technology of behavior and sustain punishment. The *goals* those ideals served (reducing aversive control) should be pursued by changing environments. This is recorded as philosophy in citations. It is **not** encoded as an NPC political stance.
- **Walden Two** (1948) is fiction about cultural design via contingencies. Not a provider.
- **Verbal Behavior** (1957): mands, tacts, echoics, intraverbals. Documented; not implemented this slice.

## Relation to OCEAN and Peterson

| | Skinner | OCEAN / Peterson |
| --- | --- | --- |
| Cause of action | History of reinforcement in context | Traits, mood, meaning/logos |
| “Personality” | Repertoire | Slow Big Five / metatraits |
| Anomaly / unknown | Change in contingencies / extinction burst (not modeled as chaos) | Maps of Meaning chaos / CMT rigidity vs exploration |

Hosts may still run all three: Peterson can bias *which* operants are emitted; Skinner updates *how strong* they become after consequences. That wiring is **project design**, not something Skinner or Peterson published as a joint model.

## Events (host-tagged)

| Kind | `Target` | Effect |
| --- | --- | --- |
| `skinner.emit` | action id | Record a response; schedule may reinforce or extinguish |
| `skinner.reinforce` | action id | Strengthen (host-delivered reinforcer) |
| `skinner.punish` | action id | Weaken |
| `skinner.extinguish` | action id | Weaken without a response count |
| `skinner.sd` | unused | Intensity = presence of discriminative stimulus (0..1) |
| `skinner.eo` | unused | Intensity = deprivation / establishing operation (0..1) |

## What is out of scope

- Pavlov / Watson classical conditioning as if it were Skinner
- Herrnstein matching law as a Skinner formula
- Verbal Behavior classes (mands/tacts)
- Interval schedules (FI/VI)
- Encoding 1971 cultural-design politics as trait values
- Claiming 0..1 strengths are laboratory response rates

## Usage

```csharp
var engine = SkinnerComposition.Create(new[] { "forage", "idle" });
engine.Tick(WorldEvent.Tick);
engine.Tick(new WorldEvent(OperantLearningProvider.EmitKind, 1f, "forage"));
var weights = engine.WeightActions(new[] { "forage", "idle" });

// Beside Peterson + OCEAN:
var both = SkinnerComposition.CreateWithOceanAndPeterson(
    OceanTraits.GebhardExample,
    new[] { "forage", "peterson.explore" });
```
