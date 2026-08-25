namespace PersonalityEngine.Providers.Pad;

internal static class PadCitations
{
    public static readonly Citation MehrabianPad = new Citation(
        "mehrabian-pad",
        "Mehrabian, A., & Russell, J. A. (1974). An Approach to Environmental Psychology. MIT Press. https://mitpress.mit.edu/9780262131269/an-approach-to-environmental-psychology/ Mehrabian, A. (1996). Pleasure-arousal-dominance: A general framework for describing and measuring individual differences in temperament. Current Psychology, 14(4), 261–292. https://doi.org/10.1007/BF02686918");

    public static readonly Citation GebhardAlma2005 = new Citation(
        "gebhard-alma-2005",
        "Gebhard, P. (2005). ALMA: A layered model of affect. In Proceedings of the Fourth International Joint Conference on Autonomous Agents and Multiagent Systems (AAMAS '05) (pp. 29–36). ACM. https://doi.org/10.1145/1082473.1082478 Mood is pulled toward a personality-derived PAD baseline; ALMA is the first wiring, not exclusive glue.");

    public static readonly Citation DecayRate = new Citation(
        "pe-pad-mood-decay",
        "Exponential approach of current PAD toward the mapped baseline, the decay rate, and pad.push payload deltas are project conventions. They are not psychometric time constants from Mehrabian or Gebhard.",
        isProjectConvention: true);
}
