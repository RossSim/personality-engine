using PersonalityEngine.Providers.Occ;

namespace PersonalityEngine.Providers.Dyad;

internal static class DyadCitations
{
    public static readonly Citation OccAttitude = OccCitations.Occ1988;

    public static readonly Citation Dynamics = new Citation(
        "pe-dyad-dynamics",
        "Pairwise liking in [-1, 1], like/dislike bump size, and exponential decay toward 0 are project conventions. This provider stores OCC liking as an attitude; it does not appraise fortune-of-others.",
        isProjectConvention: true);
}
