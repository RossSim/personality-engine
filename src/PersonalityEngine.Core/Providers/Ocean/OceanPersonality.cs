using System.Collections.Generic;

namespace PersonalityEngine.Providers.Ocean;

/// <summary>Stable Big Five / OCEAN personality. McCrae &amp; Costa.</summary>
public sealed class OceanPersonality : IAffectProvider
{
    public const string ProviderId = "ocean";

    public static readonly string OpennessKey = ChannelKey.Of(AffectLayer.Personality, ProviderId, "openness");
    public static readonly string ConscientiousnessKey = ChannelKey.Of(AffectLayer.Personality, ProviderId, "conscientiousness");
    public static readonly string ExtraversionKey = ChannelKey.Of(AffectLayer.Personality, ProviderId, "extraversion");
    public static readonly string AgreeablenessKey = ChannelKey.Of(AffectLayer.Personality, ProviderId, "agreeableness");
    public static readonly string NeuroticismKey = ChannelKey.Of(AffectLayer.Personality, ProviderId, "neuroticism");

    private readonly OceanTraits _traits;

    public OceanPersonality(OceanTraits traits) => _traits = traits;

    public string Id => ProviderId;
    public string Layer => AffectLayer.Personality;

    public Citation Citation { get; } = OceanCitations.FiveFactor;

    public IReadOnlyList<Citation> AdditionalCitations { get; } = System.Array.Empty<Citation>();

    public AffectDelta Contribute(WorldEvent ev, float deltaTime, AffectSnapshot snapshot)
    {
        return new AffectDelta()
            .Set(OpennessKey, _traits.Openness)
            .Set(ConscientiousnessKey, _traits.Conscientiousness)
            .Set(ExtraversionKey, _traits.Extraversion)
            .Set(AgreeablenessKey, _traits.Agreeableness)
            .Set(NeuroticismKey, _traits.Neuroticism);
    }
}
