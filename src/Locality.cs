namespace RedMaple.Internationalization
{
    public class Locality
    {
        /// <summary>
        /// Name of the country or region
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// ISO 3166 two-letter code
        /// </summary>
        public required string Alpha2 { get; init; }

        /// <summary>
        /// ISO 3166 three-letter code
        /// </summary>
        public required string Alpha3 { get; init; }

        /// <summary>
        /// Region of the locality
        /// </summary>
        public Region? Region { get; set; }
    }
}
