namespace PersonalityEngine.Providers.Ocean;

internal static class OceanCitations
{
    public static readonly Citation FiveFactor = new Citation(
        "mccrae-costa-fft",
        "McCrae, R. R., & Costa, P. T., Jr. (2008). The five-factor theory of personality. In O. P. John, R. W. Robins, & L. A. Pervin (Eds.), Handbook of personality: Theory and research (3rd ed., pp. 159–181). Guilford Press. This provider stores host-supplied OCEAN floats. It does not implement NEO-PI-R, NEO-FFI, or any copyrighted inventory items.");

    public static readonly Citation MehrabianPad = new Citation(
        "mehrabian-pad",
        "Mehrabian, A., & Russell, J. A. (1974). An Approach to Environmental Psychology. MIT Press. Mehrabian, A. (1996). Pleasure-arousal-dominance: A general framework for describing and measuring individual differences in temperament. Current Psychology, 14(4), 261–292.");

    public static readonly Citation GebhardAlma2005 = new Citation(
        "gebhard-alma-2005",
        "Gebhard, P. (2005). ALMA: A layered model of affect. In Proceedings of the Fourth International Joint Conference on Autonomous Agents and Multiagent Systems (AAMAS '05) (pp. 29–36). ACM. https://doi.org/10.1145/1082473.1082478 Uses Mehrabian PAD mapping coefficients as reported there.");
}
