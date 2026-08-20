# Citations

Every provider in this library names a source. Project-only numbers (game feel, clamping, extra channels) are labeled **project convention** and must not be attributed to a paper.

This file is a registry, not a closed canon. Adding a provider means adding a row. Replacing a method means citing the new method; do not silently retitle ALMA, OCEAN, PAD, or OCC.

## Default composition (v0.1)

| Provider / mapping | What it covers | Source |
| --- | --- | --- |
| `OceanPersonality` | Stable Big Five traits (O, C, E, A, N) | McCrae, R. R., & Costa, P. T., Jr. Five-factor (OCEAN) model of personality. |
| `OceanToPadMapping` | Personality → PAD baseline | Gebhard, P. (2005). ALMA: A Layered Model of Affect. *AAMAS.* Uses Mehrabian’s PAD mapping coefficients. |
| `PadMood` | Pleasure–Arousal–Dominance mood space | Mehrabian, A. PAD (Pleasure–Arousal–Dominance) emotion/temperament space. |
| `OccEmotion` | Appraisal-based emotions | Ortony, A., Clore, G. L., & Collins, A. (1988). *The Cognitive Structure of Emotions.* |

Inspiration for a *modular affective agent*, not a source to fork: Dias, J., Mascarenhas, S., & Paiva, A. FAtiMA. Do not treat FAtiMA-Toolkit types or code as this engine.

## Peterson module ([PE-7](https://prayingforradar.atlassian.net/browse/PE-7))

Full review: [`peterson.md`](peterson.md).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `StabilityPlasticityProvider` | personality | DeYoung, C. G., Peterson, J. B., & Higgins, D. M. (2002). Higher-order factors of the Big Five predict conformity: Are there neuroses of health? *Personality and Individual Differences, 33*(4), 533–552. After Digman (1997). Equal-weight aggregation and 0..1 conformity map: **project convention**. |
| `OrderChaosMeaningProvider` | meaning | Peterson, J. B. (1999). *Maps of Meaning: The Architecture of Belief.* Peterson, J. B., & Flanders, J. L. (2002). Complexity Management Theory. *Cortex, 38*(3), 429–458. Peterson, J. B. (2013). Three forms of meaning. Numeric gains/decays: **project convention**. |
| `PetersonMeaningWeighter` | action weights | Same CMT sources; mix of explore/defend/integrate/withdraw is **project convention**. |

Not implemented (needs aspect-level BFAS, not domain OCEAN): Hirsh, J. B., DeYoung, C. G., Xu, X., & Peterson, J. B. (2010). Compassionate liberals and polite conservatives. *Personality and Social Psychology Bulletin, 36*(5), 655–664.

## First numeric check

Gebhard (2005) example, slightly relaxed in tests:

`O=0.4, C=0.8, E=0.6, A=0.3, N=0.4` → `P=0.38, A=-0.08, D=0.50`

Mapping used by that example (Mehrabian coefficients as used in ALMA):

- `P = 0.21E + 0.59A + 0.19N`
- `Ar = 0.15O + 0.30A − 0.57N`
- `D = 0.25O + 0.17C + 0.60E − 0.32A`

If a later provider uses different coefficients, cite that source on the provider. Do not overwrite this row.

## How to add a source

1. Implement an `IAffectProvider` (or projector) with its own id.
2. Add a row here: id, layer, what it covers, bibliographic citation.
3. Mark any extra knobs as project convention.
4. Tests for that provider live next to it; they must not require unrelated layers to be enabled.
