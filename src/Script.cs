namespace RedMaple.Internationalization
{
    /// <summary>
    /// Representation of a script (writing system / alphabet)
    /// </summary>
    public class Script
    {
        /// <summary>
        /// ISO 15924 code
        /// </summary>
        public required string Code { get; set; }
        public string? Number { get; set; }
        public string? EnglishName { get; set; }
        public string? PVA { get; set; }
    }
}
