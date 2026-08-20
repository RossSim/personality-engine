using System;
using PersonalityEngine;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class OccEmotionTests
{
    [Fact]
    public void IsNoOp_UntilAnElicitingEvent()
    {
        var engine = new AffectEngine(new IAffectProvider[] { new OccEmotion() });
        var snap = engine.Tick(WorldEvent.Tick);
        Assert.False(snap.TryGet(OccEmotion.JoyKey, out _));
    }

    [Fact]
    public void AppraisesJoy_WithoutPadMood()
    {
        var engine = new AffectEngine(new IAffectProvider[] { new OccEmotion() });
        var snap = engine.Tick(new WorldEvent(OccEmotion.JoyKind, 0.8f));
        Assert.Equal(0.8, snap.GetOrDefault(OccEmotion.JoyKey), 2);
        Assert.False(snap.TryGet(PadMood.PleasureKey, out _));
        Assert.False(snap.TryGet(OceanToPadMapping.PleasureKey, out _));
    }

    [Fact]
    public void Mapping_IsNoOp_WithoutEmotions()
    {
        var engine = new AffectEngine(new IAffectProvider[] { new OccToPadMapping() });
        var snap = engine.Tick(WorldEvent.Tick);
        Assert.False(snap.TryGet(OccToPadMapping.PleasureKey, out _));
    }

    [Fact]
    public void Mapping_WritesPadOverlay_FromJoy()
    {
        var engine = new AffectEngine(new IAffectProvider[]
        {
            new OccEmotion(),
            new OccToPadMapping()
        });
        var snap = engine.Tick(new WorldEvent(OccEmotion.JoyKind, 1f));
        Assert.True(snap.GetOrDefault(OccToPadMapping.PleasureKey) > 0f);
        Assert.True(snap.GetOrDefault(OccToPadMapping.ArousalKey) > 0f);
    }

    [Fact]
    public void DecaysTowardZero_OverDt()
    {
        const float rate = 1.5f;
        var engine = new AffectEngine(new IAffectProvider[] { new OccEmotion { DecayRate = rate } });
        engine.Tick(new WorldEvent(OccEmotion.FearKind, 1f));
        var snap = engine.Tick(WorldEvent.Tick, deltaTime: 1f);
        Assert.Equal(MathF.Exp(-rate), snap.GetOrDefault(OccEmotion.FearKey), 4);
    }

    [Fact]
    public void Decay_WritesZero_WhenBelowFloor()
    {
        var engine = new AffectEngine(new IAffectProvider[] { new OccEmotion { DecayRate = 1.5f } });
        engine.Tick(new WorldEvent(OccEmotion.FearKind, 1f));
        for (var i = 0; i < 8; i++)
            engine.Tick(WorldEvent.Tick, deltaTime: 1f);
        Assert.Equal(0f, engine.Snapshot.GetOrDefault(OccEmotion.FearKey));
    }

    [Fact]
    public void AlmaComposition_OmitsEmotionWhenTold()
    {
        var engine = AlmaComposition.Create(OceanTraits.GebhardExample, includeOcc: false);
        var snap = engine.Tick(new WorldEvent(OccEmotion.JoyKind, 1f));
        Assert.False(snap.TryGet(OccEmotion.JoyKey, out _));
        Assert.False(snap.TryGet(OccToPadMapping.PleasureKey, out _));
        Assert.True(PadMood.TryRead(snap, out _, out _, out _));
    }

    [Fact]
    public void AlmaComposition_OverlaysEmotionOnCurrentMood()
    {
        var engine = AlmaComposition.Create(OceanTraits.GebhardExample);
        engine.Tick(WorldEvent.Tick);
        var before = engine.Snapshot.GetOrDefault(PadMood.PleasureKey);

        engine.Tick(new WorldEvent(OccEmotion.JoyKind, 1f));
        var after = engine.Snapshot.GetOrDefault(PadMood.PleasureKey);
        Assert.True(after > before);
        Assert.Equal(0.38, engine.Snapshot.GetOrDefault(OceanToPadMapping.PleasureKey), 2);
    }
}
