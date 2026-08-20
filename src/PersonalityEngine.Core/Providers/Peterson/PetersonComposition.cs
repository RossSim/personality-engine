using System.Collections.Generic;
using PersonalityEngine.Providers.Ocean;

namespace PersonalityEngine.Providers.Peterson;

/// <summary>Default composition: OCEAN plus Peterson metatraits and Maps of Meaning.</summary>
public static class PetersonComposition
{
    public static AffectEngine Create(OceanTraits traits, bool includePadMapping = false)
    {
        var providers = new List<IAffectProvider>
        {
            new OceanPersonality(traits),
            new StabilityPlasticityProvider(),
            new OrderChaosMeaningProvider()
        };
        if (includePadMapping)
            providers.Insert(1, new OceanToPadMapping());

        return new AffectEngine(providers, new IActionWeighter[] { new PetersonMeaningWeighter() });
    }
}
