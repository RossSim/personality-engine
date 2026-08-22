# Peterson module

This module implements the academic sources named below: DeYoung, Peterson & Higgins (2002), Digman (1997), Peterson (1999), Peterson & Flanders (2002), and Peterson (2013). Those papers and books are the citations.

## What is in the module

| Provider | Layer | What it does | Source |
| --- | --- | --- | --- |
| `StabilityPlasticityProvider` (`peterson-metatraits`) | personality | Stability and Plasticity metatraits; directional conformity tendency | DeYoung, Peterson & Higgins (2002), after Digman (1997) |
| `OrderChaosMeaningProvider` (`peterson-maps`) | meaning | Order (known), chaos (unknown), logos (knower), rigidity; three forms of meaning | Peterson (1999); Peterson & Flanders (2002); Peterson (2013) |
| `PetersonMeaningWeighter` | action weights | Explore / defend / integrate / withdraw from meaning state | CMT qualitative dynamics; numeric mix is project convention |

## Personality (peer-reviewed)

DeYoung, Peterson, and Higgins (2002) replicate Digman’s (1997) two higher-order Big Five factors and interpret them biologically:

- **Stability** — shared variance of Emotional Stability (1 − Neuroticism), Agreeableness, and Conscientiousness. Hypothesized serotonergic constraint. In the engine: equal-weight mean of those three (**project convention**; the paper uses factor scores).
- **Plasticity** — shared variance of Extraversion and Openness. Hypothesized dopaminergic exploration. In the engine: equal-weight mean of E and O (**project convention**).
- **Conformity** — they report Stability positively, and Plasticity negatively, related to socially desirable / moralistic responding. The engine maps `0.5 + 0.5 × (Stability − Plasticity)` onto 0..1. That mapping is a **project convention**. It is not their SEM betas (university Stability β = 0.98, Plasticity β = −0.48).

The 2002 paper also ties these metatraits to Peterson’s (1999) claim that maintaining order and adapting to novelty are the basic adaptive problems — the same pair that Maps of Meaning calls order and chaos.

Related, not implemented here:

- DeYoung, Peterson & Higgins (2005), *Journal of Personality* — cognitive/neuropsychological correlates of Openness/Intellect.
- Hirsh, DeYoung, Xu & Peterson (2010), *PSPB* — aspect-level Agreeableness and Conscientiousness. This module uses domain-level OCEAN, so that paper is not mapped here.

## Philosophy and meaning (*Maps of Meaning*, CMT)

Peterson (1999) argues that humans inhabit a world parsed for **action**, not a world of given objects. Experience has three constituent elements:

1. **The known / order** — explored, mapped, habitable territory; culture; motivation–action–perception (MAP) schemas.
2. **The unknown / chaos** — anomaly, the unexplored, that which does not fit the current map.
3. **The knower / logos** — consciousness that mediates; the “hero” who voluntarily updates the map.

Belief systems are abstract territory. They regulate emotion. When they fail, emotion dysregulates.

Peterson & Flanders (2002), Complexity Management Theory (*Cortex*): the world is too complex to represent fully, so we use simplifying conceptual structures. When those fail, two paths:

- **Rigidity** — protect the old structure (ideological dogmatism).
- **Voluntary reconstrual** — face complexity, gather information, recast the habitable world.

Peterson (2013) distinguishes three forms of meaning: meaning of the known, meaning of chaos/anomaly, and meaning that arises in voluntary exploration. Those are the `meaning-known`, `meaning-chaos`, and `meaning-exploration` channels.

## Events (host-tagged)

| Kind | Effect (qualitative; gains are project convention) |
| --- | --- |
| `peterson.anomaly` | Chaos up, order down; rigidity biased by Stability; logos biased by Plasticity |
| `peterson.voluntary-explore` | Logos up, rigidity down |
| `peterson.integrate` | Order up, chaos down (map recast) |
| `peterson.defend-belief` | Rigidity up; chaos suppressed rather than integrated |
| `peterson.confirm-map` | Slight order confirmation |

## What is out of scope

- Treating the meaning-layer numbers as a published psychometric scale. The state variables follow the qualitative dynamics in the cited sources; the numeric gains are **project convention**.
- Hirsh et al. (2010) from five domain scores (that paper uses aspect-level scores)

## Usage

```csharp
var engine = PetersonComposition.Create(new OceanTraits(0.4f, 0.8f, 0.6f, 0.3f, 0.4f));
engine.Tick(WorldEvent.Tick);
engine.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, intensity: 0.8f));
var weights = engine.WeightActions(new[] { "peterson.explore", "peterson.defend" });
```
