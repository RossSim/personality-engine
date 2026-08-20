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

## Peterson module

Full review: [`peterson.md`](peterson.md).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `StabilityPlasticityProvider` | personality | DeYoung, C. G., Peterson, J. B., & Higgins, D. M. (2002). Higher-order factors of the Big Five predict conformity: Are there neuroses of health? *Personality and Individual Differences, 33*(4), 533–552. After Digman (1997). Equal-weight aggregation and 0..1 conformity map: **project convention**. |
| `OrderChaosMeaningProvider` | meaning | Peterson, J. B. (1999). *Maps of Meaning: The Architecture of Belief.* Peterson, J. B., & Flanders, J. L. (2002). Complexity Management Theory. *Cortex, 38*(3), 429–458. Peterson, J. B. (2013). Three forms of meaning. Numeric gains/decays: **project convention**. |
| `PetersonMeaningWeighter` | action weights | Same CMT sources; mix of explore/defend/integrate/withdraw is **project convention**. |

Not implemented (needs aspect-level BFAS, not domain OCEAN): Hirsh, J. B., DeYoung, C. G., Xu, X., & Peterson, J. B. (2010). Compassionate liberals and polite conservatives. *Personality and Social Psychology Bulletin, 36*(5), 655–664.

## Skinner module

Full review: [`skinner.md`](skinner.md). **New layer** `learning` (not personality/mood/emotion/meaning).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `OperantLearningProvider` | learning | Skinner, B. F. (1938). *The Behavior of Organisms.* Skinner, B. F. (1953). *Science and Human Behavior* (three-term contingency, reinforcement, punishment, extinction, deprivation). Ferster, C. B., & Skinner, B. F. (1957). *Schedules of Reinforcement* (CRF, FR, VR). 0..1 strengths and gains: **project convention**. |
| `OperantWeighter` | action weights | Skinner (1953). SD-absent multiplier and deprivation mix: **project convention**. |

Documented, not implemented: Skinner (1957) *Verbal Behavior*; Ferster & Skinner interval schedules; Skinner (1971) *Beyond Freedom and Dignity* as NPC values.

## Piaget module

Full review: [`piaget.md`](piaget.md). **New layer** `cognition` (not personality/mood/emotion/meaning/learning).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `PiagetEquilibrationProvider` | cognition | Piaget, J. (1952). *The Origins of Intelligence in Children* (orig. 1936). Piaget, J. (1954). *The Construction of Reality in the Child* (orig. 1937). Piaget, J. (1950). *The Psychology of Intelligence* (orig. 1947). Inhelder, B., & Piaget, J. (1958). *The Growth of Logical Thinking from Childhood to Adolescence.* Piaget, J. (1970). *Genetic Epistemology.* Piaget, J. (1985). *The Equilibration of Cognitive Structures* (orig. 1975). Numeric gains, host-set stages, and 0/1 flags: **project convention**. |
| `PiagetCognitionWeighter` | action weights | Piaget, J. (1951). *Play, Dreams and Imitation in Childhood* (orig. 1945). Play ≈ assimilation, imitation ≈ accommodation. Mix coefficients: **project convention**. |

Not implemented: automatic stage advancement; conservation-task scores; sensorimotor substages / A-not-B; neo-Piagetian or Vygotsky providers.

## Erikson module

Full review: [`erikson.md`](erikson.md). **New layer** `identity` (not personality/mood/emotion/meaning/learning/cognition).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `EriksonPsychosocialProvider` | identity | Erikson, E. H. (1963). *Childhood and Society* (2nd ed.; orig. 1950). Erikson, E. H. (1959). *Identity and the Life Cycle.* Erikson, E. H. (1968). *Identity: Youth and Crisis.* Erikson, E. H. (1982). *The Life Cycle Completed.* Numeric gains, host-set stages, and 0/1 flags: **project convention**. |
| `EriksonIdentityWeighter` | action weights | Erikson (1968). Explore ≈ moratorium; commit ≈ fidelity; care ≈ generativity. Mix coefficients: **project convention**. |

Not implemented: automatic stage advancement; Marcia identity statuses; Joan Erikson 9th stage; EPSI/MEIM scores; psychohistory as NPC biography.

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
