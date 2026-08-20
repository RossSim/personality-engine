using System.Collections.Generic;

namespace PersonalityEngine;

/// <summary>Typed host event. Providers match on <see cref="Kind"/>; unknown kinds are ignored.</summary>
public sealed class WorldEvent
{
    public static WorldEvent Tick { get; } = new WorldEvent("tick", 0f);

    public WorldEvent(
        string kind,
        float intensity = 1f,
        string? target = null,
        IReadOnlyDictionary<string, float>? payload = null)
    {
        Kind = kind;
        Intensity = Clamp01(intensity);
        Target = target;
        Payload = payload ?? Empty;
    }

    public string Kind { get; }
    public float Intensity { get; }
    /// <summary>Optional action or stimulus id (used by operant events).</summary>
    public string? Target { get; }
    public IReadOnlyDictionary<string, float> Payload { get; }

    private static readonly IReadOnlyDictionary<string, float> Empty =
        new Dictionary<string, float>();

    internal static float Clamp01(float value) =>
        value < 0f ? 0f : value > 1f ? 1f : value;
}
