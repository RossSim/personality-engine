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
        var joy = IndexOf("emotion.occ.joy");
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
    public void Html_IncludesChartLegendAndTable()
    {
        var html = AlmaTimeline.ToHtml(AlmaTimeline.Run());
        Assert.Contains("<svg", html, StringComparison.Ordinal);
        Assert.Contains("id=\"legend\"", html, StringComparison.Ordinal);
        Assert.Contains("<table", html, StringComparison.Ordinal);
        Assert.Contains("PAD baseline pleasure", html, StringComparison.Ordinal);
        Assert.Contains("OCC joy", html, StringComparison.Ordinal);
        Assert.Contains("\"t\":10", html, StringComparison.Ordinal);
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
