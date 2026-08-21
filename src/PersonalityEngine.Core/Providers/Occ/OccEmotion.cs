using System;
using System.Collections.Generic;

namespace PersonalityEngine.Providers.Occ;

/// <summary>
/// OCC emotion types as named channels. Hosts tag eliciting conditions;
/// this slice does not infer goals, standards, or attitudes from untyped world state.
/// </summary>
public sealed class OccEmotion : IAffectProvider, IStatefulProvider
{
    public const string ProviderId = "occ";

    public const string JoyKind = "occ.joy";
    public const string DistressKind = "occ.distress";
    public const string HopeKind = "occ.hope";
    public const string FearKind = "occ.fear";
    public const string SatisfactionKind = "occ.satisfaction";
    public const string FearsConfirmedKind = "occ.fears-confirmed";
    public const string ReliefKind = "occ.relief";
    public const string DisappointmentKind = "occ.disappointment";
    public const string PrideKind = "occ.pride";
    public const string ShameKind = "occ.shame";
    public const string AdmirationKind = "occ.admiration";
    public const string ReproachKind = "occ.reproach";

    public static readonly string JoyKey = Key("joy");
    public static readonly string DistressKey = Key("distress");
    public static readonly string HopeKey = Key("hope");
    public static readonly string FearKey = Key("fear");
    public static readonly string SatisfactionKey = Key("satisfaction");
    public static readonly string FearsConfirmedKey = Key("fears-confirmed");
    public static readonly string ReliefKey = Key("relief");
    public static readonly string DisappointmentKey = Key("disappointment");
    public static readonly string PrideKey = Key("pride");
    public static readonly string ShameKey = Key("shame");
    public static readonly string AdmirationKey = Key("admiration");
    public static readonly string ReproachKey = Key("reproach");

    public static readonly IReadOnlyList<string> AllKeys = new[]
    {
        JoyKey, DistressKey, HopeKey, FearKey, SatisfactionKey, FearsConfirmedKey,
        ReliefKey, DisappointmentKey, PrideKey, ShameKey, AdmirationKey, ReproachKey
    };

    private static readonly Dictionary<string, string> KindToKey =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            [JoyKind] = JoyKey,
            [DistressKind] = DistressKey,
            [HopeKind] = HopeKey,
            [FearKind] = FearKey,
            [SatisfactionKind] = SatisfactionKey,
            [FearsConfirmedKind] = FearsConfirmedKey,
            [ReliefKind] = ReliefKey,
            [DisappointmentKind] = DisappointmentKey,
            [PrideKind] = PrideKey,
            [ShameKind] = ShameKey,
            [AdmirationKind] = AdmirationKey,
            [ReproachKind] = ReproachKey
        };

    private readonly Dictionary<string, float> _intensity =
        new Dictionary<string, float>(StringComparer.Ordinal);

    /// <summary>Exponential decay rate in 1/s toward 0. Project convention.</summary>
    public float DecayRate { get; init; } = 1.5f;

    public string Id => ProviderId;
    public string Layer => AffectLayer.Emotion;
    public Citation Citation { get; } = OccCitations.Occ1988;
    public IReadOnlyList<Citation> AdditionalCitations { get; } =
        new[] { OccCitations.GebhardAlma2005, OccCitations.Dynamics };

    public AffectDelta Contribute(WorldEvent ev, float deltaTime, AffectSnapshot snapshot)
    {
        if (KindToKey.TryGetValue(ev.Kind, out var key))
            _intensity[key] = Clamp01(_intensity.GetValueOrDefault(key) + ev.Intensity);

        if (deltaTime > 0f && _intensity.Count > 0)
        {
            var keep = MathF.Exp(-DecayRate * deltaTime);
            var keys = new List<string>(_intensity.Keys);
            var dropped = new List<string>();
            foreach (var k in keys)
            {
                var next = _intensity[k] * keep;
                if (next < 0.001f)
                {
                    _intensity.Remove(k);
                    dropped.Add(k);
                }
                else
                    _intensity[k] = next;
            }

            if (_intensity.Count == 0 && dropped.Count == 0)
                return new AffectDelta();

            var decayed = new AffectDelta();
            foreach (var pair in _intensity)
                decayed.Set(pair.Key, pair.Value);
            foreach (var k in dropped)
                decayed.Set(k, 0f);
            return decayed;
        }

        if (_intensity.Count == 0)
            return new AffectDelta();

        var delta = new AffectDelta();
        foreach (var pair in _intensity)
            delta.Set(pair.Key, pair.Value);
        return delta;
    }

    public IReadOnlyDictionary<string, float> ExportState()
    {
        var bag = new Dictionary<string, float>(StringComparer.Ordinal);
        foreach (var pair in _intensity)
            bag[pair.Key] = pair.Value;
        return bag;
    }

    public void ImportState(IReadOnlyDictionary<string, float> bag)
    {
        _intensity.Clear();
        foreach (var pair in bag)
        {
            if (!IsOccChannel(pair.Key))
                continue;
            if (pair.Value < 0.001f)
                continue;
            _intensity[pair.Key] = Clamp01(pair.Value);
        }
    }

    public static string Key(string occType) =>
        ChannelKey.Of(AffectLayer.Emotion, ProviderId, occType);

    private static bool IsOccChannel(string key)
    {
        foreach (var known in AllKeys)
        {
            if (known == key)
                return true;
        }

        return false;
    }

    private static float Clamp01(float value) =>
        value < 0f ? 0f : value > 1f ? 1f : value;
}
