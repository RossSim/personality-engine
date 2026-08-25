# Citations

Every catalog knob names a source. This file is the **shared registry**. Per-preset citation tables stay in the markdown rows under [`presets/`](../presets/README.md). Exact floats, action ids, and which providers a seed enables are **project convention** unless a row says otherwise.

Personality Engine’s provider bibliography: [personality-engine citations](https://github.com/RossSim/personality-engine/blob/main/docs/CITATIONS.md).

Cited authors have not endorsed this catalog. See [Disclaimer](../DISCLAIMER.md).

## Shared knobs

| Knob | What it is | Source |
| --- | --- | --- |
| Trait scale (OCEAN 0..1) | Host-supplied Big Five domains, not inventory items | [McCrae & Costa (2008)](https://www.scholars.northwestern.edu/en/publications/the-five-factor-theory-of-personality). *Handbook of personality* (3rd ed., pp. 159–181). Guilford Press. **Not** NEO-PI-R / NEO-FFI items. |
| Trait bands (low / mid / high) | Inclusive ranges so jitter stays inside an authored band | **Project convention.** low 0.20–0.40, mid 0.40–0.60, high 0.65–0.85. |
| Occupational **direction** (which trait is high vs low for a job) | Conscientiousness across jobs; Extraversion more for social work; Openness more for training proficiency | [Barrick & Mount (1991)](https://doi.org/10.1111/j.1744-6570.1991.tb00688.x). *Personnel Psychology, 44*(1), 1–26. [Hurtz & Donovan (2000)](https://doi.org/10.1037/0021-9010.85.6.869). *Journal of Applied Psychology, 85*(6), 869–879. Exact midpoints: **project convention.** |
| `operantSeeds` | Action-id → 0..1 training history for `skinner-operant` | [Skinner (1953)](https://www.bfskinner.org/product/science-and-human-behavior/). *Science and Human Behavior.* [Ferster & Skinner (1957)](https://www.bfskinner.org/product/schedules-of-reinforcement/). *Schedules of Reinforcement.* Strengths and action ids: **project convention.** |
| ALMA stack (`ocean`, `ocean-to-pad`, `pad-mood`, `occ`, `occ-to-pad`) | Personality, PAD mood baseline, OCC feelings | [Gebhard (2005)](https://doi.org/10.1145/1082473.1082478). ALMA. [McCrae & Costa (2008)](https://www.scholars.northwestern.edu/en/publications/the-five-factor-theory-of-personality); [Mehrabian & Russell (1974)](https://mitpress.mit.edu/9780262131269/an-approach-to-environmental-psychology/); [Ortony, Clore & Collins (1988)](https://doi.org/10.1017/CBO9780511525661). Enabling the stack on a seed: **project convention.** |
| PAD baseline | Personality → mood; not a separate temperament constructor | [Mehrabian & Russell (1974)](https://mitpress.mit.edu/9780262131269/an-approach-to-environmental-psychology/). [Mehrabian (1996)](https://doi.org/10.1007/BF02686918). *Current Psychology, 14*(4), 261–292. Coefficients live in PE `ocean-to-pad` ([Gebhard 2005](https://doi.org/10.1145/1082473.1082478)). |
| Named / ambient / crowd jitter | Trait delta and which layers to keep | **Project convention.** Named ±0.05, ambient/crowd ±0.12, inside the band. Personality Engine has no jitter API. |
| Omitting `cognitiveStage` / `identityStage` on adult jobs and temperament | Climate and craft are not developmental ranks | **Project convention.** |

## Clan knobs

| Knob | What it is | Source |
| --- | --- | --- |
| `cognitiveStage` | PE Piaget period; `hypothetical` on at `FormalOperational` | [Piaget (1950)](https://openlibrary.org/works/OL458043W). *The Psychology of Intelligence.* [Inhelder & Piaget (1958)](https://openlibrary.org/works/OL458031W). *The Growth of Logical Thinking.* [Piaget (1985)](https://press.uchicago.edu/ucp/books/book/chicago/E/bo3628970.html). *The Equilibration of Cognitive Structures.* Host-set stage; choosing a stage for a **fantasy clan**: **project convention.** Structure, not IQ. |
| `peterson-metatraits` / `peterson-maps` | Stability/plasticity and explore vs order | [DeYoung, Peterson & Higgins (2002)](https://doi.org/10.1016/S0191-8869(01)00171-4). [Peterson (1999)](https://www.routledge.com/Maps-of-Meaning-The-Architecture-of-Belief/Peterson/p/book/9780415922227). *Maps of Meaning.* Enabling them on a seed: **project convention.** |

## Temperament knobs

| Knob | What it is | Source |
| --- | --- | --- |
| Easy / difficult / slow-to-warm-up | NYLS constellations mapped onto OCEAN bands | [Thomas, Chess & Birch (1968)](https://nyupress.org/9780814788407/temperament-and-behavior-disorders-in-children/). *Temperament and Behavior Disorders in Children.* [Thomas & Chess (1977)](https://archive.org/details/temperamentdevel0000thom). *Temperament and Development.* [Thomas & Chess (1981)](https://archive.org/details/dynamicsofpsycho0000thom). *The Dynamics of Psychological Development.* Mapping onto Extraversion, Agreeableness, Neuroticism, and Conscientiousness: **project convention.** Not MBTI, not NYLS scoring keys, not an infant diagnosis. |
| Unmapped NYLS dimensions (intensity, threshold, distractibility) | Not PE knobs | Host OCC magnitudes and pacing. **Project convention** to omit them. |

## Not in this catalog

- IQ, g, or WAIS-style composites
- MBTI or four-letter types
- Real-world race, ethnicity, or national rank tables
- Holland RIASEC, Sternberg, Bandura, HEXACO (wait for those providers in personality-engine)
- A temperament `IAffectProvider` (PAD comes from `ocean-to-pad`)
