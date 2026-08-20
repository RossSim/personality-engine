namespace PersonalityEngine.Providers.Ocean;

/// <summary>Big Five scores in 0..1, matching Gebhard ALMA examples.</summary>
public readonly struct OceanTraits
{
    public OceanTraits(float openness, float conscientiousness, float extraversion, float agreeableness, float neuroticism)
    {
        Openness = Clamp01(openness);
        Conscientiousness = Clamp01(conscientiousness);
        Extraversion = Clamp01(extraversion);
        Agreeableness = Clamp01(agreeableness);
        Neuroticism = Clamp01(neuroticism);
    }

    public float Openness { get; }
    public float Conscientiousness { get; }
    public float Extraversion { get; }
    public float Agreeableness { get; }
    public float Neuroticism { get; }

    /// <summary>Gebhard (2005) worked example.</summary>
    public static OceanTraits GebhardExample { get; } =
        new OceanTraits(0.4f, 0.8f, 0.6f, 0.3f, 0.4f);

    public static bool TryRead(AffectSnapshot snapshot, out OceanTraits traits)
    {
        traits = default;
        if (!snapshot.TryGet(OceanPersonality.OpennessKey, out var o)) return false;
        if (!snapshot.TryGet(OceanPersonality.ConscientiousnessKey, out var c)) return false;
        if (!snapshot.TryGet(OceanPersonality.ExtraversionKey, out var e)) return false;
        if (!snapshot.TryGet(OceanPersonality.AgreeablenessKey, out var a)) return false;
        if (!snapshot.TryGet(OceanPersonality.NeuroticismKey, out var n)) return false;
        traits = new OceanTraits(o, c, e, a, n);
        return true;
    }

    private static float Clamp01(float value) =>
        value < 0f ? 0f : value > 1f ? 1f : value;
}
