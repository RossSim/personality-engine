namespace PersonalityEngine.Providers.Skinner;

/// <summary>
/// Subset of Ferster &amp; Skinner (1957). Interval schedules are documented, not implemented yet.
/// </summary>
public enum ReinforcementSchedule
{
    /// <summary>Every recorded response is reinforced (CRF / FR-1).</summary>
    Continuous,

    /// <summary>Every Nth response is reinforced.</summary>
    FixedRatio,

    /// <summary>A response is reinforced after a varying count whose mean is <see cref="OperantLearningProvider.VariableRatioMean"/>.</summary>
    VariableRatio
}
