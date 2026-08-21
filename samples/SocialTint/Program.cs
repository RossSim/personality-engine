using System.Globalization;
using PersonalityEngine;
using PersonalityEngine.Samples.SocialTint;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

var engine = SocialTintHost.CreateEngine();
engine.Tick(WorldEvent.Tick);

Console.WriteLine("Host Utility AI keeps Pick. Personality Engine tints approach/avoid.");
Dump("Seeded (host prefers avoid:rival)", engine);

engine.Tick(HostEvents.Like(SocialTintHost.Ally));
Dump("After like ally", engine);

engine.Tick(HostEvents.HappyFor(SocialTintHost.Ally));
Dump("After happy-for ally", engine);

static void Dump(string title, AffectEngine engine)
{
    Console.WriteLine(title);
    Console.WriteLine($"  pick = {SocialTintHost.Pick(engine)}");
    foreach (var pair in SocialTintHost.Finals(engine))
        Console.WriteLine($"  {pair.Key} = {pair.Value:0.###}");
    Console.WriteLine();
}
