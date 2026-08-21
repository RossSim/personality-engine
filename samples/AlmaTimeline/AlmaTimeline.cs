using PersonalityEngine;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;

namespace PersonalityEngine.Samples.AlmaTimeline;

/// <summary>
/// 10s host run of the default composition. Default: joy at t=1s.
/// Callers can schedule any OCC types, intensity, and stagger.
/// </summary>
public static class AlmaTimeline
{
    public const int DurationSeconds = 10;
    public const int JoyAtSecond = 1;
    public const int MaxStaggerSeconds = 3;

    public static readonly OccOption[] EventOptions =
    {
        new(OccEmotion.JoyKind, "Joy"),
        new(OccEmotion.DistressKind, "Distress"),
        new(OccEmotion.HopeKind, "Hope"),
        new(OccEmotion.FearKind, "Fear"),
        new(OccEmotion.SatisfactionKind, "Satisfaction"),
        new(OccEmotion.FearsConfirmedKind, "Fears confirmed"),
        new(OccEmotion.ReliefKind, "Relief"),
        new(OccEmotion.DisappointmentKind, "Disappointment"),
        new(OccEmotion.PrideKind, "Pride"),
        new(OccEmotion.ShameKind, "Shame"),
        new(OccEmotion.AdmirationKind, "Admiration"),
        new(OccEmotion.ReproachKind, "Reproach"),
        new(OccEmotion.HappyForKind, "Happy-for"),
        new(OccEmotion.PityKind, "Pity"),
        new(OccEmotion.ResentmentKind, "Resentment"),
        new(OccEmotion.GloatingKind, "Gloating"),
        new(OccEmotion.GratificationKind, "Gratification"),
        new(OccEmotion.GratitudeKind, "Gratitude"),
        new(OccEmotion.AngerKind, "Anger"),
        new(OccEmotion.RemorseKind, "Remorse")
    };

    public static readonly Metric[] Metrics =
    {
        new("mood.pad.pleasure", "PAD baseline pleasure", "#1565c0"),
        new("mood.pad.arousal", "PAD baseline arousal", "#0277bd"),
        new("mood.pad.dominance", "PAD baseline dominance", "#00838f"),
        new("mood.pad-mood.pleasure", "Current mood pleasure", "#c62828"),
        new("mood.pad-mood.arousal", "Current mood arousal", "#ef6c00"),
        new("mood.pad-mood.dominance", "Current mood dominance", "#6a1b9a"),
        new(OccEmotion.JoyKey, "OCC joy", "#2e7d32"),
        new(OccEmotion.DistressKey, "OCC distress", "#ad1457"),
        new(OccEmotion.HopeKey, "OCC hope", "#558b2f"),
        new(OccEmotion.FearKey, "OCC fear", "#6a1b9a"),
        new(OccEmotion.SatisfactionKey, "OCC satisfaction", "#00695c"),
        new(OccEmotion.FearsConfirmedKey, "OCC fears-confirmed", "#4a148c"),
        new(OccEmotion.ReliefKey, "OCC relief", "#2e7d32"),
        new(OccEmotion.DisappointmentKey, "OCC disappointment", "#bf360c"),
        new(OccEmotion.PrideKey, "OCC pride", "#1b5e20"),
        new(OccEmotion.ShameKey, "OCC shame", "#880e4f"),
        new(OccEmotion.AdmirationKey, "OCC admiration", "#33691e"),
        new(OccEmotion.ReproachKey, "OCC reproach", "#b71c1c"),
        new(OccEmotion.HappyForKey, "OCC happy-for", "#2e7d32"),
        new(OccEmotion.PityKey, "OCC pity", "#6a1b9a"),
        new(OccEmotion.ResentmentKey, "OCC resentment", "#bf360c"),
        new(OccEmotion.GloatingKey, "OCC gloating", "#4a148c"),
        new(OccEmotion.GratificationKey, "OCC gratification", "#1b5e20"),
        new(OccEmotion.GratitudeKey, "OCC gratitude", "#33691e"),
        new(OccEmotion.AngerKey, "OCC anger", "#b71c1c"),
        new(OccEmotion.RemorseKey, "OCC remorse", "#880e4f"),
        new(OccToPadMapping.PleasureKey, "OCC→PAD pleasure", "#4527a0"),
        new(OccToPadMapping.ArousalKey, "OCC→PAD arousal", "#283593"),
        new(OccToPadMapping.DominanceKey, "OCC→PAD dominance", "#1a237e")
    };

    public static TimelineRequest DefaultRequest { get; } = new(
        new[] { new OccPulse(OccEmotion.JoyKind, 1f) },
        StaggerSeconds: 0,
        FirstAtSecond: JoyAtSecond);

    public static IReadOnlyList<Frame> Run() => Run(DefaultRequest);

    public static IReadOnlyList<Frame> Run(TimelineRequest request)
    {
        var schedule = Schedule(request);
        var bySecond = new Dictionary<int, List<ScheduledPulse>>();
        foreach (var pulse in schedule)
        {
            if (!bySecond.TryGetValue(pulse.AtSecond, out var list))
            {
                list = new List<ScheduledPulse>();
                bySecond[pulse.AtSecond] = list;
            }
            list.Add(pulse);
        }

        var engine = AlmaComposition.Create(OceanTraits.GebhardExample);
        var frames = new List<Frame>(DurationSeconds + 1);

        engine.Tick(WorldEvent.Tick);
        ApplyPulses(engine, bySecond, 0, deltaTime: 0f);
        frames.Add(Capture(0, engine.Snapshot));

        for (var t = 1; t <= DurationSeconds; t++)
            frames.Add(Capture(t, TickSecond(engine, bySecond, t)));

        return frames;
    }

    public static IReadOnlyList<ScheduledPulse> Schedule(TimelineRequest request)
    {
        var stagger = Clamp(request.StaggerSeconds, 0, MaxStaggerSeconds);
        var firstAt = Clamp(request.FirstAtSecond, 0, DurationSeconds);
        var result = new List<ScheduledPulse>();
        var i = 0;
        foreach (var pulse in request.Pulses)
        {
            if (!IsKnownKind(pulse.Kind))
                throw new ArgumentException($"Unknown OCC kind '{pulse.Kind}'.", nameof(request));
            var at = firstAt + i * stagger;
            if (at > DurationSeconds)
                break;
            result.Add(new ScheduledPulse(pulse.Kind, Clamp01(pulse.Intensity), at));
            i++;
        }

        return result;
    }

    public static bool IsKnownKind(string kind)
    {
        foreach (var option in EventOptions)
        {
            if (option.Kind == kind)
                return true;
        }

        return false;
    }

    public static string ToHtml(IReadOnlyList<Frame>? frames = null) =>
        AlmaTimelinePage.Render(frames ?? Array.Empty<Frame>());

    public static string DefaultHtmlPath()
    {
        var cwd = Directory.GetCurrentDirectory();
        var nested = Path.Combine(cwd, "samples", "AlmaTimeline");
        if (Directory.Exists(nested))
            return Path.Combine(nested, "index.html");
        return Path.Combine(cwd, "index.html");
    }

    private static AffectSnapshot TickSecond(
        AffectEngine engine,
        Dictionary<int, List<ScheduledPulse>> bySecond,
        int t)
    {
        if (!bySecond.TryGetValue(t, out var due) || due.Count == 0)
            return engine.Tick(WorldEvent.Tick, 1f);

        AffectSnapshot snap = engine.Snapshot;
        for (var i = 0; i < due.Count; i++)
        {
            var dt = i == 0 ? 1f : 0f;
            snap = engine.Tick(new WorldEvent(due[i].Kind, due[i].Intensity), dt);
        }

        return snap;
    }

    private static void ApplyPulses(
        AffectEngine engine,
        Dictionary<int, List<ScheduledPulse>> bySecond,
        int t,
        float deltaTime)
    {
        if (!bySecond.TryGetValue(t, out var due) || due.Count == 0)
            return;
        for (var i = 0; i < due.Count; i++)
        {
            var dt = i == 0 ? deltaTime : 0f;
            engine.Tick(new WorldEvent(due[i].Kind, due[i].Intensity), dt);
        }
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

    private static int Clamp(int value, int min, int max) =>
        value < min ? min : value > max ? max : value;

    private static float Clamp01(float value) =>
        value < 0f ? 0f : value > 1f ? 1f : value;

    public sealed record Metric(string Key, string Label, string Color);

    public sealed record OccOption(string Kind, string Label);

    public sealed record OccPulse(string Kind, float Intensity);

    public sealed record ScheduledPulse(string Kind, float Intensity, int AtSecond);

    public sealed record TimelineRequest(
        IReadOnlyList<OccPulse> Pulses,
        int StaggerSeconds,
        int FirstAtSecond = JoyAtSecond);

    public sealed record Frame(int Second, float?[] Values);
}
