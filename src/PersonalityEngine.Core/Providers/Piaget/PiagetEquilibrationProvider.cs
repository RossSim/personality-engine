using System.Collections.Generic;

namespace PersonalityEngine.Providers.Piaget;

/// <summary>
/// Schemas adapt by assimilation and accommodation; equilibration restores balance.
/// Piaget (1950, 1985). Stage is host-set and is not advanced from events.
/// </summary>
public sealed class PiagetEquilibrationProvider : IAffectProvider
{
    public const string ProviderId = "piaget-equilibration";

    public const string EncounterKind = "piaget.encounter";
    public const string AssimilateKind = "piaget.assimilate";
    public const string AccommodateKind = "piaget.accommodate";
    public const string EquilibrateKind = "piaget.equilibrate";

    public static readonly string StageIndexKey = ChannelKey.Of(AffectLayer.Cognition, ProviderId, "stage-index");
    public static readonly string StageProgressKey = ChannelKey.Of(AffectLayer.Cognition, ProviderId, "stage-progress");
    public static readonly string EquilibriumKey = ChannelKey.Of(AffectLayer.Cognition, ProviderId, "equilibrium");
    public static readonly string DisequilibriumKey = ChannelKey.Of(AffectLayer.Cognition, ProviderId, "disequilibrium");
    public static readonly string AssimilationKey = ChannelKey.Of(AffectLayer.Cognition, ProviderId, "assimilation");
    public static readonly string AccommodationKey = ChannelKey.Of(AffectLayer.Cognition, ProviderId, "accommodation");
    public static readonly string ObjectPermanenceKey = ChannelKey.Of(AffectLayer.Cognition, ProviderId, "object-permanence");
    public static readonly string ConservationKey = ChannelKey.Of(AffectLayer.Cognition, ProviderId, "conservation");
    public static readonly string EgocentrismKey = ChannelKey.Of(AffectLayer.Cognition, ProviderId, "egocentrism");
    public static readonly string HypotheticalKey = ChannelKey.Of(AffectLayer.Cognition, ProviderId, "hypothetical");

    private float _equilibrium = 1f;
    private float _assimilation = 0.5f;
    private float _accommodation = 0.3f;
    private float _objectPermanence;

    public PiagetEquilibrationProvider(CognitiveStage stage = CognitiveStage.ConcreteOperational)
    {
        Stage = stage;
        _objectPermanence = stage == CognitiveStage.Sensorimotor ? 0f : 1f;
    }

    public string Id => ProviderId;
    public string Layer => AffectLayer.Cognition;
    public CognitiveStage Stage { get; }

    /// <summary>Misfit at or below this is treated as assimilable. Project convention.</summary>
    public float AssimilateMisfitCeiling { get; init; } = 0.40f;

    /// <summary>How hard a misfitting encounter knocks equilibrium. Project convention.</summary>
    public float EncounterGain { get; init; } = 0.85f;

    /// <summary>How completely an accommodate/equilibrate event restores balance. Project convention.</summary>
    public float RestoreGain { get; init; } = 1f;

    /// <summary>Slow pull toward a resting equilibrium when nothing happens. Project convention.</summary>
    public float DriftPerSecond { get; init; } = 0.05f;

    public Citation Citation { get; } = PiagetCitations.PsychologyOfIntelligence1950;

    public IReadOnlyList<Citation> AdditionalCitations { get; } =
        new[]
        {
            PiagetCitations.OriginsOfIntelligence1952,
            PiagetCitations.ConstructionOfReality1954,
            PiagetCitations.Equilibration1985,
            PiagetCitations.InhelderPiaget1958,
            PiagetCitations.GeneticEpistemology1970,
            PiagetCitations.DynamicsAndStageFlags
        };

    public AffectDelta Contribute(WorldEvent ev, float deltaTime, AffectSnapshot snapshot)
    {
        switch (ev.Kind)
        {
            case EncounterKind:
                HandleEncounter(ev.Intensity);
                break;
            case AssimilateKind:
                Assimilate(ev.Intensity);
                break;
            case AccommodateKind:
                Accommodate(ev.Intensity);
                break;
            case EquilibrateKind:
                _equilibrium = Clamp01(_equilibrium + ev.Intensity * RestoreGain * (1f - _equilibrium));
                break;
            default:
                if (deltaTime > 0f)
                    _equilibrium = Clamp01(_equilibrium + (0.7f - _equilibrium) * DriftPerSecond * deltaTime);
                break;
        }

        return Write();
    }

    private void HandleEncounter(float misfit)
    {
        if (misfit <= AssimilateMisfitCeiling)
            Assimilate(1f - misfit);
        else
        {
            _equilibrium = Clamp01(_equilibrium - misfit * EncounterGain);
            _accommodation = Clamp01(_accommodation + misfit * 0.45f);
            _assimilation = Clamp01(_assimilation - misfit * 0.28f);
        }
    }

    private void Assimilate(float intensity)
    {
        _assimilation = Clamp01(_assimilation + intensity * 0.25f);
        _equilibrium = Clamp01(_equilibrium + intensity * 0.15f);
        _accommodation = Clamp01(_accommodation - intensity * 0.08f);
        if (Stage == CognitiveStage.Sensorimotor)
            _objectPermanence = Clamp01(_objectPermanence + intensity * 0.05f);
    }

    private void Accommodate(float intensity)
    {
        _accommodation = Clamp01(_accommodation + intensity * 0.30f);
        _equilibrium = Clamp01(_equilibrium + intensity * RestoreGain * (1f - _equilibrium));
        _assimilation = Clamp01(_assimilation * 0.85f + 0.15f);
        if (Stage == CognitiveStage.Sensorimotor)
            _objectPermanence = Clamp01(_objectPermanence + intensity * 0.12f);
    }

    private AffectDelta Write()
    {
        var index = (int)Stage;
        return new AffectDelta()
            .Set(StageIndexKey, index)
            .Set(StageProgressKey, index / 3f)
            .Set(EquilibriumKey, _equilibrium)
            .Set(DisequilibriumKey, 1f - _equilibrium)
            .Set(AssimilationKey, _assimilation)
            .Set(AccommodationKey, _accommodation)
            .Set(ObjectPermanenceKey, _objectPermanence)
            .Set(ConservationKey, Stage >= CognitiveStage.ConcreteOperational ? 1f : 0f)
            .Set(EgocentrismKey, EgocentrismFor(Stage))
            .Set(HypotheticalKey, Stage >= CognitiveStage.FormalOperational ? 1f : 0f);
    }

    /// <summary>Project-convention scalars, not measured perspective-taking error rates.</summary>
    private static float EgocentrismFor(CognitiveStage stage) =>
        stage switch
        {
            CognitiveStage.Sensorimotor => 0.85f,
            CognitiveStage.Preoperational => 0.70f,
            CognitiveStage.ConcreteOperational => 0.20f,
            CognitiveStage.FormalOperational => 0.05f,
            _ => 0.20f
        };

    private static float Clamp01(float value) =>
        value < 0f ? 0f : value > 1f ? 1f : value;
}
