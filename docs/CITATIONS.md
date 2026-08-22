# Citations

Every provider in this library names a source. Named people (Peterson, Skinner, Piaget, Erikson, and the OCC / OCEAN / PAD authors) are cited for the academic papers and books in their tables. Project-only numbers (game feel, clamping, extra channels) are labeled **project convention** and must not be attributed to a paper.

This file is a registry, not a closed canon. Adding a provider means adding a row. Replacing a method means citing the new method; do not silently retitle ALMA, OCEAN, PAD, or OCC.

## Default composition

| Provider / mapping | What it covers | Source |
| --- | --- | --- |
| `OceanPersonality` | Stable Big Five traits (O, C, E, A, N) as host-supplied 0..1 floats | McCrae, R. R., & Costa, P. T., Jr. (2008). The five-factor theory of personality. In O. P. John, R. W. Robins, & L. A. Pervin (Eds.), *Handbook of personality: Theory and research* (3rd ed., pp. 159–181). Guilford Press. **Not** NEO-PI-R / NEO-FFI items. |
| `OceanToPadMapping` | Personality → PAD baseline | Gebhard, P. (2005). ALMA: A layered model of affect. In *Proceedings of AAMAS '05* (pp. 29–36). ACM. https://doi.org/10.1145/1082473.1082478 Uses Mehrabian PAD mapping coefficients as reported there. |
| `PadMood` | Current PAD mood; exponential pull toward the mapped baseline over `dt` | Mehrabian, A., & Russell, J. A. (1974). *An Approach to Environmental Psychology.* MIT Press. Mehrabian, A. (1996). Pleasure-arousal-dominance: A general framework for describing and measuring individual differences in temperament. *Current Psychology, 14*(4), 261–292. Gebhard (2005) for the wiring (mood toward personality baseline). Decay rate and `pad.push` deltas: **project convention**. |
| `OccEmotion` | OCC types as named emotion channels (well-being, prospect, attribution, fortune-of-others, well-being+attribution compounds) | Ortony, A., Clore, G. L., & Collins, A. (1988). *The Cognitive Structure of Emotions.* Cambridge University Press. Host-tagged eliciting events and decay: **project convention**. Full review: [`occ.md`](occ.md). |
| `OccToPadMapping` | OCC intensities → PAD overlay | Gebhard, P. (2005). ALMA. First wiring of emotion into mood; numeric overlay coefficients: **project convention**. |

Inspiration for a *modular affective agent*, not a source to fork:

- Dias, J., & Paiva, A. (2005). Feeling and reasoning: A computational model for emotional characters. In C. Bento, A. Cardoso, & G. Dias (Eds.), *Progress in Artificial Intelligence* (EPIA 2005), LNCS 3808, pp. 127–140. Springer. https://doi.org/10.1007/11595014_13
- Dias, J., Mascarenhas, S., & Paiva, A. (2014). FAtiMA Modular: Towards an agent architecture with a generic appraisal framework. In T. Bosse, J. Broekens, J. Dias, & J. van der Zwaan (Eds.), *Emotion modeling* (LNCS 8750). Springer.
- Mascarenhas, S., Guimarães, M., Prada, R., Santos, P. A., Dias, J., & Paiva, A. (2022). FAtiMA Toolkit: Toward an accessible tool for the development of socio-emotional agents. *ACM Transactions on Interactive Intelligent Systems, 12*(1), Article 8. https://doi.org/10.1145/3510822

Do not treat FAtiMA-Toolkit types or code as this engine. This repository does not include FAtiMA source.

## Peterson module

Full review: [`peterson.md`](peterson.md).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `StabilityPlasticityProvider` | personality | DeYoung, C. G., Peterson, J. B., & Higgins, D. M. (2002). Higher-order factors of the Big Five predict conformity: Are there neuroses of health? *Personality and Individual Differences, 33*(4), 533–552. https://doi.org/10.1016/S0191-8869(01)00171-4 After Digman, J. M. (1997). Higher-order factors of the Big Five. *Journal of Personality and Social Psychology, 73*(6), 1246–1256. https://doi.org/10.1037/0022-3514.73.6.1246 Equal-weight aggregation and 0..1 conformity map: **project convention**. |
| `OrderChaosMeaningProvider` | meaning | Peterson, J. B. (1999). *Maps of Meaning: The Architecture of Belief.* Routledge. Peterson, J. B., & Flanders, J. L. (2002). Complexity Management Theory: Motivation for ideological rigidity and social conflict. *Cortex, 38*(3), 429–458. https://doi.org/10.1016/S0010-9452(08)70680-4 Peterson, J. B. (2013). Three forms of meaning and the management of complexity. In K. Markman, T. Proulx, & M. Lindberg (Eds.), *The psychology of meaning*. American Psychological Association. Numeric gains/decays: **project convention**. |
| `PetersonMeaningWeighter` | action weights | Same CMT sources; mix of explore/defend/integrate/withdraw is **project convention**. |

Not implemented (needs aspect-level BFAS, not domain OCEAN): Hirsh, J. B., DeYoung, C. G., Xu, X., & Peterson, J. B. (2010). Compassionate liberals and polite conservatives: Associations of agreeableness with political ideology and moral values. *Personality and Social Psychology Bulletin, 36*(5), 655–664.

## Skinner module

Full review: [`skinner.md`](skinner.md). **New layer** `learning` (not personality/mood/emotion/meaning).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `OperantLearningProvider` | learning | Skinner, B. F. (1938). *The Behavior of Organisms.* Skinner, B. F. (1953). *Science and Human Behavior* (three-term contingency, reinforcement, punishment, extinction, deprivation). Ferster, C. B., & Skinner, B. F. (1957). *Schedules of Reinforcement* (CRF, FR, VR). 0..1 strengths and gains: **project convention**. |
| `OperantWeighter` | action weights | Skinner (1953). SD-absent multiplier and deprivation mix: **project convention**. |

Documented, not implemented: Skinner (1957) *Verbal Behavior*; Ferster & Skinner interval schedules. Skinner (1971) *Beyond Freedom and Dignity* is cited as philosophy; not a separate provider.

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

Not implemented: automatic stage advancement; Marcia identity statuses (Marcia, 1966); Joan Erikson 9th stage (Erikson & Erikson, 1997); EPSI/MEIM scores; psychohistory as NPC biography (`Young Man Luther`, 1958).

## Dyad module

Full review: [`dyad.md`](dyad.md). **New layer** `relationship` (not personality/mood/emotion/meaning/learning/cognition/identity).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `DyadProvider` | relationship | Ortony, A., Clore, G. L., & Collins, A. (1988). *The Cognitive Structure of Emotions.* Liking/disliking as an attitude that fortune-of-others assumes. Pairwise channel, bump size, and decay: **project convention**. |
| `DyadWeighter` | action weights | Same OCC attitude plus fortune-of-others intensities. Mix onto `approach:{other}` / `avoid:{other}`: **project convention**. |

Not implemented: Heider, F. (1958). *The Psychology of Interpersonal Relations.* Wiley (balance/reputation). Attraction OCC types as extra emotion channels. Inferring OCC fortune-of-others from the sign of liking.

## Named in the charter, not implemented

These appear as *allowed future providers* or *out of scope*. They are cited so the names are not orphaned:

| Name | Why it is mentioned | Source (not in this library) |
| --- | --- | --- |
| HEXACO | Alternate personality provider | Ashton, M. C., & Lee, K. (2007). Empirical, theoretical, and practical advantages of the HEXACO model of personality structure. *Personality and Social Psychology Review, 11*(2), 150–166. |
| Dark Triad | Example supplement on the personality layer | Paulhus, D. L., & Williams, K. M. (2002). The dark triad of personality: Narcissism, Machiavellianism, and psychopathy. *Journal of Research in Personality, 36*(6), 556–563. |
| Maslow | Motives / needs stack (out of current milestones) | Maslow, A. H. (1943). A theory of human motivation. *Psychological Review, 50*(4), 370–396. Maslow, A. H. (1954). *Motivation and Personality.* Harper. |
| Marcia identity statuses | Documented next to Erikson; not encoded | Marcia, J. E. (1966). Development and validation of ego-identity status. *Journal of Personality and Social Psychology, 3*(5), 551–558. |
| Joan Erikson 9th stage | Documented next to Erikson; not encoded | Erikson, E. H., & Erikson, J. M. (1997). *The Life Cycle Completed* (extended version). W. W. Norton. |

## First numeric check

Gebhard (2005) example, slightly relaxed in tests:

`O=0.4, C=0.8, E=0.6, A=0.3, N=0.4` → `P=0.38, A=-0.08, D=0.50`

Mapping used by that example (Mehrabian coefficients as used in ALMA):

- `P = 0.21E + 0.59A + 0.19N`
- `Ar = 0.15O + 0.30A − 0.57N`
- `D = 0.25O + 0.17C + 0.60E − 0.32A`

If a later provider uses different coefficients, cite that source on the provider. Do not overwrite this row.

References named for each person are the academic sources in the tables. Legal notice: [`DISCLAIMER.md`](../DISCLAIMER.md) (attribution is not a copyright license).

## How to add a source

1. Implement an `IAffectProvider` (or projector) with its own id.
2. Add a row here: id, layer, what it covers, bibliographic citation.
3. Mark any extra knobs as project convention.
4. Tests for that provider live next to it; they must not require unrelated layers to be enabled.
