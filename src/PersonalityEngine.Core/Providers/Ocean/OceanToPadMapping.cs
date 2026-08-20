using System.Collections.Generic;

namespace PersonalityEngine.Providers.Ocean;

/// <summary>
/// Personality → PAD baseline using Mehrabian coefficients as used in Gebhard ALMA (2005).
/// </summary>
public sealed class OceanToPadMapping : IAffectProvider
{
    public const string ProviderId = "ocean-to-pad";

    public static readonly string PleasureKey = ChannelKey.Of(AffectLayer.Mood, "pad", "pleasure");
    public static readonly string ArousalKey = ChannelKey.Of(AffectLayer.Mood, "pad", "arousal");
    public static readonly string DominanceKey = ChannelKey.Of(AffectLayer.Mood, "pad", "dominance");

    public string Id => ProviderId;
    public string Layer => AffectLayer.Mood;

    public Citation Citation { get; } = new Citation(
        "gebhard-alma-2005",
        "Gebhard, P. (2005). ALMA: A Layered Model of Affect. AAMAS. Uses Mehrabian PAD mapping coefficients.");

    public IReadOnlyList<Citation> AdditionalCitations { get; } =
        new[]
        {
            new Citation(
                "mehrabian-pad",
                "Mehrabian, A. Pleasure–Arousal–Dominance emotion/temperament space.")
        };

    public AffectDelta Contribute(WorldEvent ev, float deltaTime, AffectSnapshot snapshot)
    {
        var delta = new AffectDelta();
        if (!OceanTraits.TryRead(snapshot, out var ocean))
            return delta;

        var pad = Map(ocean);
        return delta
            .Set(PleasureKey, pad.Pleasure)
            .Set(ArousalKey, pad.Arousal)
            .Set(DominanceKey, pad.Dominance);
    }

    public static (float Pleasure, float Arousal, float Dominance) Map(OceanTraits t)
    {
        var pleasure = 0.21f * t.Extraversion + 0.59f * t.Agreeableness + 0.19f * t.Neuroticism;
        var arousal = 0.15f * t.Openness + 0.30f * t.Agreeableness - 0.57f * t.Neuroticism;
        var dominance = 0.25f * t.Openness + 0.17f * t.Conscientiousness + 0.60f * t.Extraversion - 0.32f * t.Agreeableness;
        return (pleasure, arousal, dominance);
    }
}
