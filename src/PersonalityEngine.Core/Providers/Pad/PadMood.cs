using System;
using System.Collections.Generic;
using PersonalityEngine.Providers.Ocean;

namespace PersonalityEngine.Providers.Pad;

/// <summary>
/// Current PAD mood, pulled toward the Gebhard-mapped baseline over dt.
/// Mapping channels stay the baseline; these channels are the moving mood.
/// </summary>
public sealed class PadMood : IAffectProvider
{
    public const string ProviderId = "pad-mood";
    public const string PushKind = "pad.push";

    public static readonly string PleasureKey = ChannelKey.Of(AffectLayer.Mood, ProviderId, "pleasure");
    public static readonly string ArousalKey = ChannelKey.Of(AffectLayer.Mood, ProviderId, "arousal");
    public static readonly string DominanceKey = ChannelKey.Of(AffectLayer.Mood, ProviderId, "dominance");

    private float _pleasure;
    private float _arousal;
    private float _dominance;
    private bool _seeded;

    /// <summary>Exponential decay rate in 1/s. Project convention.</summary>
    public float DecayRate { get; init; } = 0.5f;

    public string Id => ProviderId;
    public string Layer => AffectLayer.Mood;

    public Citation Citation { get; } = PadCitations.MehrabianPad;

    public IReadOnlyList<Citation> AdditionalCitations { get; } =
        new[] { PadCitations.GebhardAlma2005, PadCitations.DecayRate };

    public AffectDelta Contribute(WorldEvent ev, float deltaTime, AffectSnapshot snapshot)
    {
        if (!TryReadBaseline(snapshot, out var baselineP, out var baselineA, out var baselineD))
            return new AffectDelta();

        if (!_seeded)
        {
            _pleasure = baselineP;
            _arousal = baselineA;
            _dominance = baselineD;
            _seeded = true;
        }

        if (ev.Kind == PushKind)
        {
            _pleasure += ReadPayload(ev, "pleasure");
            _arousal += ReadPayload(ev, "arousal");
            _dominance += ReadPayload(ev, "dominance");
        }

        if (deltaTime > 0f)
        {
            var alpha = 1f - MathF.Exp(-DecayRate * deltaTime);
            _pleasure += (baselineP - _pleasure) * alpha;
            _arousal += (baselineA - _arousal) * alpha;
            _dominance += (baselineD - _dominance) * alpha;
        }

        return new AffectDelta()
            .Set(PleasureKey, _pleasure)
            .Set(ArousalKey, _arousal)
            .Set(DominanceKey, _dominance);
    }

    public static bool TryRead(AffectSnapshot snapshot, out float pleasure, out float arousal, out float dominance)
    {
        pleasure = arousal = dominance = 0f;
        if (!snapshot.TryGet(PleasureKey, out pleasure)) return false;
        if (!snapshot.TryGet(ArousalKey, out arousal)) return false;
        if (!snapshot.TryGet(DominanceKey, out dominance)) return false;
        return true;
    }

    private static bool TryReadBaseline(
        AffectSnapshot snapshot,
        out float pleasure,
        out float arousal,
        out float dominance)
    {
        pleasure = arousal = dominance = 0f;
        if (!snapshot.TryGet(OceanToPadMapping.PleasureKey, out pleasure)) return false;
        if (!snapshot.TryGet(OceanToPadMapping.ArousalKey, out arousal)) return false;
        if (!snapshot.TryGet(OceanToPadMapping.DominanceKey, out dominance)) return false;
        return true;
    }

    private static float ReadPayload(WorldEvent ev, string channel)
    {
        ev.Payload.TryGetValue(channel, out var delta);
        return delta * ev.Intensity;
    }
}
