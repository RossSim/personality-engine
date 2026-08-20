using System.Collections.Generic;

namespace PersonalityEngine;

/// <summary>Maps a snapshot onto host-defined action ids. Missing channels must not throw.</summary>
public interface IActionWeighter
{
    string Id { get; }
    Citation Citation { get; }
    IReadOnlyDictionary<string, float> Weight(AffectSnapshot snapshot, IReadOnlyList<string> actionIds);
}
