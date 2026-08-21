using System;
using System.Collections.Generic;
using PersonalityEngine.Providers.Occ;

namespace PersonalityEngine.Providers.Dyad;

/// <summary>
/// Tints opaque <c>approach:{other}</c> / <c>avoid:{other}</c> ids from dyad liking
/// and global fortune-of-others. Does not pick.
/// </summary>
public sealed class DyadWeighter : IActionWeighter
{
    public const string ApproachPrefix = "approach:";
    public const string AvoidPrefix = "avoid:";

    public string Id => "dyad-weighter";

    public Citation Citation { get; } = DyadCitations.Dynamics;

    public IReadOnlyDictionary<string, float> Weight(AffectSnapshot snapshot, IReadOnlyList<string> actionIds)
    {
        var fortuneApproach = snapshot.GetOrDefault(OccEmotion.HappyForKey)
            + snapshot.GetOrDefault(OccEmotion.PityKey);
        var fortuneAvoid = snapshot.GetOrDefault(OccEmotion.GloatingKey)
            + snapshot.GetOrDefault(OccEmotion.ResentmentKey);

        var result = new Dictionary<string, float>();
        foreach (var id in actionIds)
        {
            if (TryOther(id, ApproachPrefix, out var other))
            {
                var liking = snapshot.GetOrDefault(DyadProvider.LikingKey(other));
                var tint = Math.Max(0f, liking) + 0.25f * fortuneApproach;
                if (tint > 0f)
                    result[id] = Clamp01(tint);
            }
            else if (TryOther(id, AvoidPrefix, out other))
            {
                var liking = snapshot.GetOrDefault(DyadProvider.LikingKey(other));
                var tint = Math.Max(0f, -liking) + 0.25f * fortuneAvoid;
                if (tint > 0f)
                    result[id] = Clamp01(tint);
            }
        }

        return result;
    }

    public static string Approach(string otherId) => ApproachPrefix + otherId;

    public static string Avoid(string otherId) => AvoidPrefix + otherId;

    private static bool TryOther(string actionId, string prefix, out string other)
    {
        if (actionId.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            && actionId.Length > prefix.Length)
        {
            other = actionId.Substring(prefix.Length);
            return other.Length > 0;
        }

        other = string.Empty;
        return false;
    }

    private static float Clamp01(float value) =>
        value < 0f ? 0f : value > 1f ? 1f : value;
}
