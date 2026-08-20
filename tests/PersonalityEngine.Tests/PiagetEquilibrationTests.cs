using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Peterson;
using PersonalityEngine.Providers.Piaget;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class PiagetEquilibrationTests
{
    [Fact]
    public void Sensorimotor_Starts_WithoutObjectPermanenceOrConservation()
    {
        var engine = PiagetComposition.Create(CognitiveStage.Sensorimotor);
        var snapshot = engine.Tick(WorldEvent.Tick);

        Assert.Equal(0f, snapshot.GetOrDefault(PiagetEquilibrationProvider.ObjectPermanenceKey));
        Assert.Equal(0f, snapshot.GetOrDefault(PiagetEquilibrationProvider.ConservationKey));
        Assert.Equal(0f, snapshot.GetOrDefault(PiagetEquilibrationProvider.HypotheticalKey));
        Assert.True(snapshot.GetOrDefault(PiagetEquilibrationProvider.EgocentrismKey) > 0.5f);
        Assert.Equal(1f, snapshot.GetOrDefault(PiagetEquilibrationProvider.EquilibriumKey));
        Assert.False(snapshot.TryGet("personality.ocean.openness", out _));
    }

    [Fact]
    public void ConcreteOperational_HasConservation_ButNotHypotheticalReasoning()
    {
        var engine = PiagetComposition.Create(CognitiveStage.ConcreteOperational);
        var snapshot = engine.Tick(WorldEvent.Tick);

        Assert.Equal(1f, snapshot.GetOrDefault(PiagetEquilibrationProvider.ObjectPermanenceKey));
        Assert.Equal(1f, snapshot.GetOrDefault(PiagetEquilibrationProvider.ConservationKey));
        Assert.Equal(0f, snapshot.GetOrDefault(PiagetEquilibrationProvider.HypotheticalKey));
        Assert.True(snapshot.GetOrDefault(PiagetEquilibrationProvider.EgocentrismKey) < 0.3f);
    }

    [Fact]
    public void FormalOperational_UnlocksHypotheticalReasoning()
    {
        var engine = PiagetComposition.Create(CognitiveStage.FormalOperational);
        var snapshot = engine.Tick(WorldEvent.Tick);

        Assert.Equal(1f, snapshot.GetOrDefault(PiagetEquilibrationProvider.HypotheticalKey));
        Assert.Equal(1f, snapshot.GetOrDefault(PiagetEquilibrationProvider.ConservationKey));
    }

    [Fact]
    public void FittingEncounter_RaisesAssimilation_AndKeepsEquilibrium()
    {
        var engine = PiagetComposition.Create(CognitiveStage.ConcreteOperational);
        engine.Tick(WorldEvent.Tick);
        var snapshot = engine.Tick(
            new WorldEvent(PiagetEquilibrationProvider.EncounterKind, 0.2f, "new-block"));

        Assert.True(snapshot.GetOrDefault(PiagetEquilibrationProvider.AssimilationKey) > 0.5f);
        Assert.True(snapshot.GetOrDefault(PiagetEquilibrationProvider.AccommodationKey) < 0.3f);
        Assert.True(snapshot.GetOrDefault(PiagetEquilibrationProvider.EquilibriumKey) > 0.7f);
        Assert.True(snapshot.GetOrDefault(PiagetEquilibrationProvider.DisequilibriumKey) < 0.3f);
    }

    [Fact]
    public void MisfittingEncounter_RaisesDisequilibrium_AndAccommodationPressure()
    {
        var engine = PiagetComposition.Create(CognitiveStage.ConcreteOperational);
        engine.Tick(WorldEvent.Tick);
        var snapshot = engine.Tick(
            new WorldEvent(PiagetEquilibrationProvider.EncounterKind, 0.9f, "anomaly"));

        Assert.True(snapshot.GetOrDefault(PiagetEquilibrationProvider.DisequilibriumKey) > 0.7f);
        Assert.True(snapshot.GetOrDefault(PiagetEquilibrationProvider.AccommodationKey) > 0.5f);
        Assert.True(snapshot.GetOrDefault(PiagetEquilibrationProvider.EquilibriumKey) < 0.4f);
        Assert.True(snapshot.GetOrDefault(PiagetEquilibrationProvider.AssimilationKey) < 0.3f);
    }

    [Fact]
    public void Accommodate_RestoresEquilibrium()
    {
        var engine = PiagetComposition.Create(CognitiveStage.Preoperational);
        engine.Tick(new WorldEvent(PiagetEquilibrationProvider.EncounterKind, 0.85f));
        var snapshot = engine.Tick(new WorldEvent(PiagetEquilibrationProvider.AccommodateKind, 1f));

        Assert.Equal(1f, snapshot.GetOrDefault(PiagetEquilibrationProvider.EquilibriumKey));
        Assert.Equal(0f, snapshot.GetOrDefault(PiagetEquilibrationProvider.DisequilibriumKey));
        Assert.True(snapshot.GetOrDefault(PiagetEquilibrationProvider.AccommodationKey) > 0.5f);
    }

    [Fact]
    public void WorksWithoutOceanOrPeterson()
    {
        var engine = PiagetComposition.Create(CognitiveStage.Preoperational);
        var snap = engine.Tick(new WorldEvent(PiagetEquilibrationProvider.EncounterKind, 0.2f));
        Assert.False(snap.TryGet("personality.ocean.openness", out _));
        Assert.True(snap.TryGet(PiagetEquilibrationProvider.AssimilationKey, out _));
    }

    [Fact]
    public void ComposesWithPetersonWithoutReplacingMeaning()
    {
        var engine = PiagetComposition.CreateWithOceanAndPeterson(
            OceanTraits.GebhardExample,
            CognitiveStage.FormalOperational);

        engine.Tick(WorldEvent.Tick);
        engine.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, 0.9f));
        engine.Tick(new WorldEvent(PiagetEquilibrationProvider.EncounterKind, 0.9f));

        Assert.True(engine.Snapshot.GetOrDefault(OrderChaosMeaningProvider.ChaosKey) > 0.5f);
        Assert.True(engine.Snapshot.GetOrDefault(PiagetEquilibrationProvider.DisequilibriumKey) > 0.5f);
        Assert.True(engine.Snapshot.GetOrDefault(StabilityPlasticityProvider.StabilityKey) > 0f);
    }

    [Fact]
    public void Weighter_PrefersPlay_WhenAssimilatingFitMaterial()
    {
        var engine = PiagetComposition.Create(CognitiveStage.Preoperational);
        engine.Tick(new WorldEvent(PiagetEquilibrationProvider.EncounterKind, 0.15f));
        var weights = engine.WeightActions(new[]
        {
            PiagetCognitionWeighter.Play,
            PiagetCognitionWeighter.Imitate
        });

        weights.TryGetValue(PiagetCognitionWeighter.Imitate, out var imitate);
        Assert.True(weights[PiagetCognitionWeighter.Play] > imitate);
    }

    [Fact]
    public void Weighter_PrefersAccommodate_InDisequilibrium()
    {
        var engine = PiagetComposition.Create(CognitiveStage.ConcreteOperational);
        engine.Tick(new WorldEvent(PiagetEquilibrationProvider.EncounterKind, 0.95f));
        var weights = engine.WeightActions(new[]
        {
            PiagetCognitionWeighter.Accommodate,
            PiagetCognitionWeighter.Play
        });

        weights.TryGetValue(PiagetCognitionWeighter.Play, out var play);
        Assert.True(weights[PiagetCognitionWeighter.Accommodate] > play);
    }
}
