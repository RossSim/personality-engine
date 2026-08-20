using System.Collections.Generic;

namespace PersonalityEngine.Providers.Skinner;

/// <summary>
/// Action weight ≈ operant strength × deprivation × discriminative control.
/// </summary>
public sealed class OperantWeighter : IActionWeighter
{
    public string Id => "skinner-operant-weighter";

    public Citation Citation { get; } = SkinnerCitations.ScienceAndHumanBehavior1953;

    /// <summary>
    /// When an SD has been specified and is low, multiply strength.
    /// Project convention (discrimination is rarely all-or-none in games).
    /// </summary>
    public float SdAbsentMultiplier { get; init; } = 0.35f;

    public IReadOnlyDictionary<string, float> Weight(AffectSnapshot snapshot, IReadOnlyList<string> actionIds)
    {
        var deprivation = snapshot.GetOrDefault(OperantLearningProvider.DeprivationKey, 0.5f);
        var sdMultiplier = snapshot.TryGet(OperantLearningProvider.SdKey, out var sd)
            ? SdAbsentMultiplier + (1f - SdAbsentMultiplier) * sd
            : 1f;

        var result = new Dictionary<string, float>();
        foreach (var id in actionIds)
        {
            if (!snapshot.TryGet(OperantLearningProvider.StrengthKey(id), out var strength))
                continue;
            var weight = strength * (0.5f + 0.5f * deprivation) * sdMultiplier;
            if (weight > 0f)
                result[id] = weight;
        }

        return result;
    }
}
