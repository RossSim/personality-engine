using System.Collections.Generic;

namespace PersonalityEngine;

public interface IAffectProvider
{
    string Id { get; }
    string Layer { get; }
    Citation Citation { get; }
    IReadOnlyList<Citation> AdditionalCitations { get; }
    AffectDelta Contribute(WorldEvent ev, float deltaTime, AffectSnapshot snapshot);
}
