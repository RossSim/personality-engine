using PersonalityEngine;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Peterson;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class StabilityPlasticityTests
{
    [Fact]
    public void GebhardExample_HasExpectedMetatraits()
    {
        // N=0.4 → emotional stability 0.6; A=0.3; C=0.8 → Stability = 1.7/3 ≈ 0.5667
        // E=0.6; O=0.4 → Plasticity = 0.5
        var result = StabilityPlasticityProvider.Compute(OceanTraits.GebhardExample);
        Assert.Equal(0.5667, result.Stability, 3);
        Assert.Equal(0.50, result.Plasticity, 2);
        Assert.True(result.ConformityTendency > 0.5f);
    }

    [Fact]
    public void HighNeuroticism_LowersStability()
    {
        var calm = StabilityPlasticityProvider.Compute(new OceanTraits(0.5f, 0.5f, 0.5f, 0.5f, 0.1f));
        var anxious = StabilityPlasticityProvider.Compute(new OceanTraits(0.5f, 0.5f, 0.5f, 0.5f, 0.9f));
        Assert.True(calm.Stability > anxious.Stability);
    }

    [Fact]
    public void HighOpennessAndExtraversion_RaisesPlasticity()
    {
        var explorer = StabilityPlasticityProvider.Compute(new OceanTraits(0.9f, 0.5f, 0.9f, 0.5f, 0.5f));
        var reserved = StabilityPlasticityProvider.Compute(new OceanTraits(0.2f, 0.5f, 0.2f, 0.5f, 0.5f));
        Assert.True(explorer.Plasticity > reserved.Plasticity);
        Assert.True(reserved.ConformityTendency > explorer.ConformityTendency);
    }

    [Fact]
    public void ReadsOceanFromSnapshot()
    {
        var engine = new AffectEngine(new IAffectProvider[]
        {
            new OceanPersonality(OceanTraits.GebhardExample),
            new StabilityPlasticityProvider()
        });
        var snap = engine.Tick(WorldEvent.Tick);
        Assert.True(snap.TryGet(StabilityPlasticityProvider.StabilityKey, out var s));
        Assert.Equal(0.5667, s, 3);
    }

    [Fact]
    public void FallbackTraits_WorkWithoutOceanProvider()
    {
        var engine = new AffectEngine(new IAffectProvider[]
        {
            new StabilityPlasticityProvider(OceanTraits.GebhardExample)
        });
        var snap = engine.Tick(WorldEvent.Tick);
        Assert.True(snap.TryGet(StabilityPlasticityProvider.PlasticityKey, out var p));
        Assert.Equal(0.50, p, 2);
    }

    [Fact]
    public void MissingOcean_WritesNothing()
    {
        var engine = new AffectEngine(new IAffectProvider[] { new StabilityPlasticityProvider() });
        var snap = engine.Tick(WorldEvent.Tick);
        Assert.False(snap.TryGet(StabilityPlasticityProvider.StabilityKey, out _));
    }
}
