using PersonalityEngine.Samples.AlmaTimeline;

var frames = AlmaTimeline.Run();
var path = AlmaTimeline.DefaultHtmlPath();
File.WriteAllText(path, AlmaTimeline.ToHtml(frames));
Console.WriteLine($"Wrote {frames.Count} ticks (0..{AlmaTimeline.DurationSeconds}s) to {path}");
