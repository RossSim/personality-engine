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
}
