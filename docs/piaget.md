# Jean Piaget — academic review and in-module mapping

This note reviews Piaget's published research for Personality Engine and records **what is in the `cognition` layer**, what is a **project convention**, and what stays **out of scope**. Citations are Piaget and Inhelder as named below.

Layer: `cognition`.

Piaget is **not** a personality theory. His project is genetic epistemology: how knowledge structures arise through action on the world. He belongs beside Peterson (affective meaning) and Skinner (operant history), not inside them. Hosts that want developmental constraints compose `PiagetEquilibrationProvider` on the **`cognition`** layer; hosts that do not simply omit it.

| Provider | Layer | What it does | Source |
| --- | --- | --- | --- |
| `PiagetEquilibrationProvider` (`piaget-equilibration`) | cognition | Assimilation / accommodation / equilibrium; host-set stage; stage-gated flags | Piaget (1950, 1952, 1954, 1985); Inhelder & Piaget (1958) |
| `PiagetCognitionWeighter` | action weights | Play ≈ assimilation; imitation / accommodate ≈ accommodation | Piaget (1951). Mix coefficients: **project convention** |

## What Piaget actually argued

### Intelligence as adaptation (1936 / 1952)

*The Origins of Intelligence in Children* (Piaget, 1952; French 1936) treats intelligence as biological **adaptation**. Two complementary processes keep the organism in contact with the environment:

- **Assimilation** — current **schemas** (organized action patterns) take in new material without changing form. The infant who already grasps treats a new rattle as another thing-to-grasp.
- **Accommodation** — schemas change when the material will not fit. The same infant must alter grasp for a much larger object.

Neither process is sufficient alone. Adaptation is their **equilibrium**. This is constructivism, not associationism: knowledge is neither a copy of the world nor an unfolding of innate ideas, but a construction through action.

### Object permanence and the construction of reality (1937 / 1954)

*The Construction of Reality in the Child* (Piaget, 1954; French 1937) traces how the infant builds a world of permanent objects, spatial relations, causality, and time. **Object permanence** — the object continues to exist when out of sight — is not given at birth. It is constructed across sensorimotor substages, from no search, through search at the last seen location (including the famous A-not-B error), to invisible displacements.

This is a **cognitive** achievement, not a mood or a trait. An NPC that "forgets" unseen objects is a sensorimotor-stage convention, not a Neuroticism score.

### Stages of operational thought (1947 / 1950; 1958)

*The Psychology of Intelligence* (Piaget, 1950; French 1947) and *The Growth of Logical Thinking from Childhood to Adolescence* (Inhelder & Piaget, 1958) describe qualitative reorganizations of thought:

| Stage | Rough ages (descriptive, not a game clock) | What becomes possible |
| --- | --- | --- |
| Sensorimotor | ~0–2 | Action schemas; object permanence by the end |
| Preoperational | ~2–7 | Symbolic thought; **egocentrism**; failure of **conservation** |
| Concrete operational | ~7–11 | Conservation, classification, seriation — on concrete material |
| Formal operational | ~11+ | **Hypothetical-deductive** reasoning; thinking about thinking |

Piaget's ages are **observational summaries**, not genetic deadlines. Cross-cultural and training studies later showed that timing varies; the **order** of structures is the claim that mattered to him. Personality Engine therefore treats stage as **host-set**, never as an automatic function of event count.

**Conservation** (Piaget, 1950): quantity, mass, or number stays the same across irrelevant transformations (pouring liquid, spreading coins). Preoperational children typically fail; concrete operational children succeed. This is a structural limit, not stubbornness.

**Egocentrism** (Piaget, 1950): difficulty taking another spatial or conceptual viewpoint. It declines as operational reversibility develops. It is not the Big Five low-Agreeableness cluster.

**Hypothetical-deductive reasoning** (Inhelder & Piaget, 1958): the formal adolescent can treat hypotheses as hypotheses, isolate variables, and reason from the merely possible. Concrete operational thought remains bound to the given.

### Play, imitation, and the two poles of adaptation (1945 / 1951)

*Play, Dreams and Imitation in Childhood* (Piaget, 1951; French 1945) maps childhood activity onto assimilation and accommodation:

- **Play** (especially symbolic play) is **assimilation** predominating: the child bends the world to the schema ("this stick is a horse").
- **Imitation** is **accommodation** predominating: the child bends the schema to the model.
- **Intelligent adaptation** is their balance.

Dreams, for Piaget, continue assimilative play. Personality Engine does **not** implement a dream engine; it uses the play/imitation polarity as **action tags**.

### Equilibration as the motor of development (1975 / 1985)

*The Equilibration of Cognitive Structures* (Piaget, 1985; French 1975) makes **equilibration** — not maturation or social transmission alone — the central developmental mechanism. When assimilation fails, **disequilibrium** motivates accommodation and the construction of a higher-order scheme. *Genetic Epistemology* (Piaget, 1970) states the same program as a theory of knowledge: structures are neither innate nor copied; they are constructed.

Personality Engine's `disequilibrium` channel is a **project-convention scalar** standing in for that pressure. It is not a psychometric of "cognitive dissonance" in the Festinger sense, and it is not Peterson's **chaos**.

## What is in this module

| Piece | Source | In code |
| --- | --- | --- |
| Assimilation / accommodation / equilibrium | Piaget 1952; 1985 | `cognition.piaget-equilibration.assimilation`, `.accommodation`, `.equilibrium`, `.disequilibrium` |
| Four major stages (host-set) | Piaget 1950; Inhelder & Piaget 1958 | `CognitiveStage`; `cognition.piaget-equilibration.stage-index`, `.stage-progress` |
| Object permanence as a stage-gated flag | Piaget 1954 | `cognition.piaget-equilibration.object-permanence` (0 in sensorimotor at start; 1 thereafter) |
| Conservation / egocentrism / hypothetical thought as stage-gated flags | Piaget 1950; Inhelder & Piaget 1958 | `.conservation`, `.egocentrism`, `.hypothetical` |
| Play ≈ assimilation, imitation ≈ accommodation | Piaget 1951 | `PiagetCognitionWeighter` tags `piaget.play`, `piaget.imitate`, `piaget.accommodate`, `piaget.explore` |
| Encounter misfit as disequilibrium pressure | Piaget 1985 | Event `piaget.encounter` with `Intensity` as project-convention misfit 0..1 |

**Project conventions** (labeled in XML docs, not smuggled in as Piaget's numbers):

- Misfit ≤ 0.40 on `piaget.encounter` counts as assimilable; above that, disequilibrium and accommodation pressure rise.
- Stages do **not** auto-advance. The host constructs a new provider (or replaces composition) when the character should reorganize.
- Object permanence is binary at v0.1 (absent at sensorimotor start, present otherwise). Piaget's substages and A-not-B are not scored.
- Conservation and hypothetical thought are binary unlocks at concrete and formal stages. They are not item-response scores from actual conservation tasks.
- Egocentrism is a decaying convention (0.85 / 0.70 / 0.20 / 0.05 by stage), not a measured spatial-perspective error rate.

## Out of scope (and why)

- **Automatic stage transitions.** Piaget's stages are reorganizations over long observation, not a function of a handful of game events. Auto-advancing from `piaget.encounter` would fake development.
- **Conservation-task psychometrics.** We do not implement pouring-liquid trials or report "percent conservers." Flags are stage-gated conventions.
- **Sensorimotor substages 1–6 and A-not-B.** Those need a dedicated infant-cognition model, not a 0..1 channel.
- **Neo-Piagetian information-processing revisions** (Case, Pascual-Leone) and **Vygotsky** (ZPD, inner speech). Different theories; add them as sibling providers if a host wants them.
- **Stuffing Piaget into Peterson meaning.** Both describe "this does not fit, reorganize" — but Piaget's object is **cognitive schemas**, Peterson's is **affective meaning and ideology** (Maps of Meaning; CMT). Chaos and disequilibrium can rise together; they are not aliases.
- **Stuffing Piaget into Skinner.** Piaget explicitly rejected explaining knowledge as a history of reinforcements. Operant `learning.*` channels remain a separate layer.
- **Moral stages** in the Kohlberg line. Related historically, not Piaget's operational theory, not this module.

## Relation to Peterson and Skinner

| | Piaget | Peterson | Skinner |
| --- | --- | --- | --- |
| Object | Cognitive schemas and operations | Affective meaning / ideology | Operant repertoire |
| “This does not fit” | Disequilibrium → accommodation | Chaos / anomaly → explore, defend, or integrate | Change in contingencies |
| Development | Host-set stages; equilibration | Not a stage theory | History of reinforcement |
| Play | Assimilation predominating (1951) | Not modeled | Not a schema process |

Hosts may run all three: Peterson can mark an anomaly as *meaningful*, Piaget can mark it as *structurally misfitting*, Skinner can change response strength after the consequence. That stacking is **project design**, not a joint theory any of them published.

## Events

| `WorldEvent.Kind` | Effect |
| --- | --- |
| `piaget.encounter` | `Intensity` = misfit of the material to current schemas. Optional `Target` is the object or situation id (unused in v0.1 scoring). Low misfit → assimilation; high misfit → disequilibrium + accommodation pressure. |
| `piaget.assimilate` | Direct assimilation pulse (host already judged the fit). |
| `piaget.accommodate` | Schema change: accommodation up, equilibrium restored. |
| `piaget.equilibrate` | Restore equilibrium without a full accommodation pulse. |

Unknown kinds are ignored.

## Composition

Piaget-only:

```csharp
var engine = PiagetComposition.Create(CognitiveStage.ConcreteOperational);
engine.Tick(WorldEvent.Tick);
engine.Tick(new WorldEvent(PiagetEquilibrationProvider.EncounterKind, 0.2f, "new-block"));
var weights = engine.WeightActions(new[]
{
    PiagetCognitionWeighter.Play,
    PiagetCognitionWeighter.Accommodate
});
```

With Ocean + Peterson (cognition does not replace mood or meaning):

```csharp
var engine = PiagetComposition.CreateWithOceanAndPeterson(
    OceanTraits.GebhardExample,
    CognitiveStage.FormalOperational);
```

Action tags for the weighter: `piaget.play`, `piaget.imitate`, `piaget.accommodate`, `piaget.explore`.

## References

- [Piaget (1952)](https://openlibrary.org/works/OL458047W). *The Origins of Intelligence in Children* (orig. 1936).
- [Piaget (1954)](https://openlibrary.org/works/OL458050W). *The Construction of Reality in the Child* (orig. 1937).
- [Piaget (1950)](https://openlibrary.org/works/OL458043W). *The Psychology of Intelligence* (orig. 1947).
- [Piaget (1951)](https://openlibrary.org/works/OL458044W). *Play, Dreams and Imitation in Childhood* (orig. 1945).
- [Inhelder & Piaget (1958)](https://openlibrary.org/works/OL458031W). *The Growth of Logical Thinking from Childhood to Adolescence.*
- [Piaget (1970)](https://archive.org/details/geneticepistemol0000piag). *Genetic Epistemology.*
- [Piaget (1985)](https://press.uchicago.edu/ucp/books/book/chicago/E/bo3628970.html). *The Equilibration of Cognitive Structures* (orig. 1975).

Full registry: [Citations](CITATIONS.md).
