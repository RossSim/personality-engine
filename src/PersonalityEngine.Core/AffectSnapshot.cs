using System.Collections.Generic;

namespace PersonalityEngine;

/// <summary>Named-channel snapshot. Missing keys are absent, not errors.</summary>
public sealed class AffectSnapshot
{
    private readonly Dictionary<string, float> _channels = new Dictionary<string, float>();
    private readonly List<string> _ran = new List<string>();

    public IReadOnlyDictionary<string, float> Channels => _channels;
    public IReadOnlyList<string> ProvidersRan => _ran;

    public bool TryGet(string key, out float value) => _channels.TryGetValue(key, out value);

    public float GetOrDefault(string key, float fallback = 0f) =>
        _channels.TryGetValue(key, out var value) ? value : fallback;

    internal void Apply(AffectDelta delta, string providerId)
    {
        foreach (var pair in delta.Values)
            _channels[pair.Key] = pair.Value;
        _ran.Add(providerId);
    }
}
