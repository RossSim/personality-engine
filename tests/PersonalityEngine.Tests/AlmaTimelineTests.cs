using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Samples.AlmaTimeline;
using Xunit;

namespace PersonalityEngine.Tests;

public sealed class AlmaTimelineTests
{
    [Fact]
    public void RunsTenSeconds_InOneSecondTicks()
    {
        var frames = AlmaTimeline.Run();
        Assert.Equal(AlmaTimeline.DurationSeconds + 1, frames.Count);
        for (var t = 0; t <= AlmaTimeline.DurationSeconds; t++)
            Assert.Equal(t, frames[t].Second);
    }

    [Fact]
    public void JoyAppearsAtOneSecond_ThenDecays()
    {
        var frames = AlmaTimeline.Run();
        var joy = IndexOf(OccEmotion.JoyKey);
        var moodP = IndexOf("mood.pad-mood.pleasure");
        var baselineP = IndexOf("mood.pad.pleasure");

        Assert.Null(frames[0].Values[joy]);
        Assert.True(frames[AlmaTimeline.JoyAtSecond].Values[joy] > 0f);
        Assert.True(frames[AlmaTimeline.JoyAtSecond + 1].Values[joy] < frames[AlmaTimeline.JoyAtSecond].Values[joy]);
        Assert.True(frames[10].Values[joy] is null or < 0.01f);
        Assert.True(frames[AlmaTimeline.JoyAtSecond].Values[moodP] > frames[0].Values[moodP]);
        Assert.Equal(0.38, frames[0].Values[baselineP]!.Value, 2);
        Assert.Equal(0.38, frames[10].Values[baselineP]!.Value, 2);
    }

    [Fact]
    public void TwoEvents_StaggerZero_FireOnTheSameTick()
    {
        var frames = AlmaTimeline.Run(new AlmaTimeline.TimelineRequest(
            new[]
            {
                new AlmaTimeline.OccPulse(OccEmotion.JoyKind, 1f),
                new AlmaTimeline.OccPulse(OccEmotion.FearKind, 1f)
            },
            StaggerSeconds: 0,
            FirstAtSecond: 1));
        Assert.True(frames[1].Values[IndexOf(OccEmotion.JoyKey)] > 0f);
        Assert.True(frames[1].Values[IndexOf(OccEmotion.FearKey)] > 0f);
        Assert.Null(frames[0].Values[IndexOf(OccEmotion.FearKey)]);
    }

    [Fact]
    public void TwoEvents_StaggerTwo_AreTwoSecondsApart()
    {
        var frames = AlmaTimeline.Run(new AlmaTimeline.TimelineRequest(
            new[]
            {
                new AlmaTimeline.OccPulse(OccEmotion.JoyKind, 1f),
                new AlmaTimeline.OccPulse(OccEmotion.FearKind, 1f)
            },
            StaggerSeconds: 2,
            FirstAtSecond: 1));
        Assert.True(frames[1].Values[IndexOf(OccEmotion.JoyKey)] > 0f);
        Assert.Null(frames[1].Values[IndexOf(OccEmotion.FearKey)]);
        Assert.True(frames[3].Values[IndexOf(OccEmotion.FearKey)] > 0f);
    }

    [Fact]
    public void Intensity_IsHonored()
    {
        var full = AlmaTimeline.Run(new AlmaTimeline.TimelineRequest(
            new[] { new AlmaTimeline.OccPulse(OccEmotion.JoyKind, 1f) }, 0, 1));
        var half = AlmaTimeline.Run(new AlmaTimeline.TimelineRequest(
            new[] { new AlmaTimeline.OccPulse(OccEmotion.JoyKind, 0.4f) }, 0, 1));
        Assert.True(half[1].Values[IndexOf(OccEmotion.JoyKey)] < full[1].Values[IndexOf(OccEmotion.JoyKey)]);
    }

    [Fact]
    public void Schedule_RejectsUnknownKind()
    {
        Assert.Throws<ArgumentException>(() => AlmaTimeline.Schedule(
            new AlmaTimeline.TimelineRequest(
                new[] { new AlmaTimeline.OccPulse("occ.not-a-type", 1f) },
                0)));
    }

    [Fact]
    public void Html_IncludesControls()
    {
        var html = AlmaTimeline.ToHtml();
        Assert.Contains("<svg", html, StringComparison.Ordinal);
        Assert.Contains("id=\"run\"", html, StringComparison.Ordinal);
        Assert.Contains("id=\"stagger\"", html, StringComparison.Ordinal);
        Assert.Contains("id=\"events\"", html, StringComparison.Ordinal);
        Assert.Contains("occ.joy", html, StringComparison.Ordinal);
        Assert.Contains("occ.fear", html, StringComparison.Ordinal);
        Assert.Contains("Run Test", html, StringComparison.Ordinal);
    }

    private static int IndexOf(string key)
    {
        for (var i = 0; i < AlmaTimeline.Metrics.Length; i++)
        {
            if (AlmaTimeline.Metrics[i].Key == key)
                return i;
        }

        throw new InvalidOperationException(key);
    }
}
