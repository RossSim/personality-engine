using System.Collections.Generic;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Peterson;

namespace PersonalityEngine.Providers.Erikson;

/// <summary>Identity layer alone, or composed with OCEAN / Peterson without replacing them.</summary>
public static class EriksonComposition
{
    public static AffectEngine Create(PsychosocialStage stage = PsychosocialStage.IdentityVsRoleConfusion)
    {
        return new AffectEngine(
            new IAffectProvider[] { new EriksonPsychosocialProvider(stage) },
            new IActionWeighter[] { new EriksonIdentityWeighter() });
    }

    public static AffectEngine CreateWithOceanAndPeterson(
        OceanTraits traits,
        PsychosocialStage stage = PsychosocialStage.IdentityVsRoleConfusion)
    {
        return new AffectEngine(
            new IAffectProvider[]
            {
                new OceanPersonality(traits),
                new StabilityPlasticityProvider(),
                new OrderChaosMeaningProvider(),
                new EriksonPsychosocialProvider(stage)
            },
            new IActionWeighter[]
            {
                new PetersonMeaningWeighter(),
                new EriksonIdentityWeighter()
            });
    }
}
