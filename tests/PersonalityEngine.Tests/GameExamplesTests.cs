using PersonalityEngine.Providers.Dyad;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Samples.Examples;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class GameExamplesTests
{
    [Fact]
    public void Raid_ThreeSeeds_PickThreeDifferentVerbs()
    {
        var story = GameExamples.RunRaid();
        Assert.Equal(3, story.People.Count);
        var picks = story.People.Select(p => p.After.Pick).ToArray();
        Assert.Equal(RaidWeighter.Aid, picks[0]);
        Assert.Equal(RaidWeighter.Freeze, picks[1]);
        Assert.Equal(RaidWeighter.Flee, picks[2]);
        Assert.Equal(3, picks.Distinct().Count());
    }

    [Fact]
    public void Raid_Threat_RaisesFear()
    {
        var mara = GameExamples.RunRaid().People[0];
        Assert.True(mara.Before.Fear < 0.05f);
        Assert.True(mara.After.Fear > 0.5f);
    }

    [Fact]
    public void KindVisits_LikingRises_AndApproachWins()
    {
        var story = GameExamples.RunVisits(cruel: false);
        Assert.Equal(GameExamples.VisitCount, story.Frames.Count);
        Assert.True(story.Frames[^1].Liking > 0.5f);
        Assert.True(story.Frames[^1].Approach > story.Frames[^1].Avoid);
        Assert.Equal(DyadWeighter.Approach(GameExamples.PlayerId), story.Frames[^1].Pick);
    }

    [Fact]
    public void CruelVisits_LikingFalls_AndAvoidWins()
    {
        var story = GameExamples.RunVisits(cruel: true);
        Assert.True(story.Frames[^1].Liking < -0.5f);
        Assert.True(story.Frames[^1].Avoid > story.Frames[^1].Approach);
        Assert.Equal(DyadWeighter.Avoid(GameExamples.PlayerId), story.Frames[^1].Pick);
    }

    [Fact]
    public void Visits_AngerDecays_BetweenPulses()
    {
        var engine = GameExamples.CreateShopEngine();
        engine.Tick(WorldEvent.Tick);
        engine.Tick(HostEvents.Anger(GameExamples.PlayerId));
        var hot = engine.Snapshot.GetOrDefault(OccEmotion.AngerKey);
        engine.Tick(GameExamples.VisitGapSeconds);
        var cooled = engine.Snapshot.GetOrDefault(OccEmotion.AngerKey);
        Assert.True(hot > 0.5f);
        Assert.True(cooled < hot);
    }

    [Fact]
    public void Scale_ShrineFire_MovesNationVillageAndPriest()
    {
        var story = GameExamples.RunScale();
        Assert.Equal(4, story.Frames.Count);
        var before = story.Frames[0];
        var after = story.Frames[^1];
        Assert.True(after.Nation.Chaos > before.Nation.Chaos);
        Assert.True(after.Village.Pleasure < before.Village.Pleasure);
        Assert.True(after.Priest.Anger > before.Priest.Anger);
        Assert.Equal(0f, after.Priest.Liking);
        Assert.False(string.IsNullOrEmpty(after.Nation.Pick));
    }

    [Fact]
    public void Html_IncludesTheThreeStories()
    {
        var html = GameExamples.ToHtml();
        Assert.Contains("Three civilians", html, StringComparison.Ordinal);
        Assert.Contains("shopkeeper", html, StringComparison.Ordinal);
        Assert.Contains("four scales", html, StringComparison.Ordinal);
        Assert.Contains("HostEvents.Threat", html, StringComparison.Ordinal);
        Assert.Contains("approach:player", html, StringComparison.Ordinal);
        Assert.Contains("four scales", html, StringComparison.Ordinal);
    }
}
