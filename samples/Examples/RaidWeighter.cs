using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;

namespace PersonalityEngine.Samples.Examples;

/// <summary>
/// Sample-only tint over freeze / flee / aid. Coefficients are project convention.
/// Not a Core weighter; the host still Picks.
/// </summary>
public sealed class RaidWeighter : IActionWeighter
{
    public const string Freeze = "freeze";
    public const string Flee = "flee";
    public const string Aid = "aid";

    public string Id => "examples-raid";

    public Citation Citation { get; } = new Citation(
        "pe-examples-raid",
        "Freeze/flee/aid tints from fear × OCEAN are a sample convention so three seeds can share one Threat pulse.",
        isProjectConvention: true);

    public IReadOnlyDictionary<string, float> Weight(AffectSnapshot snapshot, IReadOnlyList<string> actionIds)
    {
        var fear = snapshot.GetOrDefault(OccEmotion.FearKey);
        var agree = snapshot.GetOrDefault(OceanPersonality.AgreeablenessKey);
        var neuro = snapshot.GetOrDefault(OceanPersonality.NeuroticismKey);
        var cons = snapshot.GetOrDefault(OceanPersonality.ConscientiousnessKey);

        var freeze = Clamp01(fear * neuro);
        var flee = Clamp01(fear * (1f - neuro) * (0.35f + 0.65f * cons));
        var aid = Clamp01(fear * agree * (1f - neuro));

        var result = new Dictionary<string, float>();
        foreach (var id in actionIds)
        {
            if (id == Freeze && freeze > 0f)
                result[id] = freeze;
            else if (id == Flee && flee > 0f)
                result[id] = flee;
            else if (id == Aid && aid > 0f)
                result[id] = aid;
        }

        return result;
    }

    private static float Clamp01(float value) =>
        value < 0f ? 0f : value > 1f ? 1f : value;
}
