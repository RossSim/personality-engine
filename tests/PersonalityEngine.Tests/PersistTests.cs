using System.Collections.Generic;
using System.Text.Json;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;
using PersonalityEngine.Providers.Skinner;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class PersistTests
{
    [Fact]
    public void Export_EmptyEngine_HasNoChannels()
    {
        var engine = new AffectEngine(System.Array.Empty<IAffectProvider>());
        var persist = engine.Export();
        Assert.Equal(AffectPersist.CurrentVersion, persist.Version);
        Assert.Empty(persist.Channels);
        Assert.Empty(persist.Providers);
    }

    [Fact]
    public void RoundTrip_DefaultComposition_AfterJoyPulse()
    {
        var live = AlmaComposition.Create(OceanTraits.GebhardExample);
        live.Tick(WorldEvent.Tick);
        live.Tick(new WorldEvent(OccEmotion.JoyKind, 1f));
        var persist = live.Export();

        var restored = AlmaComposition.Create(OceanTraits.GebhardExample);
        restored.Import(persist);
        restored.Tick(WorldEvent.Tick);

        Assert.Equal(
            live.Snapshot.GetOrDefault(PadMood.PleasureKey),
            restored.Snapshot.GetOrDefault(PadMood.PleasureKey),
            4);
        Assert.Equal(
            live.Snapshot.GetOrDefault(OccEmotion.JoyKey),
            restored.Snapshot.GetOrDefault(OccEmotion.JoyKey),
            4);
    }

    [Fact]
    public void PadMood_Export_IsInternalNotOverlay()
    {
        var engine = AlmaComposition.Create(OceanTraits.GebhardExample);
        engine.Tick(WorldEvent.Tick);
        engine.Tick(new WorldEvent(OccEmotion.JoyKind, 1f));
        var overlaid = engine.Snapshot.GetOrDefault(PadMood.PleasureKey);
        var persist = engine.Export();
        var internalP = persist.Providers[PadMood.ProviderId][PadMood.InternalPleasure];
        Assert.True(overlaid > internalP);
    }

    [Fact]
    public void Restore_DoesNotDoubleCountOccOverlay()
    {
        var live = AlmaComposition.Create(OceanTraits.GebhardExample);
        live.Tick(WorldEvent.Tick);
        live.Tick(new WorldEvent(OccEmotion.JoyKind, 1f));
        var overlaid = live.Snapshot.GetOrDefault(PadMood.PleasureKey);

        var restored = AlmaComposition.Create(OceanTraits.GebhardExample);
        restored.Import(live.Export());
        restored.Tick(WorldEvent.Tick);

        Assert.Equal(overlaid, restored.Snapshot.GetOrDefault(PadMood.PleasureKey), 4);
    }

    [Fact]
    public void OccDecay_ContinuesAfterRestore()
    {
        const float rate = 1.5f;
        var live = new AffectEngine(new IAffectProvider[] { new OccEmotion { DecayRate = rate } });
        live.Tick(new WorldEvent(OccEmotion.FearKind, 1f));

        var restored = new AffectEngine(new IAffectProvider[] { new OccEmotion { DecayRate = rate } });
        restored.Import(live.Export());

        var liveAfter = live.Tick(1f).GetOrDefault(OccEmotion.FearKey);
        var restoredAfter = restored.Tick(1f).GetOrDefault(OccEmotion.FearKey);
        Assert.Equal(liveAfter, restoredAfter, 4);
        Assert.Equal(MathF.Exp(-rate), restoredAfter, 4);
    }

    [Fact]
    public void HappyFor_RoundTrips_AndKeepsDecaying()
    {
        const float rate = 1.5f;
        var live = new AffectEngine(new IAffectProvider[] { new OccEmotion { DecayRate = rate } });
        live.Tick(new WorldEvent(OccEmotion.HappyForKind, 1f, "kin"));

        var restored = new AffectEngine(new IAffectProvider[] { new OccEmotion { DecayRate = rate } });
        restored.Import(live.Export());

        Assert.Equal(
            live.Snapshot.GetOrDefault(OccEmotion.HappyForKey),
            restored.Snapshot.GetOrDefault(OccEmotion.HappyForKey),
            4);

        var liveAfter = live.Tick(1f).GetOrDefault(OccEmotion.HappyForKey);
        var restoredAfter = restored.Tick(1f).GetOrDefault(OccEmotion.HappyForKey);
        Assert.Equal(liveAfter, restoredAfter, 4);
        Assert.Equal(MathF.Exp(-rate), restoredAfter, 4);
    }

    [Fact]
    public void TickDt_MatchesWorldEventTick()
    {
        var a = AlmaComposition.Create(OceanTraits.GebhardExample, includeOcc: false);
        var b = AlmaComposition.Create(OceanTraits.GebhardExample, includeOcc: false);
        a.Tick(WorldEvent.Tick);
        b.Tick(WorldEvent.Tick);
        a.Tick(Push(0.3f));
        b.Tick(Push(0.3f));

        var fromNamed = a.Tick(WorldEvent.Tick, 1f).GetOrDefault(PadMood.PleasureKey);
        var fromDt = b.Tick(1f).GetOrDefault(PadMood.PleasureKey);
        Assert.Equal(fromNamed, fromDt, 5);
    }

    [Fact]
    public void Skinner_StrengthRoundTrips()
    {
        var live = SkinnerComposition.Create(new[] { "peck" });
        live.Tick(new WorldEvent(OperantLearningProvider.ReinforceKind, 1f, "peck"));
        live.Tick(new WorldEvent(OperantLearningProvider.DiscriminativeStimulusKind, 0.4f));
        var strength = live.Snapshot.GetOrDefault(OperantLearningProvider.StrengthKey("peck"));

        var restored = SkinnerComposition.Create(new[] { "peck" });
        restored.Import(live.Export());
        restored.Tick(WorldEvent.Tick);

        Assert.Equal(strength, restored.Snapshot.GetOrDefault(OperantLearningProvider.StrengthKey("peck")), 4);
        Assert.Equal(0.4f, restored.Snapshot.GetOrDefault(OperantLearningProvider.SdKey), 4);
    }

    [Fact]
    public void UnknownKeys_AreIgnored()
    {
        var live = AlmaComposition.Create(OceanTraits.GebhardExample);
        live.Tick(WorldEvent.Tick);
        var persist = live.Export();
        persist.Channels["no.such.channel"] = 1f;
        persist.Providers["ghost"] = new Dictionary<string, float> { ["x"] = 1f };
        persist.Providers[OccEmotion.ProviderId] = new Dictionary<string, float>
        {
            ["not-an-occ-channel"] = 1f
        };

        var restored = AlmaComposition.Create(OceanTraits.GebhardExample);
        restored.Import(persist);
        Assert.Equal(1f, restored.Snapshot.GetOrDefault("no.such.channel"));
        restored.Tick(WorldEvent.Tick);
        Assert.False(restored.Snapshot.TryGet(OccEmotion.JoyKey, out var joy) && joy > 0f);
    }

    [Fact]
    public void JsonRoundTrip_PreservesPadMood()
    {
        var live = AlmaComposition.Create(OceanTraits.GebhardExample, includeOcc: false);
        live.Tick(WorldEvent.Tick);
        live.Tick(Push(0.25f));
        var json = JsonSerializer.Serialize(live.Export());
        var loaded = JsonSerializer.Deserialize<AffectPersist>(json);
        Assert.NotNull(loaded);

        var restored = AlmaComposition.Create(OceanTraits.GebhardExample, includeOcc: false);
        restored.Import(loaded!);
        restored.Tick(WorldEvent.Tick);
        Assert.Equal(
            live.Snapshot.GetOrDefault(PadMood.PleasureKey),
            restored.Snapshot.GetOrDefault(PadMood.PleasureKey),
            4);
    }

    private static WorldEvent Push(float pleasure) =>
        new WorldEvent(
            PadMood.PushKind,
            1f,
            payload: new Dictionary<string, float> { ["pleasure"] = pleasure });
}
