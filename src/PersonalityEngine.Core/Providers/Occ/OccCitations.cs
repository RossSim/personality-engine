namespace PersonalityEngine.Providers.Occ;

internal static class OccCitations
{
    public static readonly Citation Occ1988 = new Citation(
        "occ-1988",
        "Ortony, A., Clore, G. L., & Collins, A. (1988). The Cognitive Structure of Emotions. Cambridge University Press. Emotion types from eliciting conditions (well-being, prospect-based, attribution).");

    public static readonly Citation GebhardAlma2005 = new Citation(
        "gebhard-alma-2005",
        "Gebhard, P. (2005). ALMA: A Layered Model of Affect. AAMAS. First wiring: OCC emotions influence PAD mood. Not exclusive glue.");

    public static readonly Citation Dynamics = new Citation(
        "pe-occ-dynamics",
        "Host-tagged eliciting events, 0..1 intensities, exponential decay toward 0, and OCC→PAD overlay coefficients are project conventions. This slice is not a full OCC goal/standard/attitude network.",
        isProjectConvention: true);
}
