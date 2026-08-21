using System;
using System.Collections.Generic;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Pad;

namespace PersonalityEngine;

/// <summary>
/// Small PAD/OCC tint over opaque host action ids. Does not pick.
/// Coefficients are project convention.
/// </summary>
public sealed class UtilityTintWeighter : IActionWeighter
{
    public const string MeetNeed = "meet-need";
    public const string RoleWork = "role-work";
    public const string Wander = "wander";

    public string Id => "utility-tint";

    public Citation Citation { get; } = new Citation(
        "pe-utility-tint",
        "Folding PAD/OCC into host Utility-AI scores as a small additive tint is a project convention. The host chooser remains the decider.",
        isProjectConvention: true);

    public IReadOnlyDictionary<string, float> Weight(AffectSnapshot snapshot, IReadOnlyList<string> actionIds)
    {
        var pleasure = snapshot.GetOrDefault(PadMood.PleasureKey);
        var dominance = snapshot.GetOrDefault(PadMood.DominanceKey);
        var distress = snapshot.GetOrDefault(OccEmotion.DistressKey);
        var fear = snapshot.GetOrDefault(OccEmotion.FearKey);
        var pride = snapshot.GetOrDefault(OccEmotion.PrideKey);
        var relief = snapshot.GetOrDefault(OccEmotion.ReliefKey);

        var meet = Clamp01(distress + fear + Math.Max(0f, -pleasure));
        var work = Clamp01(pride + 0.5f * Math.Max(0f, dominance) - 0.5f * fear);
        var wander = Clamp01(0.5f * Math.Max(0f, pleasure) + 0.35f * relief - distress - 0.5f * fear);

        var result = new Dictionary<string, float>();
        foreach (var id in actionIds)
        {
            if (Matches(id, MeetNeed) && meet > 0f)
                result[id] = meet;
            else if (Matches(id, RoleWork) && work > 0f)
                result[id] = work;
            else if (Matches(id, Wander) && wander > 0f)
                result[id] = wander;
        }

        return result;
    }

    private static bool Matches(string actionId, string tag) =>
        actionId.Equals(tag, StringComparison.OrdinalIgnoreCase);

    private static float Clamp01(float value) =>
        value < 0f ? 0f : value > 1f ? 1f : value;
}
