using System.Collections.Generic;

namespace PersonalityEngine.Providers.Erikson;

/// <summary>
/// Psychosocial crises resolve as a syntonic/dystonic ratio; ego identity is the through-line.
/// Erikson (1963, 1968). Stage is host-set and is not advanced from events.
/// </summary>
public sealed class EriksonPsychosocialProvider : IAffectProvider
{
    public const string ProviderId = "erikson-psychosocial";

    public const string ChallengeKind = "erikson.challenge";
    public const string SupportKind = "erikson.support";
    public const string RuptureKind = "erikson.rupture";
    public const string ExploreKind = "erikson.explore";
    public const string CommitKind = "erikson.commit";
    public const string NegativeIdentityKind = "erikson.negative-identity";
    public const string GenerateKind = "erikson.generate";
    public const string ReviewKind = "erikson.review";

    public static readonly string StageIndexKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "stage-index");
    public static readonly string StageProgressKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "stage-progress");
    public static readonly string SyntonicKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "syntonic");
    public static readonly string DystonicKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "dystonic");
    public static readonly string RatioKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "ratio");
    public static readonly string VirtueKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "virtue");
    public static readonly string EgoIdentityKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "ego-identity");
    public static readonly string RoleConfusionKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "role-confusion");
    public static readonly string FidelityKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "fidelity");
    public static readonly string MoratoriumKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "moratorium");
    public static readonly string NegativeIdentityKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "negative-identity");
    public static readonly string IdentityCrisisKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "identity-crisis");
    public static readonly string GenerativityKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "generativity");
    public static readonly string StagnationKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "stagnation");
    public static readonly string IntegrityKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "integrity");
    public static readonly string DespairKey = ChannelKey.Of(AffectLayer.Identity, ProviderId, "despair");

    private float _syntonic = 0.55f;
    private float _dystonic = 0.45f;
    private float _egoIdentity;
    private float _roleConfusion;
    private float _fidelity;
    private float _moratorium;
    private float _negativeIdentity = 0.10f;
    private float _generativity;
    private float _integrity;

    public EriksonPsychosocialProvider(PsychosocialStage stage = PsychosocialStage.IdentityVsRoleConfusion)
    {
        Stage = stage;
        _egoIdentity = EgoIdentitySeed(stage);
        _roleConfusion = RoleConfusionSeed(stage);
        _fidelity = FidelitySeed(stage);
        _moratorium = MoratoriumSeed(stage);
        _generativity = stage >= PsychosocialStage.GenerativityVsStagnation ? 0.40f : 0f;
        _integrity = stage >= PsychosocialStage.IntegrityVsDespair ? 0.40f : 0f;
    }

    public string Id => ProviderId;
    public string Layer => AffectLayer.Identity;
    public PsychosocialStage Stage { get; }

    /// <summary>How hard a crisis challenge feeds the dystonic pole. Project convention.</summary>
    public float ChallengeGain { get; init; } = 0.35f;

    /// <summary>How completely support shifts the syntonic/dystonic ratio. Project convention.</summary>
    public float SupportGain { get; init; } = 0.30f;

    /// <summary>How hard a rupture feeds role confusion and the dystonic pole. Project convention.</summary>
    public float RuptureGain { get; init; } = 0.40f;

    /// <summary>Identity-commitment gain on <c>erikson.commit</c>. Project convention.</summary>
    public float CommitGain { get; init; } = 0.40f;

    public Citation Citation { get; } = EriksonCitations.ChildhoodAndSociety1963;

    public IReadOnlyList<Citation> AdditionalCitations { get; } =
        new[]
        {
            EriksonCitations.IdentityAndTheLifeCycle1959,
            EriksonCitations.IdentityYouthAndCrisis1968,
            EriksonCitations.LifeCycleCompleted1982,
            EriksonCitations.YoungManLuther1958,
            EriksonCitations.DynamicsAndStageFlags
        };

    public AffectDelta Contribute(WorldEvent ev, float deltaTime, AffectSnapshot snapshot)
    {
        switch (ev.Kind)
        {
            case ChallengeKind:
                HandleChallenge(ev.Intensity);
                break;
            case SupportKind:
                HandleSupport(ev.Intensity);
                break;
            case RuptureKind:
                HandleRupture(ev.Intensity);
                break;
            case ExploreKind:
                HandleExplore(ev.Intensity);
                break;
            case CommitKind:
                HandleCommit(ev.Intensity);
                break;
            case NegativeIdentityKind:
                HandleNegativeIdentity(ev.Intensity);
                break;
            case GenerateKind:
                HandleGenerate(ev.Intensity);
                break;
            case ReviewKind:
                HandleReview(ev.Intensity);
                break;
        }

        return Write();
    }

    private void HandleChallenge(float intensity)
    {
        _dystonic = Clamp01(_dystonic + intensity * ChallengeGain);
        _syntonic = Clamp01(_syntonic - intensity * ChallengeGain * 0.25f);
        _roleConfusion = Clamp01(_roleConfusion + intensity * 0.25f);
        if (Stage == PsychosocialStage.IdentityVsRoleConfusion)
            _moratorium = Clamp01(_moratorium + intensity * 0.20f);
    }

    private void HandleSupport(float intensity)
    {
        _syntonic = Clamp01(_syntonic + intensity * SupportGain);
        _dystonic = Clamp01(_dystonic - intensity * SupportGain * 0.50f);
        _egoIdentity = Clamp01(_egoIdentity + intensity * 0.15f);
        if (Stage >= PsychosocialStage.IdentityVsRoleConfusion)
            _fidelity = Clamp01(_fidelity + intensity * 0.10f);
        if (Stage >= PsychosocialStage.GenerativityVsStagnation)
            _generativity = Clamp01(_generativity + intensity * 0.10f);
        if (Stage >= PsychosocialStage.IntegrityVsDespair)
            _integrity = Clamp01(_integrity + intensity * 0.10f);
    }

    private void HandleRupture(float intensity)
    {
        _dystonic = Clamp01(_dystonic + intensity * RuptureGain);
        _syntonic = Clamp01(_syntonic - intensity * RuptureGain * 0.45f);
        _roleConfusion = Clamp01(_roleConfusion + intensity * 0.35f);
        _egoIdentity = Clamp01(_egoIdentity - intensity * 0.12f);
    }

    private void HandleExplore(float intensity)
    {
        _moratorium = Clamp01(_moratorium + intensity * 0.40f);
        _roleConfusion = Clamp01(_roleConfusion + intensity * 0.10f);
        _egoIdentity = Clamp01(_egoIdentity + intensity * 0.05f);
        _negativeIdentity = Clamp01(_negativeIdentity - intensity * 0.05f);
    }

    private void HandleCommit(float intensity)
    {
        _egoIdentity = Clamp01(_egoIdentity + intensity * CommitGain);
        _fidelity = Clamp01(_fidelity + intensity * CommitGain);
        _moratorium = Clamp01(_moratorium * (1f - 0.70f * intensity));
        _roleConfusion = Clamp01(_roleConfusion - intensity * 0.35f);
        _syntonic = Clamp01(_syntonic + intensity * 0.15f);
    }

    private void HandleNegativeIdentity(float intensity)
    {
        _negativeIdentity = Clamp01(_negativeIdentity + intensity * 0.45f);
        _egoIdentity = Clamp01(_egoIdentity + intensity * 0.20f);
        _fidelity = Clamp01(_fidelity - intensity * 0.20f);
        _roleConfusion = Clamp01(_roleConfusion - intensity * 0.10f);
        _moratorium = Clamp01(_moratorium * (1f - 0.40f * intensity));
    }

    private void HandleGenerate(float intensity)
    {
        if (Stage < PsychosocialStage.GenerativityVsStagnation)
            return;
        _generativity = Clamp01(_generativity + intensity * 0.30f);
        _syntonic = Clamp01(_syntonic + intensity * 0.10f);
    }

    private void HandleReview(float intensity)
    {
        if (Stage < PsychosocialStage.IntegrityVsDespair)
            return;
        var ratio = Ratio();
        _integrity = Clamp01(_integrity + intensity * 0.35f * ratio);
        _syntonic = Clamp01(_syntonic + intensity * 0.10f * ratio);
        _dystonic = Clamp01(_dystonic + intensity * 0.10f * (1f - ratio));
    }

    private AffectDelta Write()
    {
        var index = (int)Stage;
        var ratio = Ratio();
        var generativity = Stage >= PsychosocialStage.GenerativityVsStagnation ? _generativity : 0f;
        var integrity = Stage >= PsychosocialStage.IntegrityVsDespair ? _integrity : 0f;
        return new AffectDelta()
            .Set(StageIndexKey, index)
            .Set(StageProgressKey, index / 7f)
            .Set(SyntonicKey, _syntonic)
            .Set(DystonicKey, _dystonic)
            .Set(RatioKey, ratio)
            .Set(VirtueKey, ratio)
            .Set(EgoIdentityKey, _egoIdentity)
            .Set(RoleConfusionKey, _roleConfusion)
            .Set(FidelityKey, _fidelity)
            .Set(MoratoriumKey, _moratorium)
            .Set(NegativeIdentityKey, _negativeIdentity)
            .Set(IdentityCrisisKey, Stage == PsychosocialStage.IdentityVsRoleConfusion ? 1f : 0f)
            .Set(GenerativityKey, generativity)
            .Set(StagnationKey, Stage >= PsychosocialStage.GenerativityVsStagnation ? 1f - generativity : 0f)
            .Set(IntegrityKey, integrity)
            .Set(DespairKey, Stage >= PsychosocialStage.IntegrityVsDespair ? 1f - integrity : 0f);
    }

    private float Ratio()
    {
        var sum = _syntonic + _dystonic;
        return sum <= 0f ? 0.5f : _syntonic / sum;
    }

    private static float EgoIdentitySeed(PsychosocialStage stage) =>
        stage switch
        {
            PsychosocialStage.IdentityVsRoleConfusion => 0.40f,
            _ when stage > PsychosocialStage.IdentityVsRoleConfusion => 0.60f,
            _ => 0.30f
        };

    private static float RoleConfusionSeed(PsychosocialStage stage) =>
        stage == PsychosocialStage.IdentityVsRoleConfusion ? 0.45f : 0.25f;

    private static float FidelitySeed(PsychosocialStage stage) =>
        stage switch
        {
            PsychosocialStage.IdentityVsRoleConfusion => 0.35f,
            _ when stage > PsychosocialStage.IdentityVsRoleConfusion => 0.55f,
            _ => 0.15f
        };

    private static float MoratoriumSeed(PsychosocialStage stage) =>
        stage == PsychosocialStage.IdentityVsRoleConfusion ? 0.55f : 0.10f;

    private static float Clamp01(float value) =>
        value < 0f ? 0f : value > 1f ? 1f : value;
}
