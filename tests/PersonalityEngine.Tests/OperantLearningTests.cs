using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Peterson;
using PersonalityEngine.Providers.Skinner;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class OperantLearningTests
{
    private const string Peck = "peck";

    [Fact]
    public void Continuous_EveryEmit_RaisesStrength()
    {
        var engine = SkinnerComposition.Create(new[] { Peck });
        engine.Tick(WorldEvent.Tick);
        var start = engine.Snapshot.GetOrDefault(OperantLearningProvider.StrengthKey(Peck));
        engine.Tick(new WorldEvent(OperantLearningProvider.EmitKind, 1f, Peck));
        Assert.True(engine.Snapshot.GetOrDefault(OperantLearningProvider.StrengthKey(Peck)) > start);
    }

    [Fact]
    public void Punish_LowersStrength()
    {
        var engine = SkinnerComposition.Create(new[] { Peck });
        engine.Tick(new WorldEvent(OperantLearningProvider.ReinforceKind, 1f, Peck));
        var afterR = engine.Snapshot.GetOrDefault(OperantLearningProvider.StrengthKey(Peck));
        engine.Tick(new WorldEvent(OperantLearningProvider.PunishKind, 1f, Peck));
        Assert.True(engine.Snapshot.GetOrDefault(OperantLearningProvider.StrengthKey(Peck)) < afterR);
    }

    [Fact]
    public void Extinguish_LowersStrength()
    {
        var engine = SkinnerComposition.Create(new[] { Peck });
        engine.Tick(new WorldEvent(OperantLearningProvider.ReinforceKind, 1f, Peck));
        var afterR = engine.Snapshot.GetOrDefault(OperantLearningProvider.StrengthKey(Peck));
        engine.Tick(new WorldEvent(OperantLearningProvider.ExtinguishKind, 1f, Peck));
        Assert.True(engine.Snapshot.GetOrDefault(OperantLearningProvider.StrengthKey(Peck)) < afterR);
    }

    [Fact]
    public void FixedRatio5_DoesNotStrengthen_UntilFifthEmit()
    {
        var engine = new AffectEngine(
            new IAffectProvider[]
            {
                new OperantLearningProvider(new[] { Peck }, ReinforcementSchedule.FixedRatio, 1)
                {
                    FixedRatio = 5,
                    ExtinctionLoss = 0f
                }
            });
        engine.Tick(WorldEvent.Tick);
        var start = engine.Snapshot.GetOrDefault(OperantLearningProvider.StrengthKey(Peck));
        for (var i = 0; i < 4; i++)
            engine.Tick(new WorldEvent(OperantLearningProvider.EmitKind, 1f, Peck));
        Assert.Equal(start, engine.Snapshot.GetOrDefault(OperantLearningProvider.StrengthKey(Peck)));
        engine.Tick(new WorldEvent(OperantLearningProvider.EmitKind, 1f, Peck));
        Assert.True(engine.Snapshot.GetOrDefault(OperantLearningProvider.StrengthKey(Peck)) > start);
    }

    [Fact]
    public void Weighter_Prefers_ReinforcedAction()
    {
        var engine = SkinnerComposition.Create(new[] { "forage", "idle" });
        engine.Tick(WorldEvent.Tick);
        engine.Tick(new WorldEvent(OperantLearningProvider.ReinforceKind, 1f, "forage"));
        engine.Tick(new WorldEvent(OperantLearningProvider.PunishKind, 1f, "idle"));
        var weights = engine.WeightActions(new[] { "forage", "idle" });
        weights.TryGetValue("idle", out var idle);
        Assert.True(weights["forage"] > idle);
    }

    [Fact]
    public void LowSd_ReducesWeights_WhenSpecified()
    {
        var engine = SkinnerComposition.Create(new[] { Peck });
        engine.Tick(new WorldEvent(OperantLearningProvider.ReinforceKind, 1f, Peck));
        var withSd = engine.WeightActions(new[] { Peck })[Peck];
        engine.Tick(new WorldEvent(OperantLearningProvider.DiscriminativeStimulusKind, 0f));
        var withoutSd = engine.WeightActions(new[] { Peck })[Peck];
        Assert.True(withoutSd < withSd);
    }

    [Fact]
    public void WorksWithoutOceanOrPeterson()
    {
        var engine = SkinnerComposition.Create(new[] { Peck });
        var snap = engine.Tick(new WorldEvent(OperantLearningProvider.EmitKind, 1f, Peck));
        Assert.False(snap.TryGet("personality.ocean.openness", out _));
        Assert.True(snap.TryGet(OperantLearningProvider.StrengthKey(Peck), out _));
    }

    [Fact]
    public void ComposesWithPetersonWithoutReplacingMeaning()
    {
        var engine = SkinnerComposition.CreateWithOceanAndPeterson(
            OceanTraits.GebhardExample,
            new[] { PetersonMeaningWeighter.Explore, Peck });
        engine.Tick(WorldEvent.Tick);
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, 1f));
        engine.Tick(new WorldEvent(OperantLearningProvider.ReinforceKind, 1f, Peck));
        Assert.True(engine.Snapshot.TryGet(OrderChaosMeaningProvider.ChaosKey, out _));
        Assert.True(engine.Snapshot.TryGet(StabilityPlasticityProvider.StabilityKey, out _));
        Assert.True(engine.Snapshot.GetOrDefault(OperantLearningProvider.StrengthKey(Peck)) > 0.15f);
    }
}
