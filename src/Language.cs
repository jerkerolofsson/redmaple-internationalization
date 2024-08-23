namespace RedMaple.Internationalization
{
    /// <summary>
    /// Representation of a language, as defined by ISO-639
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Reference name
        /// </summary>
        public string? RefName { get; set; }

        /// <summary>
        /// ISO-639 Part1 two-letter code
        /// </summary>
        public string? Alpha2 { get; set; }

        /// <summary>
        /// ISO-639 Part3 three-letter code
        /// </summary>
        public string? Alpha3 { get; set; }

        /// <summary>
        /// ISO Language type code
        /// </summary>
        public string? IsoLanguageType { get; set; }

        /// <summary>
        /// Returns true if this is defined as a macro language
        /// </summary>
        public bool IsMacroLanguage => IsoLanguageType == "M";

        /// <summary>
        /// Returns true if this is defined as an individual language
        /// </summary>
        public bool IsIndividualLanguage => IsoLanguageType == "I";

        /// <summary>
        /// ISO639 Part2 T code
        /// </summary>
        public string? Part2T { get; internal set; }

        /// <summary>
        /// ISO639 Part2 B code
        /// </summary>
        public string? Part2B { get; internal set; }

        /// <summary>
        /// Macro language entry for this language
        /// </summary>
        public MacroLanguage? MacroLanguage { get; set; }
    }
}
