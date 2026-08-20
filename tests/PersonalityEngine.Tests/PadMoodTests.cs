using System;
using System.Collections.Generic;
using PersonalityEngine;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class PadMoodTests
{
    [Fact]
    public void IsNoOp_WithoutMappedBaseline()
    {
        var engine = new AffectEngine(new IAffectProvider[] { new PadMood() });
        var snap = engine.Tick(WorldEvent.Tick);
        Assert.False(snap.TryGet(PadMood.PleasureKey, out _));
        Assert.False(snap.TryGet(OceanToPadMapping.PleasureKey, out _));
    }

    [Fact]
    public void SeedsCurrentMood_FromGebhardBaseline()
    {
        var snap = AlmaComposition.Create(OceanTraits.GebhardExample, includeOcc: false).Tick(WorldEvent.Tick);
        Assert.Equal(0.38, snap.GetOrDefault(OceanToPadMapping.PleasureKey), 2);
        Assert.Equal(0.38, snap.GetOrDefault(PadMood.PleasureKey), 2);
        Assert.Equal(-0.08, snap.GetOrDefault(PadMood.ArousalKey), 2);
        Assert.Equal(0.50, snap.GetOrDefault(PadMood.DominanceKey), 2);
    }

    [Fact]
    public void Coexists_WithMappingBaselineChannels()
    {
        var engine = AlmaComposition.Create(OceanTraits.GebhardExample, includeOcc: false);
        engine.Tick(WorldEvent.Tick);
        engine.Tick(Push(pleasure: 0.3f));

        var snap = engine.Snapshot;
        Assert.Equal(0.38, snap.GetOrDefault(OceanToPadMapping.PleasureKey), 2);
        Assert.Equal(0.68, snap.GetOrDefault(PadMood.PleasureKey), 2);
    }

    [Fact]
    public void DecaysTowardBaseline_OverDt()
    {
        const float rate = 0.5f;
        var engine = AlmaComposition.Create(OceanTraits.GebhardExample, decayRate: rate, includeOcc: false);
        engine.Tick(WorldEvent.Tick);
        var baseline = engine.Snapshot.GetOrDefault(OceanToPadMapping.PleasureKey);
        engine.Tick(Push(pleasure: 0.3f));

        var snap = engine.Tick(WorldEvent.Tick, deltaTime: 1f);
        var expected = baseline + 0.3f * MathF.Exp(-rate);
        Assert.Equal(expected, snap.GetOrDefault(PadMood.PleasureKey), 4);
        Assert.Equal(baseline, snap.GetOrDefault(OceanToPadMapping.PleasureKey), 4);
    }

    [Fact]
    public void RepeatedTicks_CloseTheGapToBaseline()
    {
        var engine = AlmaComposition.Create(OceanTraits.GebhardExample, includeOcc: false);
        engine.Tick(WorldEvent.Tick);
        engine.Tick(Push(pleasure: 0.4f));
        var afterPush = Distance(engine.Snapshot);

        engine.Tick(WorldEvent.Tick, 1f);
        var afterOne = Distance(engine.Snapshot);
        engine.Tick(WorldEvent.Tick, 1f);
        var afterTwo = Distance(engine.Snapshot);

        Assert.True(afterOne < afterPush);
        Assert.True(afterTwo < afterOne);
    }

    private static WorldEvent Push(float pleasure = 0f, float arousal = 0f, float dominance = 0f) =>
        new WorldEvent(
            PadMood.PushKind,
            1f,
            payload: new Dictionary<string, float>
            {
                ["pleasure"] = pleasure,
                ["arousal"] = arousal,
                ["dominance"] = dominance
            });

    private static float Distance(AffectSnapshot snap)
    {
        var dp = snap.GetOrDefault(PadMood.PleasureKey) - snap.GetOrDefault(OceanToPadMapping.PleasureKey);
        var da = snap.GetOrDefault(PadMood.ArousalKey) - snap.GetOrDefault(OceanToPadMapping.ArousalKey);
        var dd = snap.GetOrDefault(PadMood.DominanceKey) - snap.GetOrDefault(OceanToPadMapping.DominanceKey);
        return MathF.Abs(dp) + MathF.Abs(da) + MathF.Abs(dd);
    }
}
