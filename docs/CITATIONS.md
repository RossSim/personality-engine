# Citations

Every provider in this library names a source. Named people (Peterson, Skinner, Piaget, Erikson, and the OCC / OCEAN / PAD authors) are cited for the academic papers and books in their tables. Project-only numbers (game feel, clamping, extra channels) are labeled **project convention** and must not be attributed to a paper.

This file is a registry, not a closed canon. Adding a provider means adding a row. Replacing a method means citing the new method; do not silently retitle ALMA, OCEAN, PAD, or OCC. What OCEAN, PAD, and OCC mean without the papers: [Personality, mood, and feeling](OCEAN_PAD_OCC.md).

## Default composition

| Provider / mapping | What it covers | Source |
| --- | --- | --- |
| `OceanPersonality` | Stable Big Five traits (O, C, E, A, N) as host-supplied 0..1 floats | [McCrae & Costa (2008)](https://www.scholars.northwestern.edu/en/publications/the-five-factor-theory-of-personality). *Handbook of personality: Theory and research* (3rd ed., pp. 159–181). Guilford Press. **Not** NEO-PI-R / NEO-FFI items. |
| `OceanToPadMapping` | Personality → PAD baseline | [Gebhard (2005)](https://doi.org/10.1145/1082473.1082478). ALMA: A layered model of affect. *Proceedings of AAMAS '05* (pp. 29–36). ACM. Uses Mehrabian PAD mapping coefficients as reported there. |
| `PadMood` | Current PAD mood; exponential pull toward the mapped baseline over `dt` | [Mehrabian & Russell (1974)](https://mitpress.mit.edu/9780262131269/an-approach-to-environmental-psychology/). *An Approach to Environmental Psychology.* MIT Press. [Mehrabian (1996)](https://doi.org/10.1007/BF02686918). Pleasure-arousal-dominance. *Current Psychology, 14*(4), 261–292. [Gebhard (2005)](https://doi.org/10.1145/1082473.1082478) for the wiring (mood toward personality baseline). Decay rate and `pad.push` deltas: **project convention**. |
| `OccEmotion` | OCC types as named emotion channels (well-being, prospect, attribution, fortune-of-others, well-being+attribution compounds) | [Ortony, Clore & Collins (1988)](https://doi.org/10.1017/CBO9780511525661). *The Cognitive Structure of Emotions.* Cambridge University Press. Host-tagged eliciting events and decay: **project convention**. Full review: [`occ.md`](occ.md). |
| `OccToPadMapping` | OCC intensities → PAD overlay | [Gebhard (2005)](https://doi.org/10.1145/1082473.1082478). ALMA. First wiring of emotion into mood; numeric overlay coefficients: **project convention**. |

Inspiration for a *modular affective agent*, not a source to fork:

- [Dias & Paiva (2005)](https://doi.org/10.1007/11595014_13). Feeling and reasoning: A computational model for emotional characters. *Progress in Artificial Intelligence* (EPIA 2005), LNCS 3808, pp. 127–140. Springer.
- [Dias, Mascarenhas & Paiva (2014)](https://doi.org/10.1007/978-3-319-12910-1_1). FAtiMA Modular: Towards an agent architecture with a generic appraisal framework. In *Emotion modeling* (LNCS 8750). Springer.
- [Mascarenhas et al. (2022)](https://doi.org/10.1145/3510822). FAtiMA Toolkit: Toward an accessible tool for the development of socio-emotional agents. *ACM Transactions on Interactive Intelligent Systems, 12*(1), Article 8.

Do not treat FAtiMA-Toolkit types or code as this engine. This repository does not include FAtiMA source.

## Peterson module

Full review: [`peterson.md`](peterson.md).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `StabilityPlasticityProvider` | personality | [DeYoung, Peterson & Higgins (2002)](https://doi.org/10.1016/S0191-8869(01)00171-4). Higher-order factors of the Big Five predict conformity. *Personality and Individual Differences, 33*(4), 533–552. After [Digman (1997)](https://doi.org/10.1037/0022-3514.73.6.1246). Equal-weight aggregation and 0..1 conformity map: **project convention**. |
| `OrderChaosMeaningProvider` | meaning | [Peterson (1999)](https://www.routledge.com/Maps-of-Meaning-The-Architecture-of-Belief/Peterson/p/book/9780415922227). *Maps of Meaning: The Architecture of Belief.* Routledge. [Peterson & Flanders (2002)](https://doi.org/10.1016/S0010-9452(08)70680-4). Complexity Management Theory. *Cortex, 38*(3), 429–458. [Peterson (2013)](https://doi.org/10.1037/13944-005). Three forms of meaning. In *The psychology of meaning*. APA. Numeric gains/decays: **project convention**. |
| `PetersonMeaningWeighter` | action weights | Same CMT sources; mix of explore/defend/integrate/withdraw is **project convention**. |

Not implemented (needs aspect-level BFAS, not domain OCEAN): [Hirsh, DeYoung, Xu & Peterson (2010)](https://doi.org/10.1177/0146167210366852). Compassionate liberals and polite conservatives. *Personality and Social Psychology Bulletin, 36*(5), 655–664.

## Skinner module

Full review: [`skinner.md`](skinner.md). **New layer** `learning` (not personality/mood/emotion/meaning).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `OperantLearningProvider` | learning | [Skinner (1938)](https://www.bfskinner.org/product/the-behavior-of-organisms/). *The Behavior of Organisms.* [Skinner (1953)](https://www.bfskinner.org/product/science-and-human-behavior/). *Science and Human Behavior* (three-term contingency, reinforcement, punishment, extinction, deprivation). [Ferster & Skinner (1957)](https://www.bfskinner.org/product/schedules-of-reinforcement/). *Schedules of Reinforcement* (CRF, FR, VR). 0..1 strengths and gains: **project convention**. |
| `OperantWeighter` | action weights | [Skinner (1953)](https://www.bfskinner.org/product/science-and-human-behavior/). SD-absent multiplier and deprivation mix: **project convention**. |

Documented, not implemented: [Skinner (1957)](https://www.bfskinner.org/product/verbal-behavior-2/) *Verbal Behavior*; Ferster & Skinner interval schedules. [Skinner (1971)](https://www.bfskinner.org/product/beyond-freedom-and-dignity/) *Beyond Freedom and Dignity* is cited as philosophy; not a separate provider.

## Piaget module

Full review: [`piaget.md`](piaget.md). **New layer** `cognition` (not personality/mood/emotion/meaning/learning).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `PiagetEquilibrationProvider` | cognition | [Piaget (1952)](https://openlibrary.org/works/OL458047W). *The Origins of Intelligence in Children* (orig. 1936). [Piaget (1954)](https://openlibrary.org/works/OL458050W). *The Construction of Reality in the Child* (orig. 1937). [Piaget (1950)](https://openlibrary.org/works/OL458043W). *The Psychology of Intelligence* (orig. 1947). [Inhelder & Piaget (1958)](https://openlibrary.org/works/OL458031W). *The Growth of Logical Thinking from Childhood to Adolescence.* [Piaget (1970)](https://archive.org/details/geneticepistemol0000piag). *Genetic Epistemology.* [Piaget (1985)](https://press.uchicago.edu/ucp/books/book/chicago/E/bo3628970.html). *The Equilibration of Cognitive Structures* (orig. 1975). Numeric gains, host-set stages, and 0/1 flags: **project convention**. |
| `PiagetCognitionWeighter` | action weights | [Piaget (1951)](https://openlibrary.org/works/OL458044W). *Play, Dreams and Imitation in Childhood* (orig. 1945). Play ≈ assimilation, imitation ≈ accommodation. Mix coefficients: **project convention**. |

Not implemented: automatic stage advancement; conservation-task scores; sensorimotor substages / A-not-B; neo-Piagetian or Vygotsky providers.

## Erikson module

Full review: [`erikson.md`](erikson.md). **New layer** `identity` (not personality/mood/emotion/meaning/learning/cognition).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `EriksonPsychosocialProvider` | identity | [Erikson (1963)](https://wwnorton.com/books/9780393310214). *Childhood and Society* (2nd ed.; orig. 1950). [Erikson (1959)](https://archive.org/details/identitylifecycl0000erik). *Identity and the Life Cycle.* [Erikson (1968)](https://wwnorton.com/books/Identity-Youth-and-Crisis/). *Identity: Youth and Crisis.* [Erikson (1982)](https://wwnorton.com/books/The-Life-Cycle-Completed/). *The Life Cycle Completed.* Numeric gains, host-set stages, and 0/1 flags: **project convention**. |
| `EriksonIdentityWeighter` | action weights | [Erikson (1968)](https://wwnorton.com/books/Identity-Youth-and-Crisis/). Explore ≈ moratorium; commit ≈ fidelity; care ≈ generativity. Mix coefficients: **project convention**. |

Not implemented: automatic stage advancement; [Marcia (1966)](https://doi.org/10.1037/h0023281) identity statuses; [Erikson & Erikson (1997)](https://wwnorton.com/books/The-Life-Cycle-Completed-Extended-Version/) 9th stage; EPSI/MEIM scores; psychohistory as NPC biography ([Erikson, 1958](https://wwnorton.com/books/Young-Man-Luther/), *Young Man Luther*).

## Dyad module

Full review: [`dyad.md`](dyad.md). **New layer** `relationship` (not personality/mood/emotion/meaning/learning/cognition/identity).

| Provider / mapping | Layer | Source |
| --- | --- | --- |
| `DyadProvider` | relationship | [Ortony, Clore & Collins (1988)](https://doi.org/10.1017/CBO9780511525661). Liking/disliking as an attitude that fortune-of-others assumes. Pairwise channel, bump size, and decay: **project convention**. |
| `DyadWeighter` | action weights | Same OCC attitude plus fortune-of-others intensities. Mix onto `approach:{other}` / `avoid:{other}`: **project convention**. |

Not implemented: [Heider (1958)](https://archive.org/details/psychologyofinte00heid). *The Psychology of Interpersonal Relations.* Wiley (balance/reputation). Attraction OCC types as extra emotion channels. Inferring OCC fortune-of-others from the sign of liking.

## Named in the charter, not implemented

These appear as *allowed future providers* or *out of scope*. They are cited so the names are not orphaned:

| Name | Why it is mentioned | Source (not in this library) |
| --- | --- | --- |
| HEXACO | Alternate personality provider (intended; see [`ROADMAP.md`](ROADMAP.md)) | [Ashton & Lee (2007)](https://doi.org/10.1111/j.1467-6494.2007.00447.x). Empirical, theoretical, and practical advantages of the HEXACO model. *Personality and Social Psychology Review, 11*(2), 150–166. |
| Dark Triad | Example supplement on the personality layer (intended; see [`ROADMAP.md`](ROADMAP.md)) | [Paulhus & Williams (2002)](https://doi.org/10.1016/S0092-6566(02)00505-6). The dark triad of personality. *Journal of Research in Personality, 36*(6), 556–563. |
| Maslow | Named so the charter word is not orphaned; **not planned** as a provider (see [`ROADMAP.md`](ROADMAP.md)) | [Maslow (1943)](https://doi.org/10.1037/h0054346). A theory of human motivation. *Psychological Review, 50*(4), 370–396. [Maslow (1954)](https://openlibrary.org/works/OL458018W). *Motivation and Personality.* Harper. |
| Marcia identity statuses | Documented next to Erikson; not encoded | [Marcia (1966)](https://doi.org/10.1037/h0023281). Development and validation of ego-identity status. *Journal of Personality and Social Psychology, 3*(5), 551–558. |
| Joan Erikson 9th stage | Documented next to Erikson; not encoded | [Erikson & Erikson (1997)](https://wwnorton.com/books/The-Life-Cycle-Completed-Extended-Version/). *The Life Cycle Completed* (extended version). W. W. Norton. |

## First numeric check

[Gebhard (2005)](https://doi.org/10.1145/1082473.1082478) example, slightly relaxed in tests:

`O=0.4, C=0.8, E=0.6, A=0.3, N=0.4` → `P=0.38, A=-0.08, D=0.50`

Mapping used by that example (Mehrabian coefficients as used in ALMA):

- `P = 0.21E + 0.59A + 0.19N`
- `Ar = 0.15O + 0.30A − 0.57N`
- `D = 0.25O + 0.17C + 0.60E − 0.32A`

If a later provider uses different coefficients, cite that source on the provider. Do not overwrite this row.

References named for each person are the academic sources in the tables. Legal notice: [`DISCLAIMER.md`](../DISCLAIMER.md) (attribution is not a copyright license).

## How to add a source

1. Implement an `IAffectProvider` (or projector) with its own id.
2. Add a row here: id, layer, what it covers, bibliographic citation with a link to the original paper or book.
3. Mark any extra knobs as project convention.
4. Tests for that provider live next to it; they must not require unrelated layers to be enabled.
