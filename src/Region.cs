namespace RedMaple.Internationalization
{
    public class Region
    {
        /// <summary>
        /// Name of the reqion (example Europe)
        /// </summary>
        public string? Name { get; internal set; }

        /// <summary>
        /// Name of the sub-region (example Northern Europe)
        /// </summary>
        public string? SubRegion { get; internal set; }

        /// <summary>
        /// Name of intermediate region (example Caribbean)
        /// </summary>
        public string? IntermediateRegion { get; internal set; }
    }
}
