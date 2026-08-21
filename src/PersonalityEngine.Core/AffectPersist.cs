using System.Collections.Generic;

namespace PersonalityEngine;

/// <summary>
/// Versioned host-save bag. Core does not depend on a JSON library;
/// hosts serialize this type (JSON is fine).
/// </summary>
public sealed class AffectPersist
{
    public const int CurrentVersion = 1;

    public int Version { get; set; } = CurrentVersion;

    public Dictionary<string, float> Channels { get; set; } =
        new Dictionary<string, float>();

    /// <summary>Per-provider internal bags, keyed by <see cref="IAffectProvider.Id"/>.</summary>
    public Dictionary<string, Dictionary<string, float>> Providers { get; set; } =
        new Dictionary<string, Dictionary<string, float>>();
}
