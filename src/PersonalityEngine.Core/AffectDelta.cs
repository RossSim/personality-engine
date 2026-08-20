using System.Collections.Generic;

namespace PersonalityEngine;

public sealed class AffectDelta
{
    private readonly Dictionary<string, float> _values = new Dictionary<string, float>();

    public IReadOnlyDictionary<string, float> Values => _values;

    public AffectDelta Set(string key, float value)
    {
        _values[key] = value;
        return this;
    }
}
