using PersonalityEngine.Providers.Dyad;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class DyadTests
{
    private const string Ally = "ally";
    private const string Rival = "rival";

    [Fact]
    public void IsNoOp_UntilALikeOrDislike()
    {
        var engine = DyadComposition.Create();
        var snap = engine.Tick(WorldEvent.Tick);
        Assert.False(snap.TryGet(DyadProvider.LikingKey(Ally), out _));
    }

    [Fact]
    public void LikeAndDislike_AreIndependentPerOther()
    {
        var engine = DyadComposition.Create();
        engine.Tick(HostEvents.Like(Ally));
        engine.Tick(HostEvents.Dislike(Rival));
        Assert.Equal(1f, engine.Snapshot.GetOrDefault(DyadProvider.LikingKey(Ally)), 4);
        Assert.Equal(-1f, engine.Snapshot.GetOrDefault(DyadProvider.LikingKey(Rival)), 4);
    }

    [Fact]
    public void Like_WithoutTarget_IsNoOp()
    {
        var engine = DyadComposition.Create();
        engine.Tick(new WorldEvent(DyadProvider.LikeKind, 1f));
        Assert.False(engine.Snapshot.TryGet(DyadProvider.LikingKey(""), out _));
        Assert.Empty(engine.Snapshot.Channels);
    }

    [Fact]
    public void AlmaComposition_OmitsDyad()
    {
        var engine = AlmaComposition.Create(OceanTraits.GebhardExample);
        engine.Tick(HostEvents.Like(Ally));
        Assert.False(engine.Snapshot.TryGet(DyadProvider.LikingKey(Ally), out _));
    }

    [Fact]
    public void DecaysTowardZero_OverDt()
    {
        const float rate = 0.5f;
        var engine = new AffectEngine(new IAffectProvider[] { new DyadProvider { DecayRate = rate } });
        engine.Tick(HostEvents.Like(Ally));
        var snap = engine.Tick(1f);
        Assert.Equal(MathF.Exp(-rate), snap.GetOrDefault(DyadProvider.LikingKey(Ally)), 4);
    }

    [Fact]
    public void RoundTrip_PreservesLiking_AndKeepsDecaying()
    {
        const float rate = 0.5f;
        var live = new AffectEngine(new IAffectProvider[] { new DyadProvider { DecayRate = rate } });
        live.Tick(HostEvents.Like(Ally));
        live.Tick(HostEvents.Dislike(Rival, 0.6f));

        var restored = new AffectEngine(new IAffectProvider[] { new DyadProvider { DecayRate = rate } });
        restored.Import(live.Export());

        Assert.Equal(
            live.Snapshot.GetOrDefault(DyadProvider.LikingKey(Ally)),
            restored.Snapshot.GetOrDefault(DyadProvider.LikingKey(Ally)),
            4);
        Assert.Equal(
            live.Snapshot.GetOrDefault(DyadProvider.LikingKey(Rival)),
            restored.Snapshot.GetOrDefault(DyadProvider.LikingKey(Rival)),
            4);

        Assert.Equal(live.Tick(1f).GetOrDefault(DyadProvider.LikingKey(Ally)),
            restored.Tick(1f).GetOrDefault(DyadProvider.LikingKey(Ally)), 4);
    }

    [Fact]
    public void Import_IgnoresUnknownBagKeys()
    {
        var live = DyadComposition.Create();
        live.Tick(HostEvents.Like(Ally));
        var persist = live.Export();
        persist.Providers[DyadProvider.ProviderId]["not-liking"] = 1f;

        var restored = DyadComposition.Create();
        restored.Import(persist);
        restored.Tick(WorldEvent.Tick);
        Assert.True(restored.Snapshot.GetOrDefault(DyadProvider.LikingKey(Ally)) > 0f);
        Assert.False(restored.Snapshot.TryGet("not-liking", out _));
    }
}
