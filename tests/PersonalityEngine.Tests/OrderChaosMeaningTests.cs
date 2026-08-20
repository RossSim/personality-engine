using PersonalityEngine;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Peterson;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class OrderChaosMeaningTests
{
    [Fact]
    public void Anomaly_RaisesChaos_AndLowersOrder()
    {
        var engine = new AffectEngine(new IAffectProvider[] { new OrderChaosMeaningProvider() });
        var before = engine.Tick(WorldEvent.Tick);
        var order0 = before.GetOrDefault(OrderChaosMeaningProvider.OrderKey);
        var chaos0 = before.GetOrDefault(OrderChaosMeaningProvider.ChaosKey);

        var after = engine.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, 1f));
        Assert.True(after.GetOrDefault(OrderChaosMeaningProvider.ChaosKey) > chaos0);
        Assert.True(after.GetOrDefault(OrderChaosMeaningProvider.OrderKey) < order0);
        Assert.True(after.GetOrDefault(OrderChaosMeaningProvider.ChaosMeaningKey) >
                    after.GetOrDefault(OrderChaosMeaningProvider.KnownMeaningKey) * 0f);
    }

    [Fact]
    public void HighPlasticity_Anomaly_RaisesLogosMoreThanLowPlasticity()
    {
        var explorer = RunAnomaly(new OceanTraits(0.9f, 0.4f, 0.9f, 0.4f, 0.3f));
        var rigid = RunAnomaly(new OceanTraits(0.2f, 0.9f, 0.2f, 0.8f, 0.2f));
        Assert.True(explorer.Logos > rigid.Logos);
        Assert.True(rigid.Rigidity > explorer.Rigidity);
    }

    [Fact]
    public void Integrate_RestoresOrder()
    {
        var engine = new AffectEngine(new IAffectProvider[] { new OrderChaosMeaningProvider() });
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, 1f));
        var chaosAfterAnomaly = engine.Snapshot.GetOrDefault(OrderChaosMeaningProvider.ChaosKey);
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.IntegrateKind, 1f));
        Assert.True(engine.Snapshot.GetOrDefault(OrderChaosMeaningProvider.ChaosKey) < chaosAfterAnomaly);
        Assert.True(engine.Snapshot.GetOrDefault(OrderChaosMeaningProvider.OrderKey) >
                    engine.Snapshot.GetOrDefault(OrderChaosMeaningProvider.ChaosKey));
    }

    [Fact]
    public void DefendBelief_RaisesRigidity()
    {
        var engine = new AffectEngine(new IAffectProvider[] { new OrderChaosMeaningProvider() });
        engine.Tick(WorldEvent.Tick);
        var r0 = engine.Snapshot.GetOrDefault(OrderChaosMeaningProvider.RigidityKey);
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.DefendBeliefKind, 1f));
        Assert.True(engine.Snapshot.GetOrDefault(OrderChaosMeaningProvider.RigidityKey) > r0);
    }

    [Fact]
    public void Weighter_PrefersExplore_WhenLogosAndChaosHigh()
    {
        var engine = PetersonComposition.Create(new OceanTraits(0.9f, 0.4f, 0.9f, 0.4f, 0.3f));
        engine.Tick(WorldEvent.Tick);
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, 1f));
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.VoluntaryExploreKind, 1f));
        var weights = engine.WeightActions(new[]
        {
            PetersonMeaningWeighter.Explore,
            PetersonMeaningWeighter.Defend,
            PetersonMeaningWeighter.Withdraw
        });
        Assert.True(weights[PetersonMeaningWeighter.Explore] > weights[PetersonMeaningWeighter.Defend]);
    }

    [Fact]
    public void Weighter_PrefersDefend_WhenStabilityDominatesAnomaly()
    {
        var engine = PetersonComposition.Create(new OceanTraits(0.15f, 0.9f, 0.15f, 0.85f, 0.15f));
        engine.Tick(WorldEvent.Tick);
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, 1f));
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.DefendBeliefKind, 0.8f));
        var weights = engine.WeightActions(new[]
        {
            PetersonMeaningWeighter.Explore,
            PetersonMeaningWeighter.Defend
        });
        Assert.True(weights[PetersonMeaningWeighter.Defend] > weights[PetersonMeaningWeighter.Explore]);
    }

    [Fact]
    public void VoluntaryExplore_RaisesExplorationMeaning()
    {
        var engine = new AffectEngine(new IAffectProvider[] { new OrderChaosMeaningProvider() });
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, 1f));
        var before = engine.Snapshot.GetOrDefault(OrderChaosMeaningProvider.ExploreMeaningKey);
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.VoluntaryExploreKind, 1f));
        Assert.True(engine.Snapshot.GetOrDefault(OrderChaosMeaningProvider.ExploreMeaningKey) > before);
    }

    private static (float Logos, float Rigidity) RunAnomaly(OceanTraits traits)
    {
        var engine = PetersonComposition.Create(traits);
        engine.Tick(WorldEvent.Tick);
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, 1f));
        return (
            engine.Snapshot.GetOrDefault(OrderChaosMeaningProvider.LogosKey),
            engine.Snapshot.GetOrDefault(OrderChaosMeaningProvider.RigidityKey));
    }
}
