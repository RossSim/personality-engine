namespace PersonalityEngine;

/// <summary>Bibliographic pointer. Project-only knobs set <see cref="IsProjectConvention"/>.</summary>
public sealed class Citation
{
    public Citation(string key, string reference, bool isProjectConvention = false)
    {
        Key = key;
        Reference = reference;
        IsProjectConvention = isProjectConvention;
    }

    public string Key { get; }
    public string Reference { get; }
    public bool IsProjectConvention { get; }
}
