using PersonalityEngine.Samples.Examples;

if (args.Contains("--serve", StringComparer.Ordinal))
{
    ExamplesServer.Listen();
    return;
}

var path = GameExamples.DefaultHtmlPath();
File.WriteAllText(path, GameExamples.ToHtml());
Console.WriteLine($"Wrote examples page to {path}");
Console.WriteLine("Interactive run: dotnet run --project samples/Examples -- --serve");
