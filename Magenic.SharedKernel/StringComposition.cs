namespace Magenic.SharedKernel
{
    /// <summary>
    /// An enumerated type that constitutes a bitmap for specifying string composition.
    /// </summary>
    public enum StringComposition
    {
        None = 0x00,
        LowercaseLetter = 0x01,
        UppercaseLetter = 0x02,
        Digit = 0x04,
        Symbol = 0x08,
        PunctuationMark = 0x10,
        WhiteSpace = 0x20,

        Letter = LowercaseLetter | UppercaseLetter,
        AlphaNumeric = Letter | Digit,
        AlphaNumericWhiteSpace = AlphaNumeric | WhiteSpace,
        All = LowercaseLetter | UppercaseLetter | Digit | Symbol | PunctuationMark | WhiteSpace
    }
}
