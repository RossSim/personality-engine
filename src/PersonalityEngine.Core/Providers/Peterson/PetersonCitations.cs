namespace PersonalityEngine.Providers.Peterson;

internal static class PetersonCitations
{
    public static readonly Citation DeYoungPetersonHiggins2002 = new Citation(
        "deyoung-peterson-higgins-2002",
        "DeYoung, C. G., Peterson, J. B., & Higgins, D. M. (2002). Higher-order factors of the Big Five predict conformity: Are there neuroses of health? Personality and Individual Differences, 33(4), 533–552. https://doi.org/10.1016/S0191-8869(01)00171-4");

    public static readonly Citation Digman1997 = new Citation(
        "digman-1997",
        "Digman, J. M. (1997). Higher-order factors of the Big Five. Journal of Personality and Social Psychology, 73(6), 1246–1256. https://doi.org/10.1037/0022-3514.73.6.1246");

    public static readonly Citation MapsOfMeaning1999 = new Citation(
        "peterson-maps-1999",
        "Peterson, J. B. (1999). Maps of Meaning: The Architecture of Belief. Routledge. https://www.routledge.com/Maps-of-Meaning-The-Architecture-of-Belief/Peterson/p/book/9780415922227");

    public static readonly Citation ComplexityManagement2002 = new Citation(
        "peterson-flanders-2002",
        "Peterson, J. B., & Flanders, J. L. (2002). Complexity Management Theory: Motivation for ideological rigidity and social conflict. Cortex, 38(3), 429–458. https://doi.org/10.1016/S0010-9452(08)70680-4");

    public static readonly Citation ThreeFormsOfMeaning2013 = new Citation(
        "peterson-meaning-2013",
        "Peterson, J. B. (2013). Three forms of meaning and the management of complexity. In K. Markman, T. Proulx, & M. Lindberg (Eds.), The psychology of meaning. American Psychological Association. https://doi.org/10.1037/13944-005");

    public static readonly Citation EqualWeightAggregation = new Citation(
        "pe-equal-weight-metatraits",
        "Equal-weight means for Stability (1−N, A, C) and Plasticity (E, O), and the 0..1 conformity mapping, are project conventions. DeYoung, Peterson & Higgins (2002) report factor scores and SEM betas, not these game-ready averages.",
        isProjectConvention: true);

    public static readonly Citation MeaningDynamics = new Citation(
        "pe-peterson-meaning-dynamics",
        "Numeric gains/decays for order, chaos, logos, and rigidity are project conventions implementing the qualitative dynamics in Peterson (1999) and Peterson & Flanders (2002).",
        isProjectConvention: true);

    public static readonly Citation Hirsh2010 = new Citation(
        "hirsh-deyoung-xu-peterson-2010",
        "Hirsh, J. B., DeYoung, C. G., Xu, X., & Peterson, J. B. (2010). Compassionate liberals and polite conservatives: Associations of agreeableness with political ideology and moral values. Personality and Social Psychology Bulletin, 36(5), 655–664. https://doi.org/10.1177/0146167210366852 Aspect-level (BFAS) associations; not implemented as domain-level OCEAN scores.");
}
