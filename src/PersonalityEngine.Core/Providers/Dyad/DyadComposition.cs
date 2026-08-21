using System.Collections.Generic;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;

namespace PersonalityEngine.Providers.Dyad;

/// <summary>Relationship layer alone, or beside the default ALMA stack. Off by default.</summary>
public static class DyadComposition
{
    public static AffectEngine Create()
    {
        return new AffectEngine(
            new IAffectProvider[] { new DyadProvider() },
            new IActionWeighter[] { new DyadWeighter() });
    }

    public static AffectEngine CreateWithAlma(OceanTraits traits)
    {
        return new AffectEngine(
            new IAffectProvider[]
            {
                new OceanPersonality(traits),
                new OceanToPadMapping(),
                new OccEmotion(),
                new OccToPadMapping(),
                new PadMood(),
                new DyadProvider()
            },
            new IActionWeighter[] { new DyadWeighter() });
    }
}
