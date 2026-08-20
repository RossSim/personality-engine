namespace PersonalityEngine.Providers.Pad;

internal static class PadCitations
{
    public static readonly Citation MehrabianPad = new Citation(
        "mehrabian-pad",
        "Mehrabian, A. Pleasure–Arousal–Dominance emotion/temperament space.");

    public static readonly Citation GebhardAlma2005 = new Citation(
        "gebhard-alma-2005",
        "Gebhard, P. (2005). ALMA: A Layered Model of Affect. AAMAS. Mood is pulled toward a personality-derived PAD baseline; ALMA is the first wiring, not exclusive glue.");

    public static readonly Citation DecayRate = new Citation(
        "pe-pad-mood-decay",
        "Exponential approach of current PAD toward the mapped baseline, the decay rate, and pad.push payload deltas are project conventions. They are not psychometric time constants from Mehrabian or Gebhard.",
        isProjectConvention: true);
}
