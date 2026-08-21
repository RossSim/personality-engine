using System.Collections.Generic;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;
using PersonalityEngine.Samples.UtilityTint;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class UtilityTintTests
{
    [Fact]
    public void SeededChooser_PicksRoleWork_FromHostBases()
    {
        var engine = UtilityTintHost.CreateEngine();
        engine.Tick(WorldEvent.Tick);
        Assert.Equal(UtilityTintWeighter.RoleWork, UtilityTintHost.Pick(engine));
    }

    [Fact]
    public void Threat_FlipsPick_ToMeetNeed()
    {
        var engine = UtilityTintHost.CreateEngine();
        engine.Tick(WorldEvent.Tick);
        engine.Tick(HostEvents.Threat());
        Assert.Equal(UtilityTintWeighter.MeetNeed, UtilityTintHost.Pick(engine));
    }

    [Fact]
    public void MissingChannels_DoNotThrow()
    {
        var weighter = new UtilityTintWeighter();
        var weights = weighter.Weight(
            new AffectSnapshot(),
            new[]
            {
                UtilityTintWeighter.MeetNeed,
                UtilityTintWeighter.RoleWork,
                UtilityTintWeighter.Wander
            });
        Assert.Empty(weights);
    }

    [Fact]
    public void Combine_LeavesHostAsDecider()
    {
        var bases = new Dictionary<string, float>
        {
            [UtilityTintWeighter.MeetNeed] = 10f,
            [UtilityTintWeighter.RoleWork] = 1f
        };
        var tints = new Dictionary<string, float>
        {
            [UtilityTintWeighter.MeetNeed] = 1f,
            [UtilityTintWeighter.RoleWork] = 1f
        };
        var finals = HostChooser.Combine(bases, tints, gain: 0.35f);
        Assert.Equal(UtilityTintWeighter.MeetNeed, HostChooser.Pick(finals));
    }
}
