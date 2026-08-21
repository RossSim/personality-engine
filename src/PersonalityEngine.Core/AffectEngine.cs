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

    /// <summary>Idle tick: decay without a host event. Same as <c>Tick(WorldEvent.Tick, deltaTime)</c>.</summary>
    public AffectSnapshot Tick(float deltaTime) => Tick(WorldEvent.Tick, deltaTime);

    public AffectSnapshot Tick(WorldEvent ev, float deltaTime = 0f)
    {
        foreach (var provider in _providers)
        {
            var delta = provider.Contribute(ev, deltaTime, _snapshot);
            _snapshot.Apply(delta, provider.Id);
        }

        return _snapshot;
    }

    /// <summary>Snapshot channels plus stateful provider bags. Unknown provider ids are omitted.</summary>
    public AffectPersist Export()
    {
        var persist = new AffectPersist { Version = AffectPersist.CurrentVersion };
        foreach (var pair in _snapshot.Channels)
            persist.Channels[pair.Key] = pair.Value;

        foreach (var provider in _providers)
        {
            if (provider is not IStatefulProvider stateful)
                continue;
            var bag = stateful.ExportState();
            if (bag.Count == 0)
                continue;
            var copy = new Dictionary<string, float>();
            foreach (var pair in bag)
                copy[pair.Key] = pair.Value;
            persist.Providers[provider.Id] = copy;
        }

        return persist;
    }

    /// <summary>
    /// Restore onto this engine. Rebuild the same composition (same traits, same providers) first.
    /// Unknown provider ids and unknown bag keys are ignored.
    /// </summary>
    public void Import(AffectPersist persist)
    {
        if (persist is null)
            throw new System.ArgumentNullException(nameof(persist));

        var channels = persist.Channels ?? new Dictionary<string, float>();
        _snapshot.Replace(channels);

        var bags = persist.Providers ?? new Dictionary<string, Dictionary<string, float>>();
        foreach (var provider in _providers)
        {
            if (provider is not IStatefulProvider stateful)
                continue;
            if (!bags.TryGetValue(provider.Id, out var bag) || bag is null)
                continue;
            stateful.ImportState(bag);
        }
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
