using System.Collections.Generic;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Peterson;

namespace PersonalityEngine.Providers.Piaget;

/// <summary>Cognition layer alone, or composed with OCEAN / Peterson without replacing them.</summary>
public static class PiagetComposition
{
    public static AffectEngine Create(CognitiveStage stage = CognitiveStage.ConcreteOperational)
    {
        return new AffectEngine(
            new IAffectProvider[] { new PiagetEquilibrationProvider(stage) },
            new IActionWeighter[] { new PiagetCognitionWeighter() });
    }

    public static AffectEngine CreateWithOceanAndPeterson(
        OceanTraits traits,
        CognitiveStage stage = CognitiveStage.ConcreteOperational)
    {
        return new AffectEngine(
            new IAffectProvider[]
            {
                new OceanPersonality(traits),
                new StabilityPlasticityProvider(),
                new OrderChaosMeaningProvider(),
                new PiagetEquilibrationProvider(stage)
            },
            new IActionWeighter[]
            {
                new PetersonMeaningWeighter(),
                new PiagetCognitionWeighter()
            });
    }
}
