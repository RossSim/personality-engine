using PersonalityEngine.Providers.Ocean;

namespace PersonalityEngine.Providers.Pad;

/// <summary>
/// Default ALMA-style stack without OCC: OCEAN, mapped PAD baseline, current mood dynamics.
/// </summary>
public static class AlmaComposition
{
    public static AffectEngine Create(OceanTraits traits, float decayRate = 0.5f)
    {
        return new AffectEngine(new IAffectProvider[]
        {
            new OceanPersonality(traits),
            new OceanToPadMapping(),
            new PadMood { DecayRate = decayRate }
        });
    }
}
