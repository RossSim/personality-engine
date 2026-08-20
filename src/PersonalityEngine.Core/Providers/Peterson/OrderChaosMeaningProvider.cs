using System.Collections.Generic;

namespace PersonalityEngine.Providers.Peterson;

/// <summary>
/// Maps of Meaning / Complexity Management: known (order), unknown (chaos), knower (logos),
/// and defensive rigidity when anomaly appears.
/// </summary>
public sealed class OrderChaosMeaningProvider : IAffectProvider
{
    public const string ProviderId = "peterson-maps";

    public const string AnomalyKind = "peterson.anomaly";
    public const string VoluntaryExploreKind = "peterson.voluntary-explore";
    public const string IntegrateKind = "peterson.integrate";
    public const string DefendBeliefKind = "peterson.defend-belief";
    public const string ConfirmMapKind = "peterson.confirm-map";

    public static readonly string OrderKey = ChannelKey.Of(AffectLayer.Meaning, ProviderId, "order");
    public static readonly string ChaosKey = ChannelKey.Of(AffectLayer.Meaning, ProviderId, "chaos");
    public static readonly string LogosKey = ChannelKey.Of(AffectLayer.Meaning, ProviderId, "logos");
    public static readonly string RigidityKey = ChannelKey.Of(AffectLayer.Meaning, ProviderId, "rigidity");
    public static readonly string KnownMeaningKey = ChannelKey.Of(AffectLayer.Meaning, ProviderId, "meaning-known");
    public static readonly string ChaosMeaningKey = ChannelKey.Of(AffectLayer.Meaning, ProviderId, "meaning-chaos");
    public static readonly string ExploreMeaningKey = ChannelKey.Of(AffectLayer.Meaning, ProviderId, "meaning-exploration");

    public string Id => ProviderId;
    public string Layer => AffectLayer.Meaning;

    public Citation Citation { get; } = PetersonCitations.MapsOfMeaning1999;

    public IReadOnlyList<Citation> AdditionalCitations { get; } =
        new[]
        {
            PetersonCitations.ComplexityManagement2002,
            PetersonCitations.ThreeFormsOfMeaning2013,
            PetersonCitations.MeaningDynamics
        };

    public float AnomalyChaosGain { get; init; } = 0.55f;
    public float AnomalyOrderLoss { get; init; } = 0.35f;
    public float ExploreLogosGain { get; init; } = 0.40f;
    public float IntegrateOrderGain { get; init; } = 0.45f;
    public float IntegrateChaosLoss { get; init; } = 0.50f;
    public float DefendRigidityGain { get; init; } = 0.50f;
    public float DefendChaosSuppression { get; init; } = 0.25f;
    public float ConfirmOrderGain { get; init; } = 0.12f;
    public float ChaosDecayPerSecond { get; init; } = 0.08f;
    public float RigidityDecayPerSecond { get; init; } = 0.05f;

    public AffectDelta Contribute(WorldEvent ev, float deltaTime, AffectSnapshot snapshot)
    {
        var order = snapshot.GetOrDefault(OrderKey, 0.70f);
        var chaos = snapshot.GetOrDefault(ChaosKey, 0.20f);
        var logos = snapshot.GetOrDefault(LogosKey, 0.50f);
        var rigidity = snapshot.GetOrDefault(RigidityKey, 0.20f);

        TryPersonalityBias(snapshot, out var stability, out var plasticity);

        switch (ev.Kind)
        {
            case AnomalyKind:
                chaos = Clamp01(chaos + ev.Intensity * AnomalyChaosGain);
                order = Clamp01(order - ev.Intensity * AnomalyOrderLoss);
                // CMT: anomaly can produce rigidity or voluntary reconstrual.
                rigidity = Clamp01(rigidity + ev.Intensity * stability * 0.35f);
                logos = Clamp01(logos + ev.Intensity * plasticity * 0.20f);
                break;
            case VoluntaryExploreKind:
                logos = Clamp01(logos + ev.Intensity * ExploreLogosGain);
                rigidity = Clamp01(rigidity - ev.Intensity * 0.20f);
                break;
            case IntegrateKind:
                order = Clamp01(order + ev.Intensity * IntegrateOrderGain);
                chaos = Clamp01(chaos - ev.Intensity * IntegrateChaosLoss);
                break;
            case DefendBeliefKind:
                rigidity = Clamp01(rigidity + ev.Intensity * DefendRigidityGain);
                chaos = Clamp01(chaos - ev.Intensity * DefendChaosSuppression);
                logos = Clamp01(logos - ev.Intensity * 0.15f);
                break;
            case ConfirmMapKind:
                order = Clamp01(order + ev.Intensity * ConfirmOrderGain);
                chaos = Clamp01(chaos - ev.Intensity * 0.08f);
                break;
            default:
                chaos = DecayToward(chaos, 0.20f, ChaosDecayPerSecond, deltaTime);
                rigidity = DecayToward(rigidity, stability * 0.3f + 0.05f, RigidityDecayPerSecond, deltaTime);
                break;
        }

        // Three forms of meaning (Peterson, 2013): known, chaos, exploration.
        var meaningKnown = order * (1f - chaos);
        var meaningChaos = chaos;
        var meaningExplore = logos * chaos;

        return new AffectDelta()
            .Set(OrderKey, order)
            .Set(ChaosKey, chaos)
            .Set(LogosKey, logos)
            .Set(RigidityKey, rigidity)
            .Set(KnownMeaningKey, meaningKnown)
            .Set(ChaosMeaningKey, meaningChaos)
            .Set(ExploreMeaningKey, meaningExplore);
    }

    private static void TryPersonalityBias(AffectSnapshot snapshot, out float stability, out float plasticity)
    {
        stability = snapshot.TryGet(StabilityPlasticityProvider.StabilityKey, out var s) ? s : 0.5f;
        plasticity = snapshot.TryGet(StabilityPlasticityProvider.PlasticityKey, out var p) ? p : 0.5f;
    }

    private static float DecayToward(float current, float target, float rate, float dt)
    {
        if (dt <= 0f || rate <= 0f)
            return current;
        var t = Clamp01(rate * dt);
        return current + (target - current) * t;
    }

    private static float Clamp01(float value) =>
        value < 0f ? 0f : value > 1f ? 1f : value;
}
