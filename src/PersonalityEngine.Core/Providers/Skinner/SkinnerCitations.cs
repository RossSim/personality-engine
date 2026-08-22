namespace PersonalityEngine.Providers.Skinner;

internal static class SkinnerCitations
{
    public static readonly Citation BehaviorOfOrganisms1938 = new Citation(
        "skinner-1938",
        "Skinner, B. F. (1938). The Behavior of Organisms: An Experimental Analysis. Appleton-Century.");

    public static readonly Citation ScienceAndHumanBehavior1953 = new Citation(
        "skinner-1953",
        "Skinner, B. F. (1953). Science and Human Behavior. Macmillan.");

    public static readonly Citation SchedulesOfReinforcement1957 = new Citation(
        "ferster-skinner-1957",
        "Ferster, C. B., & Skinner, B. F. (1957). Schedules of Reinforcement. Appleton-Century-Crofts.");

    public static readonly Citation VerbalBehavior1957 = new Citation(
        "skinner-verbal-1957",
        "Skinner, B. F. (1957). Verbal Behavior. Appleton-Century-Crofts. Documented; not implemented in this slice.");

    public static readonly Citation BeyondFreedomAndDignity1971 = new Citation(
        "skinner-1971",
        "Skinner, B. F. (1971). Beyond Freedom and Dignity. Knopf. Cited as philosophy; not a separate provider.");

    public static readonly Citation StrengthDynamics = new Citation(
        "pe-skinner-strength-dynamics",
        "0..1 operant strengths, CRF/FR/VR numeric gains, extinction decay, and SD-absent multiplier are project conventions implementing Skinner (1953) and Ferster & Skinner (1957) qualitatively. They are not published rate equations.",
        isProjectConvention: true);
}
