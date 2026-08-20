# Erik Erikson — academic review and in-module mapping

This note reviews Erikson's published research for Personality Engine and records **what is in the `identity` layer**, what is a **project convention**, and what stays **out of scope**.

Layer: `identity`.

Erikson is **not** a trait theory and **not** a theory of cognitive operations. His project is psychosocial development: how the ego finds a sense of sameness and continuity among changing bodily, social, and historical demands. He belongs beside Piaget (schemas), Peterson (affective meaning), and Skinner (operant history), not inside them. Hosts that want lifespan crises and identity commitment compose `EriksonPsychosocialProvider` on the **`identity`** layer; hosts that do not simply omit it.

| Provider | Layer | What it does | Source |
| --- | --- | --- | --- |
| `EriksonPsychosocialProvider` (`erikson-psychosocial`) | identity | Eight host-set ages; syntonic/dystonic ratio; ego identity, moratorium, fidelity, generativity, integrity | Erikson (1963, 1959, 1968, 1982) |
| `EriksonIdentityWeighter` | action weights | Explore ≈ moratorium; commit ≈ fidelity; care ≈ generativity; withdraw ≈ confusion/despair | Erikson (1968). Mix coefficients: **project convention** |

## What Erikson actually argued

### The epigenetic principle and the eight ages (1950 / 1963)

*Childhood and Society* (Erikson, 1963; first edition 1950) states the **epigenetic principle**: anything that grows has a ground plan, and out of this ground plan the parts arise, each part having its time of special ascendancy, until all parts form a functioning whole. Personality, for Erikson, is this sequence of psychosocial **crises**, not a list of traits.

Each age is a conflict between a **syntonic** (adaptive) pole and a **dystonic** (maladaptive) pole. The healthy outcome is a **favorable ratio** with the syntonic predominating — not the annihilation of the dystonic pole. Mistrust, shame, guilt, and doubt remain available; they become dangerous when they dominate.

The eight ages (Erikson, 1963, ch. 7):

| Age | Crisis | Emerging virtue (1982) |
| --- | --- | --- |
| Infancy | Trust vs mistrust | Hope |
| Early childhood | Autonomy vs shame/doubt | Will |
| Play age | Initiative vs guilt | Purpose |
| School age | Industry vs inferiority | Competence |
| Adolescence | Identity vs role confusion | Fidelity |
| Young adulthood | Intimacy vs isolation | Love |
| Adulthood | Generativity vs stagnation | Care |
| Old age | Integrity vs despair | Wisdom |

Ages in years are **descriptive**, not a game clock. Cross-cultural timing varies; the **order** of the ground plan is the claim. Personality Engine therefore treats stage as **host-set**, never as an automatic function of event count. That is the same honesty rule as Piaget's stages in this library.

### Ego identity, crisis, and moratorium (1959, 1968)

*Identity and the Life Cycle* (Erikson, 1959) and *Identity: Youth and Crisis* (Erikson, 1968) make **ego identity** the through-line: a sense of **sameness and continuity** of the self across time and social recognition. Identity is psychosocial, not a private feeling. It must be confirmed by a community of others.

**Identity crisis** is the adolescent turning point at which earlier identifications are recast into a durable configuration. It is not a synonym for panic, and it is not Peterson's **chaos** (unknown in a meaning system). A character can know the world is dangerous and still not know who they are.

**Role confusion** (identity diffusion): the inability to settle on a vocational, sexual, and ideological role. Drift, not a mood.

**Psychosocial moratorium**: a sanctioned delay during which the youth may experiment with roles before irreversible adult commitment. Play, apprenticeship, travel, and ideology-shopping belong here when the host marks them as exploration rather than as a finished identity.

**Negative identity**: an identity perversely based on roles that were presented as most undesirable. It is still an identity — a commitment — not mere confusion. Erikson (1968) treats it as a real configuration, often socially recognized by a delinquent or rejected group.

**Fidelity** is the adolescent virtue: the ability to sustain loyalties freely pledged despite inevitable value contradictions (Erikson, 1968; 1982).

### Generativity and integrity (1963, 1982)

**Generativity** is concern for establishing and guiding the next generation — not only one's own children, but work, ideas, and institutions that outlive the self (Erikson, 1963). Its dystonic opposite is **stagnation**.

**Ego integrity** is the accrued assurance of one's one and only life cycle as something that had to be: acceptance of the life that was lived, vs **despair** and fear of death (Erikson, 1963; 1982). *The Life Cycle Completed* (1982) restates the virtues and the last crisis.

### Psychohistory (1958, 1969)

*Young Man Luther* (1958) and *Gandhi's Truth* (1969) apply the life-cycle chart to historical figures. They are studies of identity in history, not algorithms for generating NPC backstories. Personality Engine records them as sources and does **not** implement a psychohistory engine.

## What is in this module

| Piece | Source | In code |
| --- | --- | --- |
| Eight ages (host-set) | Erikson 1963 | `PsychosocialStage`; `identity.erikson-psychosocial.stage-index`, `.stage-progress` |
| Syntonic / dystonic ratio | Erikson 1963 | `.syntonic`, `.dystonic`, `.ratio` (virtue of the current age ≈ ratio) |
| Ego identity / role confusion / fidelity | Erikson 1959, 1968 | `.ego-identity`, `.role-confusion`, `.fidelity` |
| Moratorium vs commitment | Erikson 1968 | `.moratorium`; events `erikson.explore`, `erikson.commit` |
| Negative identity | Erikson 1968 | `.negative-identity`; event `erikson.negative-identity` |
| Identity-crisis flag | Erikson 1968 | `.identity-crisis` = 1 only at Identity vs Role Confusion |
| Generativity / stagnation | Erikson 1963 | `.generativity`, `.stagnation` (zero before adulthood stage) |
| Integrity / despair | Erikson 1963, 1982 | `.integrity`, `.despair` (zero before old age) |
| Explore / commit / care / withdraw | Erikson 1968 | `EriksonIdentityWeighter` tags |

**Project conventions** (labeled in XML docs, not smuggled in as Erikson's numbers):

- Stages do **not** auto-advance. The host constructs a new provider (or replaces composition) when the character should enter another age.
- A slightly favorable starting ratio (syntonic 0.55 / dystonic 0.45) stands in for Erikson's "predominance," not a published constant.
- Identity-stage seeds raise moratorium and role confusion relative to other ages. Later stages seed higher ego-identity and fidelity as a **host-set implication**, not proof that earlier crises were resolved.
- Virtue is the current syntonic ratio, not eight separately normed EPSI scales.
- Generativity and integrity channels are zero before their ages of ascendancy. Acts of care (`erikson.generate`) and life review (`erikson.review`) are ignored until those stages.

## Out of scope (and why)

- **Automatic stage transitions.** Erikson's ages are a ground plan over a life, not a function of a handful of game events.
- **James Marcia's identity statuses** (1966: diffusion, foreclosure, moratorium, achievement). Marcia operationalized Erikson for research interviews. Distinct theory; add a sibling provider if a host wants statuses as such.
- **Joan Erikson's ninth stage** (1997 edition of *The Life Cycle Completed*): gerotranscendence in very old age. Documented; not in this slice.
- **EPSI / EOM-EIS / MEIM psychometrics.** We do not claim 0..1 channels are inventory scores.
- **Psychohistory as biography generation.** *Young Man Luther* and *Gandhi's Truth* stay citations, not providers.
- **Stuffing Erikson into OCEAN.** Ego identity is not Extraversion or low Neuroticism. A stable Big Five profile can still be role-confused.
- **Stuffing Erikson into Piaget.** Formal operations can accompany identity diffusion. Cognitive stage ≠ psychosocial age.
- **Stuffing Erikson into Peterson meaning.** Identity crisis is not chaos. Anomaly in a map of meaning and diffusion of roles can co-occur; they are not aliases.
- **Stuffing Erikson into Skinner.** Fidelity is not an operant rate. Reinforced repertoires remain the `learning` layer.

## Relation to Piaget, Peterson, and Skinner

| | Erikson | Piaget | Peterson | Skinner |
| --- | --- | --- | --- | --- |
| Object | Ego identity; psychosocial crises | Cognitive schemas and operations | Affective meaning / ideology | Operant repertoire |
| “This does not fit” | Role confusion; dystonic predominance | Disequilibrium → accommodation | Chaos / anomaly | Change in contingencies |
| Development | Host-set eight ages; epigenetic order | Host-set cognitive stages | Not a stage theory | History of reinforcement |
| Adolescence | Identity vs role confusion; moratorium | Formal / hypothetical thought | Logos vs rigidity | Schedule in force |

Hosts may run several at once: Piaget can mark what the adolescent can *think*; Erikson marks whether they know *who they are*; Peterson marks whether the world still *means*; Skinner marks what has *paid off*. That stacking is **project design**, not a joint theory any of them published.

## Events

| `WorldEvent.Kind` | Effect |
| --- | --- |
| `erikson.challenge` | Psychosocial demand. Raises dystonic and role confusion; at the identity age also raises moratorium. |
| `erikson.support` | Syntonic experience (reliable care, recognized work, confirmed identity, etc.). Raises syntonic ratio. |
| `erikson.rupture` | Dystonic blow. Raises dystonic and role confusion; lowers ego-identity. |
| `erikson.explore` | Psychosocial moratorium. Raises moratorium. |
| `erikson.commit` | Identity / fidelity commitment. Raises ego-identity and fidelity; lowers moratorium. |
| `erikson.negative-identity` | Commitment to a rejected role. Raises negative-identity and some ego-identity; lowers fidelity. |
| `erikson.generate` | Generative act. Raises generativity only from the adulthood stage onward; ignored earlier. |
| `erikson.review` | Life review. Moves integrity vs despair only at old age. |

Unknown kinds are ignored (snapshot is still written).

## Composition

Erikson-only:

```csharp
var engine = EriksonComposition.Create(PsychosocialStage.IdentityVsRoleConfusion);
engine.Tick(WorldEvent.Tick);
engine.Tick(new WorldEvent(EriksonPsychosocialProvider.ExploreKind, 1f));
var weights = engine.WeightActions(new[]
{
    EriksonIdentityWeighter.Explore,
    EriksonIdentityWeighter.Commit
});
```

With Ocean + Peterson (identity does not replace mood or meaning):

```csharp
var engine = EriksonComposition.CreateWithOceanAndPeterson(
    OceanTraits.GebhardExample,
    PsychosocialStage.GenerativityVsStagnation);
```

Action tags for the weighter: `erikson.explore`, `erikson.commit`, `erikson.care`, `erikson.withdraw`.
