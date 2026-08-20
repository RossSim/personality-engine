using PersonalityEngine;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;

namespace PersonalityEngine.Samples.AlmaTimeline;

/// <summary>
/// 10s host run of the default composition: 1s ticks, joy at t=1, then decay.
/// </summary>
public static class AlmaTimeline
{
    public const int DurationSeconds = 10;
    public const int JoyAtSecond = 1;

    public static readonly Metric[] Metrics =
    {
        new("mood.pad.pleasure", "PAD baseline pleasure", "#1565c0"),
        new("mood.pad.arousal", "PAD baseline arousal", "#0277bd"),
        new("mood.pad.dominance", "PAD baseline dominance", "#00838f"),
        new("mood.pad-mood.pleasure", "Current mood pleasure", "#c62828"),
        new("mood.pad-mood.arousal", "Current mood arousal", "#ef6c00"),
        new("mood.pad-mood.dominance", "Current mood dominance", "#6a1b9a"),
        new("emotion.occ.joy", "OCC joy", "#2e7d32"),
        new("mood.occ-to-pad.pleasure", "OCC→PAD pleasure", "#4527a0")
    };

    public static IReadOnlyList<Frame> Run()
    {
        var engine = AlmaComposition.Create(OceanTraits.GebhardExample);
        var frames = new List<Frame>(DurationSeconds + 1);

        engine.Tick(WorldEvent.Tick);
        frames.Add(Capture(0, engine.Snapshot));

        for (var t = 1; t <= DurationSeconds; t++)
        {
            var ev = t == JoyAtSecond
                ? new WorldEvent(OccEmotion.JoyKind, 1f)
                : WorldEvent.Tick;
            engine.Tick(ev, deltaTime: 1f);
            frames.Add(Capture(t, engine.Snapshot));
        }

        return frames;
    }

    public static string ToHtml(IReadOnlyList<Frame> frames) =>
        AlmaTimelinePage.Render(frames);

    public static string DefaultHtmlPath()
    {
        var cwd = Directory.GetCurrentDirectory();
        var nested = Path.Combine(cwd, "samples", "AlmaTimeline");
        if (Directory.Exists(nested))
            return Path.Combine(nested, "index.html");
        return Path.Combine(cwd, "index.html");
    }

    private static Frame Capture(int second, AffectSnapshot snap)
    {
        var values = new float?[Metrics.Length];
        for (var i = 0; i < Metrics.Length; i++)
        {
            if (snap.TryGet(Metrics[i].Key, out var value))
                values[i] = value;
        }

        return new Frame(second, values);
    }

    public sealed record Metric(string Key, string Label, string Color);

    public sealed record Frame(int Second, float?[] Values);
}
