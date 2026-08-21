namespace PersonalityEngine.Providers.Occ;

internal static class OccCitations
{
    public static readonly Citation Occ1988 = new Citation(
        "occ-1988",
        "Ortony, A., Clore, G. L., & Collins, A. (1988). The Cognitive Structure of Emotions. Cambridge University Press. Emotion types from eliciting conditions (well-being, prospect-based, attribution, fortune-of-others, well-being+attribution compounds). ISBN 978-0-521-38664-7.");

    public static readonly Citation GebhardAlma2005 = new Citation(
        "gebhard-alma-2005",
        "Gebhard, P. (2005). ALMA: A layered model of affect. In Proceedings of the Fourth International Joint Conference on Autonomous Agents and Multiagent Systems (AAMAS '05) (pp. 29–36). ACM. https://doi.org/10.1145/1082473.1082478 First wiring: OCC emotions influence PAD mood. Not exclusive glue.");

    public static readonly Citation Dynamics = new Citation(
        "pe-occ-dynamics",
        "Host-tagged eliciting events, 0..1 intensities, exponential decay toward 0, and OCC→PAD overlay coefficients (including fortune-of-others and well-being+attribution compounds) are project conventions. This slice is not a full OCC goal/standard/attitude network.",
        isProjectConvention: true);
}
