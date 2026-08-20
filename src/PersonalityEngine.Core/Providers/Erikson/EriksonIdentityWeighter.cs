using System.Collections.Generic;

namespace PersonalityEngine.Providers.Erikson;

/// <summary>
/// Moratorium ≈ exploration before commitment; fidelity ≈ chosen identity (Erikson, 1968).
/// Tags: <c>erikson.explore</c>, <c>erikson.commit</c>, <c>erikson.care</c>, <c>erikson.withdraw</c>.
/// </summary>
public sealed class EriksonIdentityWeighter : IActionWeighter
{
    public const string Explore = "erikson.explore";
    public const string Commit = "erikson.commit";
    public const string Care = "erikson.care";
    public const string Withdraw = "erikson.withdraw";

    public string Id => "erikson-identity-weighter";

    public Citation Citation { get; } = EriksonCitations.IdentityYouthAndCrisis1968;

    public IReadOnlyDictionary<string, float> Weight(AffectSnapshot snapshot, IReadOnlyList<string> actionIds)
    {
        var moratorium = snapshot.GetOrDefault(EriksonPsychosocialProvider.MoratoriumKey, 0.1f);
        var confusion = snapshot.GetOrDefault(EriksonPsychosocialProvider.RoleConfusionKey, 0.25f);
        var identity = snapshot.GetOrDefault(EriksonPsychosocialProvider.EgoIdentityKey, 0.3f);
        var fidelity = snapshot.GetOrDefault(EriksonPsychosocialProvider.FidelityKey, 0.2f);
        var generativity = snapshot.GetOrDefault(EriksonPsychosocialProvider.GenerativityKey, 0f);
        var dystonic = snapshot.GetOrDefault(EriksonPsychosocialProvider.DystonicKey, 0.45f);
        var despair = snapshot.GetOrDefault(EriksonPsychosocialProvider.DespairKey, 0f);

        var explore = moratorium * (0.4f + 0.6f * confusion);
        var commit = identity * fidelity;
        var care = generativity;
        var withdraw = dystonic * confusion + despair * 0.5f;

        var result = new Dictionary<string, float>();
        foreach (var id in actionIds)
        {
            var weight = 0f;
            if (Contains(id, Explore)) weight += explore;
            if (Contains(id, Commit)) weight += commit;
            if (Contains(id, Care)) weight += care;
            if (Contains(id, Withdraw)) weight += withdraw;
            if (weight > 0f)
                result[id] = weight;
        }

        return result;
    }

    private static bool Contains(string actionId, string tag) =>
        actionId.Equals(tag, System.StringComparison.OrdinalIgnoreCase)
        || actionId.IndexOf(tag, System.StringComparison.OrdinalIgnoreCase) >= 0;
}
