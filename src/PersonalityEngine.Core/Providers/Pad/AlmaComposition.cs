using System.Collections.Generic;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;

namespace PersonalityEngine.Providers.Pad;

/// <summary>
/// Default ALMA-style stack: OCEAN, mapped PAD baseline, OCC appraisal, OCC→PAD overlay, mood dynamics.
/// OCC providers are optional; omit them and mood stays mapping + decay only.
/// </summary>
public static class AlmaComposition
{
    public static AffectEngine Create(
        OceanTraits traits,
        float decayRate = 0.5f,
        bool includeOcc = true,
        IEnumerable<IActionWeighter>? weighters = null)
    {
        if (!includeOcc)
        {
            return new AffectEngine(
                new IAffectProvider[]
                {
                    new OceanPersonality(traits),
                    new OceanToPadMapping(),
                    new PadMood { DecayRate = decayRate }
                },
                weighters);
        }

        return new AffectEngine(
            new IAffectProvider[]
            {
                new OceanPersonality(traits),
                new OceanToPadMapping(),
                new OccEmotion(),
                new OccToPadMapping(),
                new PadMood { DecayRate = decayRate }
            },
            weighters);
    }
}
