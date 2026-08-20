using System.Globalization;
using PersonalityEngine;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

var engine = AlmaComposition.Create(OceanTraits.GebhardExample);

engine.Tick(WorldEvent.Tick);
Dump("Seeded from Gebhard OCEAN (mapped baseline = current mood)", engine.Snapshot);

engine.Tick(new WorldEvent(OccEmotion.JoyKind, 1f));
Dump("Host tagged the moment as joy", engine.Snapshot);

engine.Tick(WorldEvent.Tick, deltaTime: 1f);
Dump("One second later (emotion decays; mood pulls toward baseline)", engine.Snapshot);

static void Dump(string title, AffectSnapshot snap)
{
    Console.WriteLine(title);
    Write(snap, OceanToPadMapping.PleasureKey);
    Write(snap, OceanToPadMapping.ArousalKey);
    Write(snap, OceanToPadMapping.DominanceKey);
    Write(snap, PadMood.PleasureKey);
    Write(snap, PadMood.ArousalKey);
    Write(snap, PadMood.DominanceKey);
    Write(snap, OccEmotion.JoyKey);
    Write(snap, OccToPadMapping.PleasureKey);
    Console.WriteLine();
}

static void Write(AffectSnapshot snap, string key)
{
    if (snap.TryGet(key, out var value))
        Console.WriteLine($"  {key} = {value:0.###}");
    else
        Console.WriteLine($"  {key} (absent)");
}
