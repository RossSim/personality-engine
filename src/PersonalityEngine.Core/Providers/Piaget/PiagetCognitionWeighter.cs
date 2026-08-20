using System.Collections.Generic;

namespace PersonalityEngine.Providers.Piaget;

/// <summary>
/// Play ≈ pure assimilation; imitation ≈ accommodation (Piaget, 1951).
/// Tags: <c>piaget.play</c>, <c>piaget.imitate</c>, <c>piaget.accommodate</c>, <c>piaget.explore</c>.
/// </summary>
public sealed class PiagetCognitionWeighter : IActionWeighter
{
    public const string Play = "piaget.play";
    public const string Imitate = "piaget.imitate";
    public const string Accommodate = "piaget.accommodate";
    public const string Explore = "piaget.explore";

    public string Id => "piaget-cognition-weighter";

    public Citation Citation { get; } = PiagetCitations.PlayDreamsImitation1951;

    public IReadOnlyDictionary<string, float> Weight(AffectSnapshot snapshot, IReadOnlyList<string> actionIds)
    {
        var eq = snapshot.GetOrDefault(PiagetEquilibrationProvider.EquilibriumKey, 0.5f);
        var dis = snapshot.GetOrDefault(PiagetEquilibrationProvider.DisequilibriumKey, 0.5f);
        var assim = snapshot.GetOrDefault(PiagetEquilibrationProvider.AssimilationKey, 0.5f);
        var accom = snapshot.GetOrDefault(PiagetEquilibrationProvider.AccommodationKey, 0.3f);

        var play = eq * assim;
        var imitate = dis * accom;
        var accommodate = dis * (0.5f + 0.5f * accom);
        var explore = dis * (1f - accom);

        var result = new Dictionary<string, float>();
        foreach (var id in actionIds)
        {
            var weight = 0f;
            if (Contains(id, Play)) weight += play;
            if (Contains(id, Imitate)) weight += imitate;
            if (Contains(id, Accommodate)) weight += accommodate;
            if (Contains(id, Explore)) weight += explore;
            if (weight > 0f)
                result[id] = weight;
        }

        return result;
    }

    private static bool Contains(string actionId, string tag) =>
        actionId.Equals(tag, System.StringComparison.OrdinalIgnoreCase)
        || actionId.IndexOf(tag, System.StringComparison.OrdinalIgnoreCase) >= 0;
}
