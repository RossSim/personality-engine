using PersonalityEngine.Providers.Dyad;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class HostEventsTests
{
    [Fact]
    public void NeedMet_WritesJoy() =>
        AssertChannel(HostEvents.NeedMet(), OccEmotion.JoyKey);

    [Fact]
    public void Harm_WritesDistress() =>
        AssertChannel(HostEvents.Harm(), OccEmotion.DistressKey);

    [Fact]
    public void Threat_WritesFear() =>
        AssertChannel(HostEvents.Threat(), OccEmotion.FearKey);

    [Fact]
    public void ThreatPassed_WritesRelief() =>
        AssertChannel(HostEvents.ThreatPassed(), OccEmotion.ReliefKey);

    [Fact]
    public void SelfCredit_WritesPride() =>
        AssertChannel(HostEvents.SelfCredit(), OccEmotion.PrideKey);

    [Fact]
    public void SelfBlame_WritesShame() =>
        AssertChannel(HostEvents.SelfBlame(), OccEmotion.ShameKey);

    [Fact]
    public void Helpers_UseTheSameKindsOccAlreadyUnderstands()
    {
        Assert.Equal(OccEmotion.JoyKind, HostEvents.NeedMet().Kind);
        Assert.Equal(OccEmotion.DistressKind, HostEvents.Harm().Kind);
        Assert.Equal(OccEmotion.FearKind, HostEvents.Threat().Kind);
        Assert.Equal(OccEmotion.ReliefKind, HostEvents.ThreatPassed().Kind);
        Assert.Equal(OccEmotion.PrideKind, HostEvents.SelfCredit().Kind);
        Assert.Equal(OccEmotion.ShameKind, HostEvents.SelfBlame().Kind);
        Assert.Equal(OccEmotion.HappyForKind, HostEvents.HappyFor("kin").Kind);
        Assert.Equal(OccEmotion.PityKind, HostEvents.Pity("kin").Kind);
        Assert.Equal(OccEmotion.ResentmentKind, HostEvents.Resent("rival").Kind);
        Assert.Equal(OccEmotion.GloatingKind, HostEvents.Gloat("rival").Kind);
        Assert.Equal(OccEmotion.AngerKind, HostEvents.Anger("rival").Kind);
        Assert.Equal(OccEmotion.GratitudeKind, HostEvents.Gratitude("kin").Kind);
        Assert.Equal(OccEmotion.GratificationKind, HostEvents.Gratification().Kind);
        Assert.Equal(OccEmotion.RemorseKind, HostEvents.Remorse().Kind);
        Assert.Equal(DyadProvider.LikeKind, HostEvents.Like("kin").Kind);
        Assert.Equal(DyadProvider.DislikeKind, HostEvents.Dislike("rival").Kind);
    }

    [Fact]
    public void HappyFor_WritesChannel_AndKeepsTarget() =>
        AssertSocial(HostEvents.HappyFor("kin"), OccEmotion.HappyForKey, "kin");

    [Fact]
    public void Pity_WritesChannel_AndKeepsTarget() =>
        AssertSocial(HostEvents.Pity("kin"), OccEmotion.PityKey, "kin");

    [Fact]
    public void Resent_WritesChannel_AndKeepsTarget() =>
        AssertSocial(HostEvents.Resent("rival"), OccEmotion.ResentmentKey, "rival");

    [Fact]
    public void Gloat_WritesChannel_AndKeepsTarget() =>
        AssertSocial(HostEvents.Gloat("rival"), OccEmotion.GloatingKey, "rival");

    [Fact]
    public void Anger_WritesChannel_AndKeepsTarget() =>
        AssertSocial(HostEvents.Anger("rival"), OccEmotion.AngerKey, "rival");

    [Fact]
    public void Gratitude_WritesChannel_AndKeepsTarget() =>
        AssertSocial(HostEvents.Gratitude("kin"), OccEmotion.GratitudeKey, "kin");

    [Fact]
    public void Gratification_WritesChannel() =>
        AssertChannel(HostEvents.Gratification(), OccEmotion.GratificationKey);

    [Fact]
    public void Remorse_WritesChannel() =>
        AssertChannel(HostEvents.Remorse(), OccEmotion.RemorseKey);

    [Fact]
    public void Like_WritesLiking_AndKeepsTarget()
    {
        Assert.Equal("kin", HostEvents.Like("kin").Target);
        var engine = DyadComposition.Create();
        var snap = engine.Tick(HostEvents.Like("kin"));
        Assert.True(snap.GetOrDefault(DyadProvider.LikingKey("kin")) > 0f);
    }

    [Fact]
    public void Dislike_WritesNegativeLiking_AndKeepsTarget()
    {
        Assert.Equal("rival", HostEvents.Dislike("rival").Target);
        var engine = DyadComposition.Create();
        var snap = engine.Tick(HostEvents.Dislike("rival"));
        Assert.True(snap.GetOrDefault(DyadProvider.LikingKey("rival")) < 0f);
    }

    private static void AssertChannel(WorldEvent ev, string key)
    {
        var engine = AlmaComposition.Create(OceanTraits.GebhardExample);
        engine.Tick(WorldEvent.Tick);
        var snap = engine.Tick(ev);
        Assert.True(snap.GetOrDefault(key) > 0f);
    }

    private static void AssertSocial(WorldEvent ev, string key, string otherId)
    {
        Assert.Equal(otherId, ev.Target);
        AssertChannel(ev, key);
    }
}
