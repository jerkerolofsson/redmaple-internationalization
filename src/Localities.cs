namespace RedMaple.Internationalization
{
    /// <summary>
    /// Class for working with localities/countries/regions (ISO-3166)
    /// Mapping regions to sub-regions, and sub-regions to localities
    /// </summary>
    public class Localities
    {
        /// <summary>
        /// Gets a Locality from an iso3166 alpha-3 code
        /// </summary>
        /// <param name="alpha3"></param>
        /// <param name="locality"></param>
        /// <returns></returns>
        public static bool TryGetByThreeLetterCode(string alpha3, [NotNullWhen(true)] out Locality? locality)
        {
            locality = null;
            if (alpha3 is null || alpha3.Length != 3)
            {
                return false;
            }
            EnsureInitialized();
            if (true == mAlpha3Map?.TryGetValue(alpha3.ToUpper(), out Iso3166? iso3166))
            {
                locality = MapIso3166ToCountry(iso3166);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a Locality from an iso3166 alpha-2 code
        /// </summary>
        /// <param name="alpha2"></param>
        /// <param name="locality"></param>
        /// <returns></returns>
        public static bool TryGetByTwoLetterCode(string alpha2, [NotNullWhen(true)] out Locality? locality)
        {
            locality = null;
            if (alpha2 is null || alpha2.Length != 2)
            {
                return false;
            }
            EnsureInitialized();
            if (true == mAlpha2Map?.TryGetValue(alpha2.ToUpper(), out Iso3166? iso3166))
            {
                locality = MapIso3166ToCountry(iso3166);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a list of regions (Asia, Africa, Europe ..)
        /// </summary>
        public static IReadOnlyList<string> Regions
        {
            get
            {
                EnsureInitialized();
                return mRegions;
            }
        }

        /// <summary>
        /// Returns a list of localities in a region
        /// </summary>
        public static IReadOnlyList<Locality> GetRegionalLocalities(string region)
        {
            EnsureInitialized();
            List<Locality> localities = new();
            if (mRegionsMap.TryGetValue(region, out var list))
            {
                foreach(var locality in list)
                {
                    localities.Add(MapIso3166ToCountry(locality));
                }
            }
            return localities;
        }

        /// <summary>
        /// Returns a list of localities in a region
        /// </summary>
        public static IReadOnlyList<Locality> GetRegionalLocalities(string region, string subRegion)
        {
            EnsureInitialized();
            List<Locality> localities = new();
            if (mRegionsMap.TryGetValue(region, out var list))
            {
                foreach (var locality in list.Where(x=>x.subregion == subRegion))
                {
                    localities.Add(MapIso3166ToCountry(locality));
                }
            }
            return localities;
        }

        /// <summary>
        /// Returns a locality from region/subregion and alpha-3
        /// </summary>
        /// <param name="region"></param>
        /// <param name="subRegion"></param>
        /// <param name="iso3166_3"></param>
        /// <returns></returns>
        public static Locality? GetLocalityInRegion(string region, string subRegion, string iso3166_3)
        {
            return GetLocality(region, subRegion, (x) => x.Alpha3.Equals(iso3166_3, StringComparison.InvariantCultureIgnoreCase));
        }
        
        public static Locality? GetLocality(string region, string subRegion, Predicate<Locality> predicate)
        {
            EnsureInitialized();
            if (mRegionsMap.TryGetValue(region, out var list))
            {
                foreach (var iso in list.Where(x => x.subregion == subRegion))
                {
                    var locality = MapIso3166ToCountry(iso);
                    if(predicate(locality))
                    {
                        return locality;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Returns a list of sub-regions within a region
        /// </summary>
        public static IReadOnlyList<string> GetSubRegions(string region)
        {
            EnsureInitialized();
            if(mRegionsToSubRegions.TryGetValue(region, out var list))
            {
                return list;
            }
            return new List<string>();
        }

        private static Locality MapIso3166ToCountry(Iso3166 iso3166)
        {
            return new Locality
            {
                Alpha2 = iso3166.alpha2,
                Alpha3 = iso3166.alpha3,
                Name = iso3166.name,
                Region = new Region
                {
                    Name = iso3166.region,
                    SubRegion = iso3166.subregion,
                    IntermediateRegion = iso3166.intermediateregion,
                }
            };
        }

        #region Caching
        private static FrozenDictionary<string, Iso3166>? mAlpha2Map;
        private static FrozenDictionary<string, Iso3166>? mAlpha3Map;
        private static IReadOnlyList<string>? mRegions;
        private static FrozenDictionary<string, List<string>>? mRegionsToSubRegions;
        private static FrozenDictionary<string, List<Iso3166>>? mRegionsMap;
        private static FrozenDictionary<int, Region>? mRegionCodeMap;
        private static FrozenDictionary<int, Region>? mSubRegionCodeMap;

        [MemberNotNull(
            nameof(mAlpha2Map),
            nameof(mAlpha3Map),
            nameof(mRegionsMap),
            nameof(mRegionCodeMap),
            nameof(mSubRegionCodeMap),
            nameof(mRegionsToSubRegions),
            nameof(mRegions))]
        private static void EnsureInitialized()
        {
            if (mAlpha2Map is null || mAlpha3Map is null || mRegions is null || mRegionsToSubRegions is null || mRegionsMap is null)
            {
                using var jsonStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RedMaple.Internationalization.Data.iso-3166.json");
                if (jsonStream is null)
                {
                    throw new InvalidDataException("Failed to load iso3166.json");
                }
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var items = JsonSerializer.Deserialize<List<Iso3166>>(jsonStream, options);
                if (items is null)
                {
                    throw new InvalidDataException("Failed to load iso3166.json");
                }

                var alpha2 = new Dictionary<string, Iso3166>();
                var alpha3 = new Dictionary<string, Iso3166>();
                var regions = new Dictionary<string, List<string>>();
                var regionsMap = new Dictionary<string, List<Iso3166>>();
                var regionCodeMap = new Dictionary<int, Region>();
                var subRegionCodeMap = new Dictionary<int, Region>();
                foreach (var item in items)
                {
                    alpha2[item.alpha2] = item;
                    alpha3[item.alpha3] = item;

                    if (!string.IsNullOrEmpty(item.region))
                    {
                        if (int.TryParse(item.regioncode, out int regionCode) && !regionCodeMap.ContainsKey(regionCode))
                        {
                            Region region = MapToRegion(item);
                            regionCodeMap[regionCode] = region;
                        }
                        if (int.TryParse(item.subregioncode, out int subRegionCode) && !subRegionCodeMap.ContainsKey(subRegionCode))
                        {
                            Region region = MapToSubRegion(item);
                            subRegionCodeMap[subRegionCode] = region;
                        }
                    }

                    if (!string.IsNullOrEmpty(item.region))
                    {
                        if (!regions.TryGetValue(item.region, out var subRegions))
                        {
                            regionsMap[item.region] = new List<Iso3166>();
                            subRegions = new List<string>();
                            regions[item.region] = subRegions;
                        }
                        regionsMap[item.region].Add(item);
                        if (!string.IsNullOrEmpty(item.subregion))
                        {
                            subRegions.Add(item.subregion);
                        }
                    }
                }
                mAlpha2Map = alpha2.ToFrozenDictionary();
                mAlpha3Map = alpha3.ToFrozenDictionary();
                mRegions = regions.Keys.ToList();
                mRegionsToSubRegions = regions.ToFrozenDictionary();
                mRegionsMap = regionsMap.ToFrozenDictionary();
                mRegionCodeMap = regionCodeMap.ToFrozenDictionary();
                mSubRegionCodeMap = subRegionCodeMap.ToFrozenDictionary();
            }
        }

        private static Region MapToRegion(Iso3166 item)
        {
            return new Region
            {
                Name = item.region
            };
        }
        private static Region MapToSubRegion(Iso3166 item)
        {
            return new Region
            {
                Name = item.region,
                SubRegion = item.subregion
            };
        }

        /// <summary>
        /// Gets a region by it's code
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="region"></param>
        /// <returns>true if success</returns>
        public static bool TryGetRegionByCode(int regionCode, [NotNullWhen(true)] out Region? region)
        {
            EnsureInitialized();
            if (mRegionCodeMap.TryGetValue(regionCode, out region))
            {
                return true;
            }
            region = null;
            return false;
        }

        /// <summary>
        /// Gets a subregion by it's code
        /// </summary>
        /// <param name="subRegionCode"></param>
        /// <param name="region"></param>
        /// <returns>true if success</returns>
        public static bool TryGetSubRegionByCode(int subRegionCode, [NotNullWhen(true)] out Region? region)
        {
            EnsureInitialized();
            if (mSubRegionCodeMap.TryGetValue(subRegionCode, out region))
            {
                return true;
            }
            region = null;
            return false;
        }
        #endregion
    }
}
