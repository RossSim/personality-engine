using PersonalityEngine;
using PersonalityEngine.Providers.Ocean;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class OceanToPadTests
{
    [Fact]
    public void GebhardExample_MapsToPublishedPad()
    {
        var pad = OceanToPadMapping.Map(OceanTraits.GebhardExample);
        Assert.Equal(0.38, pad.Pleasure, 2);
        Assert.Equal(-0.08, pad.Arousal, 2);
        Assert.Equal(0.50, pad.Dominance, 2);
    }

    [Fact]
    public void Engine_WritesPadChannels_WhenOceanPresent()
    {
        var engine = new AffectEngine(new IAffectProvider[]
        {
            new OceanPersonality(OceanTraits.GebhardExample),
            new OceanToPadMapping()
        });
        var snap = engine.Tick(WorldEvent.Tick);
        Assert.Equal(0.38, snap.GetOrDefault(OceanToPadMapping.PleasureKey), 2);
    }

    [Fact]
    public void Mapping_IsNoOp_WithoutOcean()
    {
        var engine = new AffectEngine(new IAffectProvider[] { new OceanToPadMapping() });
        var snap = engine.Tick(WorldEvent.Tick);
        Assert.False(snap.TryGet(OceanToPadMapping.PleasureKey, out _));
    }
}
