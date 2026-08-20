using System.Collections.Generic;

namespace PersonalityEngine.Providers.Peterson;

/// <summary>
/// Action tags: <c>peterson.explore</c>, <c>peterson.defend</c>, <c>peterson.integrate</c>, <c>peterson.withdraw</c>.
/// </summary>
public sealed class PetersonMeaningWeighter : IActionWeighter
{
    public const string Explore = "peterson.explore";
    public const string Defend = "peterson.defend";
    public const string Integrate = "peterson.integrate";
    public const string Withdraw = "peterson.withdraw";

    public string Id => "peterson-meaning-weighter";

    public Citation Citation { get; } = PetersonCitations.ComplexityManagement2002;

    public IReadOnlyDictionary<string, float> Weight(AffectSnapshot snapshot, IReadOnlyList<string> actionIds)
    {
        var chaos = snapshot.GetOrDefault(OrderChaosMeaningProvider.ChaosKey, 0f);
        var logos = snapshot.GetOrDefault(OrderChaosMeaningProvider.LogosKey, 0.5f);
        var rigidity = snapshot.GetOrDefault(OrderChaosMeaningProvider.RigidityKey, 0f);
        var order = snapshot.GetOrDefault(OrderChaosMeaningProvider.OrderKey, 0.5f);

        var explore = chaos * logos;
        var defend = chaos * rigidity;
        var integrate = logos * (1f - rigidity) * chaos;
        var withdraw = chaos * (1f - logos) * (1f - order);

        var result = new Dictionary<string, float>();
        foreach (var id in actionIds)
        {
            var weight = 0f;
            if (ContainsTag(id, Explore)) weight += explore;
            if (ContainsTag(id, Defend)) weight += defend;
            if (ContainsTag(id, Integrate)) weight += integrate;
            if (ContainsTag(id, Withdraw)) weight += withdraw;
            if (weight > 0f)
                result[id] = weight;
        }

        return result;
    }

    private static bool ContainsTag(string actionId, string tag) =>
        actionId.Equals(tag, System.StringComparison.OrdinalIgnoreCase)
        || actionId.IndexOf(tag, System.StringComparison.OrdinalIgnoreCase) >= 0;
}
