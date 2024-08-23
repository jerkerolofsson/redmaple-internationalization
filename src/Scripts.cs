namespace RedMaple.Internationalization
{
    /// <summary>
    /// Class for working with writing systems / scripts
    /// </summary>
    public class Scripts
    {
        /// <summary>
        /// Tries to get a script from the ISO code
        /// </summary>
        /// <param name="iso15924">ISO code, for example Latn, Hani, Hans, Hant</param>
        /// <param name="script"></param>
        /// <returns></returns>
        public static bool TryGetScript(string iso15924, [NotNullWhen(true)] out Script? script)
        {
            EnsureInitialized();
            script = null;
            if (mIso15924.TryGetValue(iso15924.ToLower(), out script))
            {
                return true;
            }
            return false;
        }

        #region Caching
        private static FrozenDictionary<string, Script>? mIso15924;

        [MemberNotNull(nameof(mIso15924))]
        private static void EnsureInitialized()
        {
            if (mIso15924 is null)
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RedMaple.Internationalization.Data.iso-15924.txt");
                if (stream is null)
                {
                    throw new InvalidDataException("Failed to load iso-15924.txt");
                }
                var dict = new Dictionary<string, Script>();
                using var reader = new StreamReader(stream);
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line is null)
                    {
                        break;
                    }
                    var trimmedLine = line.Trim();
                    if (trimmedLine.StartsWith('#') || trimmedLine.Length == 0)
                    {
                        continue;
                    }
                    var items = trimmedLine.Split(';');
                    if (items.Length > 4)
                    {
                        var code = items[0];
                        var number = items[1];
                        var englishName = items[2];
                        var frenchName = items[3];
                        var pva = items[4];

                        dict[code.ToLower()] = new Script
                        {
                            Code = code,
                            Number = number,
                            EnglishName = englishName,
                            PVA = pva
                        };
                    }
                }

                mIso15924 = dict.ToFrozenDictionary();
            }
        }
        #endregion
    }
}
