using System.Collections.Generic;
using PersonalityEngine.Providers.Ocean;

namespace PersonalityEngine.Providers.Peterson;

/// <summary>
/// Higher-order Big Five metatraits: Stability and Plasticity.
/// DeYoung, Peterson &amp; Higgins (2002), after Digman (1997).
/// </summary>
public sealed class StabilityPlasticityProvider : IAffectProvider
{
    public const string ProviderId = "peterson-metatraits";

    public static readonly string StabilityKey = ChannelKey.Of(AffectLayer.Personality, ProviderId, "stability");
    public static readonly string PlasticityKey = ChannelKey.Of(AffectLayer.Personality, ProviderId, "plasticity");
    public static readonly string ConformityKey = ChannelKey.Of(AffectLayer.Personality, ProviderId, "conformity-tendency");

    private readonly OceanTraits? _fallback;

    public StabilityPlasticityProvider(OceanTraits? fallbackTraits = null) => _fallback = fallbackTraits;

    public string Id => ProviderId;
    public string Layer => AffectLayer.Personality;

    public Citation Citation { get; } = PetersonCitations.DeYoungPetersonHiggins2002;

    public IReadOnlyList<Citation> AdditionalCitations { get; } =
        new[]
        {
            PetersonCitations.Digman1997,
            PetersonCitations.EqualWeightAggregation
        };

    public AffectDelta Contribute(WorldEvent ev, float deltaTime, AffectSnapshot snapshot)
    {
        var delta = new AffectDelta();
        if (!TryResolveTraits(snapshot, out var traits))
            return delta;

        var metatraits = Compute(traits);
        return delta
            .Set(StabilityKey, metatraits.Stability)
            .Set(PlasticityKey, metatraits.Plasticity)
            .Set(ConformityKey, metatraits.ConformityTendency);
    }

    /// <summary>
    /// Stability = mean(1−N, A, C); Plasticity = mean(E, O).
    /// Equal-weight means are a project convenience; the paper uses factor scores.
    /// </summary>
    public static (float Stability, float Plasticity, float ConformityTendency) Compute(OceanTraits t)
    {
        var emotionalStability = 1f - t.Neuroticism;
        var stability = (emotionalStability + t.Agreeableness + t.Conscientiousness) / 3f;
        var plasticity = (t.Extraversion + t.Openness) / 2f;
        // Directional hypothesis from DeYoung et al. (2002): Stability +, Plasticity −.
        // Mapping onto 0..1 is a project convention, not the SEM betas.
        var conformity = Clamp01(0.5f + 0.5f * (stability - plasticity));
        return (stability, plasticity, conformity);
    }

    private bool TryResolveTraits(AffectSnapshot snapshot, out OceanTraits traits)
    {
        if (OceanTraits.TryRead(snapshot, out traits))
            return true;
        if (_fallback.HasValue)
        {
            traits = _fallback.Value;
            return true;
        }

        traits = default;
        return false;
    }

    private static float Clamp01(float value) =>
        value < 0f ? 0f : value > 1f ? 1f : value;
}
