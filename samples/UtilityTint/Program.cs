using System.Globalization;
using PersonalityEngine;
using PersonalityEngine.Samples.UtilityTint;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

var engine = UtilityTintHost.CreateEngine();
engine.Tick(WorldEvent.Tick);

Console.WriteLine("Host Utility AI keeps Pick. Personality Engine only tints.");
Dump("Seeded (no pulse)", engine);

engine.Tick(HostEvents.Threat());
Dump("After host threat", engine);

static void Dump(string title, AffectEngine engine)
{
    Console.WriteLine(title);
    Console.WriteLine($"  pick = {UtilityTintHost.Pick(engine)}");
    foreach (var pair in UtilityTintHost.Finals(engine))
        Console.WriteLine($"  {pair.Key} = {pair.Value:0.###}");
    Console.WriteLine();
}
