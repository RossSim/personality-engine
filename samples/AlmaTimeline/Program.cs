using PersonalityEngine.Samples.AlmaTimeline;

if (args.Contains("--serve", StringComparer.Ordinal))
{
    AlmaTimelineServer.Listen();
    return;
}

var path = AlmaTimeline.DefaultHtmlPath();
File.WriteAllText(path, AlmaTimeline.ToHtml());
Console.WriteLine($"Wrote timeline page to {path}");
Console.WriteLine("Interactive run: dotnet run --project samples/AlmaTimeline -- --serve");
