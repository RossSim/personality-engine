using PersonalityEngine.Providers.Erikson;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Peterson;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class EriksonPsychosocialTests
{
    [Fact]
    public void Industry_Starts_WithoutIdentityCrisisOrGenerativity()
    {
        var engine = EriksonComposition.Create(PsychosocialStage.IndustryVsInferiority);
        var snapshot = engine.Tick(WorldEvent.Tick);

        Assert.Equal(0f, snapshot.GetOrDefault(EriksonPsychosocialProvider.IdentityCrisisKey));
        Assert.Equal(0f, snapshot.GetOrDefault(EriksonPsychosocialProvider.GenerativityKey));
        Assert.Equal(0f, snapshot.GetOrDefault(EriksonPsychosocialProvider.IntegrityKey));
        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.MoratoriumKey) < 0.25f);
        Assert.False(snapshot.TryGet("personality.ocean.openness", out _));
    }

    [Fact]
    public void IdentityStage_MarksCrisis_AndRaisesMoratorium()
    {
        var industry = EriksonComposition.Create(PsychosocialStage.IndustryVsInferiority);
        industry.Tick(WorldEvent.Tick);
        var identity = EriksonComposition.Create(PsychosocialStage.IdentityVsRoleConfusion);
        var snapshot = identity.Tick(WorldEvent.Tick);

        Assert.Equal(1f, snapshot.GetOrDefault(EriksonPsychosocialProvider.IdentityCrisisKey));
        Assert.Equal(0f, snapshot.GetOrDefault(EriksonPsychosocialProvider.GenerativityKey));
        Assert.True(
            snapshot.GetOrDefault(EriksonPsychosocialProvider.MoratoriumKey) >
            industry.Snapshot.GetOrDefault(EriksonPsychosocialProvider.MoratoriumKey));
        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.RoleConfusionKey) > 0.35f);
    }

    [Fact]
    public void GenerativityStage_UnlocksGenerativity_ButNotIntegrity()
    {
        var engine = EriksonComposition.Create(PsychosocialStage.GenerativityVsStagnation);
        var snapshot = engine.Tick(WorldEvent.Tick);

        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.GenerativityKey) > 0.3f);
        Assert.Equal(0f, snapshot.GetOrDefault(EriksonPsychosocialProvider.IntegrityKey));
        Assert.Equal(0f, snapshot.GetOrDefault(EriksonPsychosocialProvider.IdentityCrisisKey));
    }

    [Fact]
    public void IntegrityStage_UnlocksIntegrity()
    {
        var engine = EriksonComposition.Create(PsychosocialStage.IntegrityVsDespair);
        var snapshot = engine.Tick(WorldEvent.Tick);

        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.IntegrityKey) > 0.3f);
        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.GenerativityKey) > 0.3f);
    }

    [Fact]
    public void Support_RaisesSyntonicRatio()
    {
        var engine = EriksonComposition.Create(PsychosocialStage.TrustVsMistrust);
        engine.Tick(WorldEvent.Tick);
        var before = engine.Snapshot.GetOrDefault(EriksonPsychosocialProvider.RatioKey);
        var snapshot = engine.Tick(new WorldEvent(EriksonPsychosocialProvider.SupportKind, 1f));

        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.SyntonicKey) > 0.7f);
        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.RatioKey) > before);
        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.DystonicKey) < 0.4f);
    }

    [Fact]
    public void Rupture_RaisesDystonic_AndRoleConfusion()
    {
        var engine = EriksonComposition.Create(PsychosocialStage.IdentityVsRoleConfusion);
        engine.Tick(WorldEvent.Tick);
        var snapshot = engine.Tick(new WorldEvent(EriksonPsychosocialProvider.RuptureKind, 1f));

        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.DystonicKey) > 0.7f);
        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.SyntonicKey) < 0.5f);
        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.RoleConfusionKey) > 0.6f);
    }

    [Fact]
    public void Commit_RaisesEgoIdentity_AndLowersMoratorium()
    {
        var engine = EriksonComposition.Create(PsychosocialStage.IdentityVsRoleConfusion);
        engine.Tick(WorldEvent.Tick);
        var beforeMoratorium = engine.Snapshot.GetOrDefault(EriksonPsychosocialProvider.MoratoriumKey);
        var snapshot = engine.Tick(new WorldEvent(EriksonPsychosocialProvider.CommitKind, 1f));

        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.EgoIdentityKey) > 0.7f);
        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.FidelityKey) > 0.6f);
        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.MoratoriumKey) < beforeMoratorium);
    }

    [Fact]
    public void NegativeIdentity_IsAFormOfIdentity_NotMereConfusion()
    {
        var engine = EriksonComposition.Create(PsychosocialStage.IdentityVsRoleConfusion);
        engine.Tick(WorldEvent.Tick);
        var beforeIdentity = engine.Snapshot.GetOrDefault(EriksonPsychosocialProvider.EgoIdentityKey);
        var snapshot = engine.Tick(new WorldEvent(EriksonPsychosocialProvider.NegativeIdentityKind, 1f));

        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.NegativeIdentityKey) > 0.5f);
        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.EgoIdentityKey) > beforeIdentity);
        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.FidelityKey) < 0.3f);
    }

    [Fact]
    public void Generate_DoesNothingBeforeGenerativityStage()
    {
        var engine = EriksonComposition.Create(PsychosocialStage.IndustryVsInferiority);
        var snapshot = engine.Tick(new WorldEvent(EriksonPsychosocialProvider.GenerateKind, 1f));
        Assert.Equal(0f, snapshot.GetOrDefault(EriksonPsychosocialProvider.GenerativityKey));
    }

    [Fact]
    public void Generate_RaisesGenerativity_WhenStageAllows()
    {
        var engine = EriksonComposition.Create(PsychosocialStage.GenerativityVsStagnation);
        engine.Tick(WorldEvent.Tick);
        var before = engine.Snapshot.GetOrDefault(EriksonPsychosocialProvider.GenerativityKey);
        var snapshot = engine.Tick(new WorldEvent(EriksonPsychosocialProvider.GenerateKind, 1f));
        Assert.True(snapshot.GetOrDefault(EriksonPsychosocialProvider.GenerativityKey) > before);
    }

    [Fact]
    public void WorksWithoutOceanOrPeterson()
    {
        var engine = EriksonComposition.Create(PsychosocialStage.IntimacyVsIsolation);
        var snap = engine.Tick(new WorldEvent(EriksonPsychosocialProvider.ChallengeKind, 0.5f));
        Assert.False(snap.TryGet("personality.ocean.openness", out _));
        Assert.True(snap.TryGet(EriksonPsychosocialProvider.EgoIdentityKey, out _));
    }

    [Fact]
    public void ComposesWithPetersonWithoutReplacingMeaning()
    {
        var engine = EriksonComposition.CreateWithOceanAndPeterson(
            OceanTraits.GebhardExample,
            PsychosocialStage.IdentityVsRoleConfusion);

        engine.Tick(WorldEvent.Tick);
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, 0.9f));
        engine.Tick(new WorldEvent(EriksonPsychosocialProvider.ChallengeKind, 0.9f));

        Assert.True(engine.Snapshot.GetOrDefault(OrderChaosMeaningProvider.ChaosKey) > 0.5f);
        Assert.True(engine.Snapshot.GetOrDefault(EriksonPsychosocialProvider.DystonicKey) > 0.5f);
        Assert.True(engine.Snapshot.GetOrDefault(StabilityPlasticityProvider.StabilityKey) > 0f);
    }

    [Fact]
    public void Weighter_PrefersExplore_DuringMoratorium()
    {
        var engine = EriksonComposition.Create(PsychosocialStage.IdentityVsRoleConfusion);
        engine.Tick(new WorldEvent(EriksonPsychosocialProvider.ExploreKind, 1f));
        var weights = engine.WeightActions(new[]
        {
            EriksonIdentityWeighter.Explore,
            EriksonIdentityWeighter.Commit
        });

        weights.TryGetValue(EriksonIdentityWeighter.Commit, out var commit);
        Assert.True(weights[EriksonIdentityWeighter.Explore] > commit);
    }

    [Fact]
    public void Weighter_PrefersCommit_AfterIdentityChoice()
    {
        var engine = EriksonComposition.Create(PsychosocialStage.IdentityVsRoleConfusion);
        engine.Tick(new WorldEvent(EriksonPsychosocialProvider.CommitKind, 1f));
        var weights = engine.WeightActions(new[]
        {
            EriksonIdentityWeighter.Explore,
            EriksonIdentityWeighter.Commit
        });

        weights.TryGetValue(EriksonIdentityWeighter.Explore, out var explore);
        Assert.True(weights[EriksonIdentityWeighter.Commit] > explore);
    }
}
