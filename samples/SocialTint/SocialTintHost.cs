using PersonalityEngine;
using PersonalityEngine.Providers.Dyad;
using PersonalityEngine.Providers.Ocean;

namespace PersonalityEngine.Samples.SocialTint;

/// <summary>
/// Host Utility AI keeps Pick. PE tints approach/avoid for two named others.
/// </summary>
public static class SocialTintHost
{
    public const string Ally = "ally";
    public const string Rival = "rival";

    public static readonly string[] Actions =
    {
        DyadWeighter.Approach(Ally),
        DyadWeighter.Avoid(Ally),
        DyadWeighter.Approach(Rival),
        DyadWeighter.Avoid(Rival)
    };

    public static readonly IReadOnlyDictionary<string, float> DefaultBaseScores =
        new Dictionary<string, float>
        {
            [DyadWeighter.Approach(Ally)] = 0.50f,
            [DyadWeighter.Avoid(Ally)] = 0.40f,
            [DyadWeighter.Approach(Rival)] = 0.42f,
            [DyadWeighter.Avoid(Rival)] = 0.55f
        };

    public static AffectEngine CreateEngine() =>
        DyadComposition.CreateWithAlma(OceanTraits.GebhardExample);

    public static string Pick(AffectEngine engine) =>
        HostChooser.Pick(HostChooser.Combine(DefaultBaseScores, engine.WeightActions(Actions)));

    public static IReadOnlyDictionary<string, float> Finals(AffectEngine engine) =>
        HostChooser.Combine(DefaultBaseScores, engine.WeightActions(Actions));
}
