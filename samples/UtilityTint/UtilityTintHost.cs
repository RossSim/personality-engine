using PersonalityEngine;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;

namespace PersonalityEngine.Samples.UtilityTint;

/// <summary>
/// Host Utility AI keeps Pick. PE only tints three opaque action ids.
/// </summary>
public static class UtilityTintHost
{
    public static readonly string[] Actions =
    {
        UtilityTintWeighter.MeetNeed,
        UtilityTintWeighter.RoleWork,
        UtilityTintWeighter.Wander
    };

    public static readonly IReadOnlyDictionary<string, float> DefaultBaseScores =
        new Dictionary<string, float>
        {
            [UtilityTintWeighter.MeetNeed] = 0.50f,
            [UtilityTintWeighter.RoleWork] = 0.62f,
            [UtilityTintWeighter.Wander] = 0.48f
        };

    public static AffectEngine CreateEngine() =>
        AlmaComposition.Create(
            OceanTraits.GebhardExample,
            weighters: new IActionWeighter[] { new UtilityTintWeighter() });

    public static string Pick(AffectEngine engine, IReadOnlyDictionary<string, float>? bases = null)
    {
        var tints = engine.WeightActions(Actions);
        return HostChooser.Pick(HostChooser.Combine(bases ?? DefaultBaseScores, tints));
    }

    public static IReadOnlyDictionary<string, float> Finals(AffectEngine engine) =>
        HostChooser.Combine(DefaultBaseScores, engine.WeightActions(Actions));
}
