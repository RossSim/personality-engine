using PersonalityEngine.Providers.Dyad;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;
using PersonalityEngine.Providers.Peterson;

namespace PersonalityEngine.Samples.Examples;

/// <summary>
/// Three host stories with real AffectEngine ticks. Sample is a consumer, not a provider.
/// </summary>
public static class GameExamples
{
    public const string PlayerId = "player";
    public const int VisitCount = 8;
    public const float VisitBump = 0.14f;
    public const float VisitGapSeconds = 2f;

    public static readonly string[] RaidActions =
    {
        RaidWeighter.Freeze,
        RaidWeighter.Flee,
        RaidWeighter.Aid
    };

    public static readonly IReadOnlyDictionary<string, float> RaidBases =
        new Dictionary<string, float>
        {
            [RaidWeighter.Freeze] = 0.45f,
            [RaidWeighter.Flee] = 0.45f,
            [RaidWeighter.Aid] = 0.45f
        };

    public static readonly string[] SocialActions =
    {
        DyadWeighter.Approach(PlayerId),
        DyadWeighter.Avoid(PlayerId)
    };

    public static readonly IReadOnlyDictionary<string, float> SocialBases =
        new Dictionary<string, float>
        {
            [DyadWeighter.Approach(PlayerId)] = 0.45f,
            [DyadWeighter.Avoid(PlayerId)] = 0.45f
        };

    public static readonly string[] NationActions =
    {
        PetersonMeaningWeighter.Explore,
        PetersonMeaningWeighter.Defend,
        PetersonMeaningWeighter.Integrate,
        PetersonMeaningWeighter.Withdraw
    };

    public static PersonSeed Mara { get; } = new(
        "Mara",
        "High Agreeableness. The host already decided she is the one who runs toward the downed.",
        new OceanTraits(0.50f, 0.50f, 0.55f, 0.88f, 0.22f));

    public static PersonSeed Joss { get; } = new(
        "Joss",
        "High Neuroticism. Same street, same Threat, freeze wins the tint.",
        new OceanTraits(0.40f, 0.40f, 0.35f, 0.28f, 0.88f));

    public static PersonSeed Cal { get; } = new(
        "Cal",
        "High Conscientiousness, low Neuroticism. Not a medic — flee ranks above aid.",
        new OceanTraits(0.40f, 0.90f, 0.45f, 0.35f, 0.20f));

    public static PersonSeed[] RaidCast { get; } = { Mara, Joss, Cal };

    public static AffectEngine CreateRaidEngine(OceanTraits traits) =>
        AlmaComposition.Create(
            traits,
            weighters: new IActionWeighter[] { new RaidWeighter() });

    public static AffectEngine CreateShopEngine() =>
        DyadComposition.CreateWithAlma(new OceanTraits(0.45f, 0.60f, 0.50f, 0.55f, 0.40f));

    public static RaidStory RunRaid()
    {
        var people = new List<RaidPerson>(RaidCast.Length);
        foreach (var seed in RaidCast)
        {
            var engine = CreateRaidEngine(seed.Traits);
            engine.Tick(WorldEvent.Tick);
            var before = CaptureRaid(engine);
            engine.Tick(HostEvents.Threat(0.8f));
            var after = CaptureRaid(engine);
            people.Add(new RaidPerson(seed.Name, seed.Blurb, before, after));
        }

        return new RaidStory(people);
    }

    public static VisitStory RunVisits(bool cruel)
    {
        var engine = CreateShopEngine();
        engine.Tick(WorldEvent.Tick);
        var frames = new List<VisitFrame>(VisitCount);
        for (var i = 0; i < VisitCount; i++)
        {
            if (cruel)
            {
                engine.Tick(HostEvents.Dislike(PlayerId, VisitBump));
                engine.Tick(HostEvents.Anger(PlayerId));
                engine.Tick(HostEvents.Harm());
            }
            else
            {
                engine.Tick(HostEvents.Like(PlayerId, VisitBump));
                engine.Tick(HostEvents.Gratitude(PlayerId));
                engine.Tick(HostEvents.NeedMet());
            }

            frames.Add(CaptureVisit(engine, i + 1));
            engine.Tick(VisitGapSeconds);
        }

        return new VisitStory(cruel, frames);
    }

    public static ScaleStory RunScale()
    {
        var nation = PetersonComposition.Create(OceanTraits.GebhardExample);
        nation.Tick(WorldEvent.Tick);
        var village = AlmaComposition.Create(OceanTraits.GebhardExample);
        village.Tick(WorldEvent.Tick);
        var priest = DyadComposition.CreateWithAlma(new OceanTraits(0.50f, 0.70f, 0.40f, 0.60f, 0.35f));
        priest.Tick(WorldEvent.Tick);

        var before = new ScaleFrame(
            "Quiet map",
            CaptureNation(nation),
            CaptureVillage(village),
            CapturePriest(priest));

        nation.Tick(new WorldEvent(OrderChaosMeaningProvider.AnomalyKind, 0.8f));
        var afterNation = new ScaleFrame(
            "Nation tagged the desecration as an anomaly",
            CaptureNation(nation),
            before.Village,
            before.Priest);

        village.Tick(HostEvents.Harm());
        var afterVillage = new ScaleFrame(
            "Village mood took Harm",
            afterNation.Nation,
            CaptureVillage(village),
            before.Priest);

        priest.Tick(HostEvents.Anger(PlayerId));
        var afterPriest = new ScaleFrame(
            "Named priest tagged Anger at the player",
            afterNation.Nation,
            afterVillage.Village,
            CapturePriest(priest));

        return new ScaleStory(new[] { before, afterNation, afterVillage, afterPriest });
    }

    public static string ToHtml() => ExamplesPage.Render(new ExampleBundle(
        RunRaid(),
        RunVisits(cruel: false),
        RunVisits(cruel: true),
        RunScale()));

    public static string DefaultHtmlPath()
    {
        var cwd = Directory.GetCurrentDirectory();
        var nested = Path.Combine(cwd, "samples", "Examples");
        if (Directory.Exists(nested))
            return Path.Combine(nested, "index.html");
        return Path.Combine(cwd, "index.html");
    }

    private static RaidFrame CaptureRaid(AffectEngine engine)
    {
        var snap = engine.Snapshot;
        var finals = HostChooser.Combine(RaidBases, engine.WeightActions(RaidActions));
        return new RaidFrame(
            snap.GetOrDefault(OccEmotion.FearKey),
            snap.GetOrDefault(PadMood.PleasureKey),
            snap.GetOrDefault(PadMood.ArousalKey),
            finals[RaidWeighter.Freeze],
            finals[RaidWeighter.Flee],
            finals[RaidWeighter.Aid],
            HostChooser.Pick(finals));
    }

    private static VisitFrame CaptureVisit(AffectEngine engine, int visit)
    {
        var snap = engine.Snapshot;
        var finals = HostChooser.Combine(SocialBases, engine.WeightActions(SocialActions));
        var approach = DyadWeighter.Approach(PlayerId);
        var avoid = DyadWeighter.Avoid(PlayerId);
        return new VisitFrame(
            visit,
            snap.GetOrDefault(DyadProvider.LikingKey(PlayerId)),
            snap.GetOrDefault(PadMood.PleasureKey),
            snap.GetOrDefault(OccEmotion.AngerKey),
            snap.GetOrDefault(OccEmotion.GratitudeKey),
            finals[approach],
            finals[avoid],
            HostChooser.Pick(finals));
    }

    private static NationSnap CaptureNation(AffectEngine engine)
    {
        var snap = engine.Snapshot;
        var tints = engine.WeightActions(NationActions);
        var scored = new Dictionary<string, float>();
        foreach (var id in NationActions)
            scored[id] = tints.GetValueOrDefault(id);
        return new NationSnap(
            snap.GetOrDefault(OrderChaosMeaningProvider.ChaosKey),
            snap.GetOrDefault(OrderChaosMeaningProvider.OrderKey),
            HostChooser.Pick(scored),
            scored);
    }

    private static VillageSnap CaptureVillage(AffectEngine engine)
    {
        var snap = engine.Snapshot;
        return new VillageSnap(
            snap.GetOrDefault(PadMood.PleasureKey),
            snap.GetOrDefault(PadMood.ArousalKey));
    }

    private static PriestSnap CapturePriest(AffectEngine engine)
    {
        var snap = engine.Snapshot;
        return new PriestSnap(
            snap.GetOrDefault(OccEmotion.AngerKey),
            snap.GetOrDefault(DyadProvider.LikingKey(PlayerId)));
    }

    public sealed record PersonSeed(string Name, string Blurb, OceanTraits Traits);

    public sealed record RaidFrame(
        float Fear,
        float Pleasure,
        float Arousal,
        float Freeze,
        float Flee,
        float Aid,
        string Pick);

    public sealed record RaidPerson(string Name, string Blurb, RaidFrame Before, RaidFrame After);

    public sealed record RaidStory(IReadOnlyList<RaidPerson> People);

    public sealed record VisitFrame(
        int Visit,
        float Liking,
        float Pleasure,
        float Anger,
        float Gratitude,
        float Approach,
        float Avoid,
        string Pick);

    public sealed record VisitStory(bool Cruel, IReadOnlyList<VisitFrame> Frames);

    public sealed record NationSnap(
        float Chaos,
        float Order,
        string Pick,
        IReadOnlyDictionary<string, float> Scores);

    public sealed record VillageSnap(float Pleasure, float Arousal);

    public sealed record PriestSnap(float Anger, float Liking);

    public sealed record ScaleFrame(
        string Caption,
        NationSnap Nation,
        VillageSnap Village,
        PriestSnap Priest);

    public sealed record ScaleStory(IReadOnlyList<ScaleFrame> Frames);

    public sealed record ExampleBundle(
        RaidStory Raid,
        VisitStory Kind,
        VisitStory Cruel,
        ScaleStory Scale);
}
