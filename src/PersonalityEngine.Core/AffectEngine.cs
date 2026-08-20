using System.Collections.Generic;

namespace PersonalityEngine;

/// <summary>Composition root: runs providers in order, then optional weighters.</summary>
public sealed class AffectEngine
{
    private readonly IAffectProvider[] _providers;
    private readonly IActionWeighter[] _weighters;
    private readonly AffectSnapshot _snapshot = new AffectSnapshot();

    public AffectEngine(
        IEnumerable<IAffectProvider> providers,
        IEnumerable<IActionWeighter>? weighters = null)
    {
        _providers = new List<IAffectProvider>(providers).ToArray();
        _weighters = weighters is null
            ? System.Array.Empty<IActionWeighter>()
            : new List<IActionWeighter>(weighters).ToArray();
    }

    public AffectSnapshot Snapshot => _snapshot;

    public AffectSnapshot Tick(WorldEvent ev, float deltaTime = 0f)
    {
        foreach (var provider in _providers)
        {
            var delta = provider.Contribute(ev, deltaTime, _snapshot);
            _snapshot.Apply(delta, provider.Id);
        }

        return _snapshot;
    }

    public IReadOnlyDictionary<string, float> WeightActions(IReadOnlyList<string> actionIds)
    {
        var combined = new Dictionary<string, float>();
        foreach (var weighter in _weighters)
        {
            foreach (var pair in weighter.Weight(_snapshot, actionIds))
            {
                combined.TryGetValue(pair.Key, out var current);
                combined[pair.Key] = current + pair.Value;
            }
        }

        return combined;
    }
}
