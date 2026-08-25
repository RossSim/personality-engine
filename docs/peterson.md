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

## References

- [DeYoung, Peterson & Higgins (2002)](https://doi.org/10.1016/S0191-8869(01)00171-4). Higher-order factors of the Big Five predict conformity. *Personality and Individual Differences, 33*(4), 533–552.
- [Digman (1997)](https://doi.org/10.1037/0022-3514.73.6.1246). Higher-order factors of the Big Five. *Journal of Personality and Social Psychology, 73*(6), 1246–1256.
- [DeYoung, Peterson & Higgins (2005)](https://doi.org/10.1111/j.0022-3506.2005.00318.x). Sources of openness/intellect. *Journal of Personality, 73*(4), 825–858. (Related; not implemented here.)
- [Peterson (1999)](https://www.routledge.com/Maps-of-Meaning-The-Architecture-of-Belief/Peterson/p/book/9780415922227). *Maps of Meaning: The Architecture of Belief.* Routledge.
- [Peterson & Flanders (2002)](https://doi.org/10.1016/S0010-9452(08)70680-4). Complexity Management Theory. *Cortex, 38*(3), 429–458.
- [Peterson (2013)](https://doi.org/10.1037/13944-005). Three forms of meaning and the management of complexity. In *The psychology of meaning*. APA.
- [Hirsh, DeYoung, Xu & Peterson (2010)](https://doi.org/10.1177/0146167210366852). Compassionate liberals and polite conservatives. *PSPB, 36*(5), 655–664. (Not implemented at domain OCEAN level.)

Full registry: [Citations](CITATIONS.md).
