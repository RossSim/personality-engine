using PersonalityEngine.Providers.Dyad;
using PersonalityEngine.Samples.SocialTint;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class SocialTintTests
{
    [Fact]
    public void SeededChooser_PicksAvoidRival_FromHostBases()
    {
        var engine = SocialTintHost.CreateEngine();
        engine.Tick(WorldEvent.Tick);
        Assert.Equal(DyadWeighter.Avoid(SocialTintHost.Rival), SocialTintHost.Pick(engine));
    }

    [Fact]
    public void LikeAlly_FlipsPick_ToApproachAlly()
    {
        var engine = SocialTintHost.CreateEngine();
        engine.Tick(WorldEvent.Tick);
        engine.Tick(HostEvents.Like(SocialTintHost.Ally));
        Assert.Equal(DyadWeighter.Approach(SocialTintHost.Ally), SocialTintHost.Pick(engine));
    }

    [Fact]
    public void MissingChannels_DoNotThrow()
    {
        var weights = new DyadWeighter().Weight(new AffectSnapshot(), SocialTintHost.Actions);
        Assert.Empty(weights);
    }

    [Fact]
    public void Gloating_WithoutLiking_TintsAvoid()
    {
        var engine = SocialTintHost.CreateEngine();
        engine.Tick(WorldEvent.Tick);
        engine.Tick(HostEvents.Gloat(SocialTintHost.Rival));
        var finals = SocialTintHost.Finals(engine);
        Assert.True(finals[DyadWeighter.Avoid(SocialTintHost.Rival)]
            > SocialTintHost.DefaultBaseScores[DyadWeighter.Avoid(SocialTintHost.Rival)]);
    }
}
