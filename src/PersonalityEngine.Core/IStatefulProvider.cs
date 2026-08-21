using System.Collections.Generic;

namespace PersonalityEngine;

/// <summary>
/// Optional persist hook. Snapshot channels are not always enough:
/// some providers keep internal values that the next tick reads.
/// Unknown keys on import must be ignored.
/// </summary>
public interface IStatefulProvider
{
    IReadOnlyDictionary<string, float> ExportState();
    void ImportState(IReadOnlyDictionary<string, float> bag);
}
