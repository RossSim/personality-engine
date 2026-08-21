using System;
using System.Collections.Generic;

namespace PersonalityEngine.Providers.Skinner;

/// <summary>
/// Operant strengths under a three-term contingency (SD, response, consequence).
/// Skinner (1953); schedules after Ferster &amp; Skinner (1957).
/// </summary>
public sealed class OperantLearningProvider : IAffectProvider, IStatefulProvider
{
    public const string ProviderId = "skinner-operant";

    public const string EmitKind = "skinner.emit";
    public const string ReinforceKind = "skinner.reinforce";
    public const string PunishKind = "skinner.punish";
    public const string ExtinguishKind = "skinner.extinguish";
    public const string DiscriminativeStimulusKind = "skinner.sd";
    public const string EstablishingOperationKind = "skinner.eo";

    public static readonly string SdKey = ChannelKey.Of(AffectLayer.Learning, ProviderId, "sd");
    public static readonly string DeprivationKey = ChannelKey.Of(AffectLayer.Learning, ProviderId, "deprivation");

    public static string StrengthKey(string actionId) =>
        ChannelKey.Of(AffectLayer.Learning, ProviderId, "strength:" + actionId);

    internal const string HasSdKey = "has-sd";
    internal const string RatioPrefix = "ratio:";
    internal const string NextVrPrefix = "next-vr:";

    private readonly Dictionary<string, float> _strength = new Dictionary<string, float>(StringComparer.Ordinal);
    private readonly Dictionary<string, int> _responsesSinceReinforcer = new Dictionary<string, int>(StringComparer.Ordinal);
    private readonly Dictionary<string, int> _nextVr = new Dictionary<string, int>(StringComparer.Ordinal);
    private readonly Random _random;
    private float? _sd;
    private float _deprivation = 0.5f;

    public OperantLearningProvider(
        IEnumerable<string>? actionIds = null,
        ReinforcementSchedule schedule = ReinforcementSchedule.Continuous,
        int? randomSeed = null)
    {
        Schedule = schedule;
        _random = randomSeed.HasValue ? new Random(randomSeed.Value) : new Random(1);
        if (actionIds != null)
        {
            foreach (var id in actionIds)
                _strength[id] = OperantLevel;
        }
    }

    public string Id => ProviderId;
    public string Layer => AffectLayer.Learning;
    public ReinforcementSchedule Schedule { get; }
    public int FixedRatio { get; init; } = 5;
    public int VariableRatioMean { get; init; } = 5;
    public float OperantLevel { get; init; } = 0.15f;
    public float ReinforceGain { get; init; } = 0.20f;
    public float PunishLoss { get; init; } = 0.25f;
    public float ExtinctionLoss { get; init; } = 0.08f;

    public Citation Citation { get; } = SkinnerCitations.ScienceAndHumanBehavior1953;

    public IReadOnlyList<Citation> AdditionalCitations { get; } =
        new[]
        {
            SkinnerCitations.BehaviorOfOrganisms1938,
            SkinnerCitations.SchedulesOfReinforcement1957,
            SkinnerCitations.BeyondFreedomAndDignity1971,
            SkinnerCitations.StrengthDynamics
        };

    public AffectDelta Contribute(WorldEvent ev, float deltaTime, AffectSnapshot snapshot)
    {
        switch (ev.Kind)
        {
            case EmitKind when ev.Target != null:
                HandleEmit(ev.Target);
                break;
            case ReinforceKind when ev.Target != null:
                Strengthen(ev.Target, ev.Intensity * ReinforceGain);
                ResetRatio(ev.Target);
                break;
            case PunishKind when ev.Target != null:
                Strengthen(ev.Target, -ev.Intensity * PunishLoss);
                break;
            case ExtinguishKind when ev.Target != null:
                Strengthen(ev.Target, -ev.Intensity * ExtinctionLoss);
                break;
            case DiscriminativeStimulusKind:
                _sd = ev.Intensity;
                break;
            case EstablishingOperationKind:
                _deprivation = ev.Intensity;
                break;
        }

        var delta = new AffectDelta()
            .Set(DeprivationKey, _deprivation);
        if (_sd.HasValue)
            delta.Set(SdKey, _sd.Value);

        foreach (var pair in _strength)
            delta.Set(StrengthKey(pair.Key), pair.Value);

        return delta;
    }

    public IReadOnlyDictionary<string, float> ExportState()
    {
        var bag = new Dictionary<string, float>(StringComparer.Ordinal)
        {
            [DeprivationKey] = _deprivation
        };
        if (_sd.HasValue)
        {
            bag[HasSdKey] = 1f;
            bag[SdKey] = _sd.Value;
        }

        foreach (var pair in _strength)
            bag[StrengthKey(pair.Key)] = pair.Value;
        foreach (var pair in _responsesSinceReinforcer)
            bag[RatioPrefix + pair.Key] = pair.Value;
        foreach (var pair in _nextVr)
            bag[NextVrPrefix + pair.Key] = pair.Value;

        return bag;
    }

    public void ImportState(IReadOnlyDictionary<string, float> bag)
    {
        _strength.Clear();
        _responsesSinceReinforcer.Clear();
        _nextVr.Clear();
        _sd = null;

        if (bag.TryGetValue(DeprivationKey, out var deprivation))
            _deprivation = Clamp01(deprivation);

        if (bag.TryGetValue(HasSdKey, out var hasSd) && hasSd >= 0.5f && bag.TryGetValue(SdKey, out var sd))
            _sd = Clamp01(sd);
        else if (bag.TryGetValue(SdKey, out sd) && !bag.ContainsKey(HasSdKey))
            _sd = Clamp01(sd);

        var strengthPrefix = StrengthKey("");
        foreach (var pair in bag)
        {
            if (pair.Key.StartsWith(strengthPrefix, StringComparison.Ordinal) && pair.Key.Length > strengthPrefix.Length)
                _strength[pair.Key.Substring(strengthPrefix.Length)] = Clamp01(pair.Value);
            else if (pair.Key.StartsWith(RatioPrefix, StringComparison.Ordinal) && pair.Key.Length > RatioPrefix.Length)
                _responsesSinceReinforcer[pair.Key.Substring(RatioPrefix.Length)] = (int)pair.Value;
            else if (pair.Key.StartsWith(NextVrPrefix, StringComparison.Ordinal) && pair.Key.Length > NextVrPrefix.Length)
                _nextVr[pair.Key.Substring(NextVrPrefix.Length)] = Math.Max(1, (int)pair.Value);
        }
    }

    private void HandleEmit(string actionId)
    {
        Ensure(actionId);
        if (ScheduleDelivers(actionId))
        {
            Strengthen(actionId, ReinforceGain);
            ResetRatio(actionId);
        }
        else
        {
            Strengthen(actionId, -ExtinctionLoss);
        }
    }

    private bool ScheduleDelivers(string actionId)
    {
        switch (Schedule)
        {
            case ReinforcementSchedule.Continuous:
                return true;
            case ReinforcementSchedule.FixedRatio:
            {
                var n = _responsesSinceReinforcer.GetValueOrDefault(actionId) + 1;
                _responsesSinceReinforcer[actionId] = n;
                return n >= Math.Max(1, FixedRatio);
            }
            case ReinforcementSchedule.VariableRatio:
            {
                var n = _responsesSinceReinforcer.GetValueOrDefault(actionId) + 1;
                _responsesSinceReinforcer[actionId] = n;
                if (!_nextVr.TryGetValue(actionId, out var need))
                {
                    need = NextVariableRatio();
                    _nextVr[actionId] = need;
                }

                return n >= need;
            }
            default:
                return false;
        }
    }

    private void ResetRatio(string actionId)
    {
        _responsesSinceReinforcer[actionId] = 0;
        if (Schedule == ReinforcementSchedule.VariableRatio)
            _nextVr[actionId] = NextVariableRatio();
    }

    private int NextVariableRatio()
    {
        var mean = Math.Max(1, VariableRatioMean);
        var draw = _random.Next(1, mean * 2);
        return Math.Max(1, draw);
    }

    private void Ensure(string actionId)
    {
        if (!_strength.ContainsKey(actionId))
            _strength[actionId] = OperantLevel;
    }

    private void Strengthen(string actionId, float delta)
    {
        Ensure(actionId);
        _strength[actionId] = Clamp01(_strength[actionId] + delta);
    }

    private static float Clamp01(float value) =>
        value < 0f ? 0f : value > 1f ? 1f : value;
}
