using System.Collections.Generic;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Peterson;

namespace PersonalityEngine.Providers.Skinner;

/// <summary>Operant layer alone, or composed with OCEAN / Peterson without replacing them.</summary>
public static class SkinnerComposition
{
    public static AffectEngine Create(
        IReadOnlyList<string> actionIds,
        ReinforcementSchedule schedule = ReinforcementSchedule.Continuous,
        int? randomSeed = 1)
    {
        return new AffectEngine(
            new IAffectProvider[] { new OperantLearningProvider(actionIds, schedule, randomSeed) },
            new IActionWeighter[] { new OperantWeighter() });
    }

    public static AffectEngine CreateWithOceanAndPeterson(
        OceanTraits traits,
        IReadOnlyList<string> actionIds,
        ReinforcementSchedule schedule = ReinforcementSchedule.Continuous)
    {
        return new AffectEngine(
            new IAffectProvider[]
            {
                new OceanPersonality(traits),
                new StabilityPlasticityProvider(),
                new OrderChaosMeaningProvider(),
                new OperantLearningProvider(actionIds, schedule, randomSeed: 1)
            },
            new IActionWeighter[]
            {
                new PetersonMeaningWeighter(),
                new OperantWeighter()
            });
    }
}
