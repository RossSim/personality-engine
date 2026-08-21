using System.Collections.Generic;

namespace PersonalityEngine.Providers.Occ;

/// <summary>
/// First ALMA-style glue: OCC intensities → PAD overlay. Coefficients are project convention.
/// Does not require PadMood; missing emotion channels yield no overlay.
/// </summary>
public sealed class OccToPadMapping : IAffectProvider
{
    public const string ProviderId = "occ-to-pad";

    public static readonly string PleasureKey = ChannelKey.Of(AffectLayer.Mood, ProviderId, "pleasure");
    public static readonly string ArousalKey = ChannelKey.Of(AffectLayer.Mood, ProviderId, "arousal");
    public static readonly string DominanceKey = ChannelKey.Of(AffectLayer.Mood, ProviderId, "dominance");

    public string Id => ProviderId;
    public string Layer => AffectLayer.Mood;
    public Citation Citation { get; } = OccCitations.GebhardAlma2005;
    public IReadOnlyList<Citation> AdditionalCitations { get; } =
        new[] { OccCitations.Occ1988, OccCitations.Dynamics };

    public AffectDelta Contribute(WorldEvent ev, float deltaTime, AffectSnapshot snapshot)
    {
        float p = 0f, a = 0f, d = 0f;
        var any = false;

        any |= Add(snapshot, OccEmotion.JoyKey, 0.6f, 0.3f, 0.1f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.DistressKey, -0.6f, 0.4f, -0.1f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.HopeKey, 0.3f, 0.2f, 0.1f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.FearKey, -0.4f, 0.5f, -0.3f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.SatisfactionKey, 0.5f, 0.1f, 0.2f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.FearsConfirmedKey, -0.5f, 0.3f, -0.2f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.ReliefKey, 0.4f, -0.2f, 0.2f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.DisappointmentKey, -0.4f, 0.2f, -0.2f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.PrideKey, 0.4f, 0.2f, 0.5f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.ShameKey, -0.5f, 0.3f, -0.5f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.AdmirationKey, 0.3f, 0.1f, 0.0f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.ReproachKey, -0.3f, 0.3f, 0.2f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.HappyForKey, 0.5f, 0.2f, 0.1f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.PityKey, -0.5f, 0.3f, -0.2f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.ResentmentKey, -0.4f, 0.4f, 0.1f, ref p, ref a, ref d);
        any |= Add(snapshot, OccEmotion.GloatingKey, 0.4f, 0.3f, 0.3f, ref p, ref a, ref d);

        if (!any)
            return new AffectDelta();

        return new AffectDelta()
            .Set(PleasureKey, p)
            .Set(ArousalKey, a)
            .Set(DominanceKey, d);
    }

    private static bool Add(
        AffectSnapshot snapshot,
        string key,
        float pleasure,
        float arousal,
        float dominance,
        ref float p,
        ref float a,
        ref float d)
    {
        if (!snapshot.TryGet(key, out var intensity))
            return false;
        p += intensity * pleasure;
        a += intensity * arousal;
        d += intensity * dominance;
        return true;
    }
}
