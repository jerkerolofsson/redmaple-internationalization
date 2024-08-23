namespace RedMaple.Internationalization
{
    /// <summary>
    /// Representation of a macro language based on ISO-639
    /// </summary>
    public class MacroLanguage
    {
        /// <summary>
        /// ISO 639-3 macro language code
        /// </summary>
        public required string Iso639Code { get; set; }

        /// <summary>
        /// ISO 639-3 langauge codes
        /// </summary>
        public List<string> IndividualLanguageCodes { get; } = new();

        /// <summary>
        /// Reference to the iso entry for the language
        /// </summary>
        public List<Language> IndividualLanguages { get; } = new();

        public string? RefName { get; internal set; }
    }
}
