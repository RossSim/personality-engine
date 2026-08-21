using PersonalityEngine.Providers.Dyad;
using PersonalityEngine.Providers.Occ;

namespace PersonalityEngine;

/// <summary>
/// Named host events that wrap OCC kinds. Convenience catalog, not an appraisal algorithm.
/// The mapping is a project convention.
/// </summary>
public static class HostEvents
{
    public static WorldEvent NeedMet(float intensity = 1f) =>
        new WorldEvent(OccEmotion.JoyKind, intensity);

    public static WorldEvent Harm(float intensity = 1f) =>
        new WorldEvent(OccEmotion.DistressKind, intensity);

    public static WorldEvent Threat(float intensity = 1f) =>
        new WorldEvent(OccEmotion.FearKind, intensity);

    public static WorldEvent ThreatPassed(float intensity = 1f) =>
        new WorldEvent(OccEmotion.ReliefKind, intensity);

    public static WorldEvent SelfCredit(float intensity = 1f) =>
        new WorldEvent(OccEmotion.PrideKind, intensity);

    public static WorldEvent SelfBlame(float intensity = 1f) =>
        new WorldEvent(OccEmotion.ShameKind, intensity);

    public static WorldEvent HappyFor(string otherId, float intensity = 1f) =>
        new WorldEvent(OccEmotion.HappyForKind, intensity, otherId);

    public static WorldEvent Pity(string otherId, float intensity = 1f) =>
        new WorldEvent(OccEmotion.PityKind, intensity, otherId);

    public static WorldEvent Resent(string otherId, float intensity = 1f) =>
        new WorldEvent(OccEmotion.ResentmentKind, intensity, otherId);

    public static WorldEvent Gloat(string otherId, float intensity = 1f) =>
        new WorldEvent(OccEmotion.GloatingKind, intensity, otherId);

    public static WorldEvent Like(string otherId, float intensity = 1f) =>
        new WorldEvent(DyadProvider.LikeKind, intensity, otherId);

    public static WorldEvent Dislike(string otherId, float intensity = 1f) =>
        new WorldEvent(DyadProvider.DislikeKind, intensity, otherId);

    public static WorldEvent Anger(string otherId, float intensity = 1f) =>
        new WorldEvent(OccEmotion.AngerKind, intensity, otherId);

    public static WorldEvent Gratitude(string otherId, float intensity = 1f) =>
        new WorldEvent(OccEmotion.GratitudeKind, intensity, otherId);

    public static WorldEvent Gratification(float intensity = 1f) =>
        new WorldEvent(OccEmotion.GratificationKind, intensity);

    public static WorldEvent Remorse(float intensity = 1f) =>
        new WorldEvent(OccEmotion.RemorseKind, intensity);
}
