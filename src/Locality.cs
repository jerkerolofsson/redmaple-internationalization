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
        public required string Id { get; init; }
        public string? IntermediateRegion { get; internal set; }
        public string? SubRegion { get; internal set; }
        public string? Region { get; internal set; }
        public string? RegionId { get; internal set; }
    }
}
