namespace PersonalityEngine.Providers.Erikson;

internal static class EriksonCitations
{
    public static readonly Citation ChildhoodAndSociety1963 = new Citation(
        "erikson-1963",
        "Erikson, E. H. (1963). Childhood and Society (2nd ed.). W. W. Norton. https://wwnorton.com/books/9780393310214 (Original work published 1950). Eight ages of man; epigenetic principle; syntonic/dystonic ratio.");

    public static readonly Citation IdentityAndTheLifeCycle1959 = new Citation(
        "erikson-1959",
        "Erikson, E. H. (1959). Identity and the Life Cycle. Psychological Issues, 1(1). International Universities Press. https://archive.org/details/identitylifecycl0000erik");

    public static readonly Citation IdentityYouthAndCrisis1968 = new Citation(
        "erikson-1968",
        "Erikson, E. H. (1968). Identity: Youth and Crisis. W. W. Norton. https://wwnorton.com/books/Identity-Youth-and-Crisis/ Identity crisis, role confusion, psychosocial moratorium, negative identity, fidelity.");

    public static readonly Citation LifeCycleCompleted1982 = new Citation(
        "erikson-1982",
        "Erikson, E. H. (1982). The Life Cycle Completed. W. W. Norton. https://wwnorton.com/books/The-Life-Cycle-Completed/ Virtues of the life cycle; integrity vs despair.");

    public static readonly Citation YoungManLuther1958 = new Citation(
        "erikson-1958",
        "Erikson, E. H. (1958). Young Man Luther: A Study in Psychoanalysis and History. W. W. Norton. https://wwnorton.com/books/Young-Man-Luther/ Psychohistory; documented, not implemented as an NPC biography engine.");

    public static readonly Citation DynamicsAndStageFlags = new Citation(
        "pe-erikson-dynamics",
        "Numeric syntonic/dystonic gains, 0..1 virtues, and stage-gated flags (identity-crisis, generativity, integrity) are project conventions implementing Erikson qualitatively. They are not EPSI, EOM-EIS, or MEIM scores, and they do not auto-advance the eight ages.",
        isProjectConvention: true);
}
