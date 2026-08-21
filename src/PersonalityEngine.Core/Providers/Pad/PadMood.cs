using System;
using System.Collections.Generic;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;

namespace PersonalityEngine.Providers.Pad;

/// <summary>
/// Current PAD mood, pulled toward the Gebhard-mapped baseline over dt.
/// Mapping channels stay the baseline; these channels are the moving mood.
/// </summary>
public sealed class PadMood : IAffectProvider, IStatefulProvider
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

    public const string InternalPleasure = "internal.pleasure";
    public const string InternalArousal = "internal.arousal";
    public const string InternalDominance = "internal.dominance";
    public const string SeededKey = "seeded";

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

        var pleasure = _pleasure;
        var arousal = _arousal;
        var dominance = _dominance;
        if (snapshot.TryGet(OccToPadMapping.PleasureKey, out var occP))
            pleasure += occP;
        if (snapshot.TryGet(OccToPadMapping.ArousalKey, out var occA))
            arousal += occA;
        if (snapshot.TryGet(OccToPadMapping.DominanceKey, out var occD))
            dominance += occD;

        return new AffectDelta()
            .Set(PleasureKey, pleasure)
            .Set(ArousalKey, arousal)
            .Set(DominanceKey, dominance);
    }

    public IReadOnlyDictionary<string, float> ExportState()
    {
        if (!_seeded)
            return new Dictionary<string, float>();

        return new Dictionary<string, float>
        {
            [InternalPleasure] = _pleasure,
            [InternalArousal] = _arousal,
            [InternalDominance] = _dominance,
            [SeededKey] = 1f
        };
    }

    public void ImportState(IReadOnlyDictionary<string, float> bag)
    {
        if (bag.TryGetValue(InternalPleasure, out var p))
            _pleasure = p;
        if (bag.TryGetValue(InternalArousal, out var a))
            _arousal = a;
        if (bag.TryGetValue(InternalDominance, out var d))
            _dominance = d;
        if (bag.TryGetValue(SeededKey, out var seeded))
            _seeded = seeded >= 0.5f;
        else
            _seeded = bag.ContainsKey(InternalPleasure);
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
