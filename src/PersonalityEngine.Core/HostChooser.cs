using System.Collections.Generic;

namespace PersonalityEngine;

/// <summary>
/// Fold PE tints into host Utility-AI scores. The host still picks.
/// Additive gain is a project convention.
/// </summary>
public static class HostChooser
{
    public const float DefaultGain = 0.35f;

    public static Dictionary<string, float> Combine(
        IReadOnlyDictionary<string, float> baseScores,
        IReadOnlyDictionary<string, float> tints,
        float gain = DefaultGain)
    {
        var result = new Dictionary<string, float>();
        foreach (var pair in baseScores)
        {
            tints.TryGetValue(pair.Key, out var tint);
            result[pair.Key] = pair.Value + gain * tint;
        }

        return result;
    }

    public static string Pick(IReadOnlyDictionary<string, float> scores)
    {
        string? best = null;
        var bestScore = float.NegativeInfinity;
        foreach (var pair in scores)
        {
            if (pair.Value <= bestScore)
                continue;
            bestScore = pair.Value;
            best = pair.Key;
        }

        return best ?? string.Empty;
    }
}
