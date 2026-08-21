using System;
using System.Collections.Generic;

namespace PersonalityEngine.Providers.Dyad;

/// <summary>
/// Pairwise liking toward a named other. OCC treats liking as an attitude
/// that fortune-of-others assumes. This provider stores that attitude;
/// it does not fire OCC types.
/// </summary>
public sealed class DyadProvider : IAffectProvider, IStatefulProvider
{
    public const string ProviderId = "dyad";
    public const string LikeKind = "dyad.like";
    public const string DislikeKind = "dyad.dislike";

    private readonly Dictionary<string, float> _liking =
        new Dictionary<string, float>(StringComparer.Ordinal);

    /// <summary>Exponential decay rate in 1/s toward 0. Project convention.</summary>
    public float DecayRate { get; init; } = 0.05f;

    public string Id => ProviderId;
    public string Layer => AffectLayer.Relationship;
    public Citation Citation { get; } = DyadCitations.OccAttitude;
    public IReadOnlyList<Citation> AdditionalCitations { get; } =
        new[] { DyadCitations.Dynamics };

    public static string LikingKey(string otherId) =>
        ChannelKey.Of(AffectLayer.Relationship, ProviderId, "liking:" + otherId);

    public AffectDelta Contribute(WorldEvent ev, float deltaTime, AffectSnapshot snapshot)
    {
        if (ev.Target != null)
        {
            if (ev.Kind == LikeKind)
                Bump(ev.Target, ev.Intensity);
            else if (ev.Kind == DislikeKind)
                Bump(ev.Target, -ev.Intensity);
        }

        if (deltaTime > 0f && _liking.Count > 0)
        {
            var keep = MathF.Exp(-DecayRate * deltaTime);
            var keys = new List<string>(_liking.Keys);
            var dropped = new List<string>();
            foreach (var other in keys)
            {
                var next = _liking[other] * keep;
                if (MathF.Abs(next) < 0.001f)
                {
                    _liking.Remove(other);
                    dropped.Add(other);
                }
                else
                    _liking[other] = next;
            }

            if (_liking.Count == 0 && dropped.Count == 0)
                return new AffectDelta();

            var decayed = new AffectDelta();
            foreach (var pair in _liking)
                decayed.Set(LikingKey(pair.Key), pair.Value);
            foreach (var other in dropped)
                decayed.Set(LikingKey(other), 0f);
            return decayed;
        }

        if (_liking.Count == 0)
            return new AffectDelta();

        var delta = new AffectDelta();
        foreach (var pair in _liking)
            delta.Set(LikingKey(pair.Key), pair.Value);
        return delta;
    }

    public IReadOnlyDictionary<string, float> ExportState()
    {
        var bag = new Dictionary<string, float>(StringComparer.Ordinal);
        foreach (var pair in _liking)
            bag[LikingKey(pair.Key)] = pair.Value;
        return bag;
    }

    public void ImportState(IReadOnlyDictionary<string, float> bag)
    {
        _liking.Clear();
        var prefix = LikingKey("");
        foreach (var pair in bag)
        {
            if (float.IsNaN(pair.Value) || float.IsInfinity(pair.Value))
                continue;
            if (!pair.Key.StartsWith(prefix, StringComparison.Ordinal) || pair.Key.Length <= prefix.Length)
                continue;
            var other = pair.Key.Substring(prefix.Length);
            if (other.Length == 0)
                continue;
            var value = Clamp(pair.Value);
            if (MathF.Abs(value) < 0.001f)
                continue;
            _liking[other] = value;
        }
    }

    private void Bump(string otherId, float delta)
    {
        if (otherId.Length == 0)
            return;
        _liking.TryGetValue(otherId, out var current);
        var next = Clamp(current + delta);
        if (MathF.Abs(next) < 0.001f)
            _liking.Remove(otherId);
        else
            _liking[otherId] = next;
    }

    private static float Clamp(float value) =>
        value < -1f ? -1f : value > 1f ? 1f : value;
}
