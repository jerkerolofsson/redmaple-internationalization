using RedMaple.Internationalization.Models;

namespace RedMaple.Internationalization
{
    /// <summary>
    /// Class for working with languages (ISO-639)
    /// </summary>
    /// <example>
    /// 
    /// if(Languages.TryGetLanguageFromThreeLetterCode("eng", out var language))
    /// {
    ///     Console.WriteLine($"{language.Alpha3}: {language.RefName} ({language.Alpha2})");
    /// }
    /// </example>
    public class Languages
    {
        /// <summary>
        /// Cached collection of macro languages
        /// iso code is key
        /// </summary>
        private static FrozenDictionary<string, MacroLanguage>? mIso639MacroLanguages;

        /// <summary>
        /// ISO 639 models with the alpha-3 code as key
        /// </summary>
        private static FrozenDictionary<string, Iso639>? mIso639_3;

        /// <summary>
        /// ISO 639 models with the alpha-2 code as key
        /// </summary>
        private static FrozenDictionary<string, Iso639>? mIso639_2;

        /// <summary>
        /// All ISO alpha-3 language codes
        /// </summary>
        internal static IEnumerable<Iso639> Iso639_3
        {
            get
            {
                InitializeIso639_3();
                return mIso639_3.Values;
            }
        }

        /// <summary>
        /// Tries to get a macro language from the iso code (alpha-3 or alpha-2)
        /// </summary>
        /// <param name="iso639Code"></param>
        /// <param name="macroLanguage"></param>
        /// <returns></returns>
        public static bool TryGetMacroLanguage(string iso639Code, [NotNullWhen(true)] out MacroLanguage? macroLanguage)
        {
            ArgumentNullException.ThrowIfNull(iso639Code);

            // First look-up in the iso list of macro languages
            if (iso639Code.Length == 3)
            {
                InitializeMacroLanguages();
                if (mIso639MacroLanguages.TryGetValue(iso639Code.ToLower(), out macroLanguage))
                {
                    return true;
                }
            }

            // ..if not found, check if it is a 2 letter code.
            // we don't have a lookup table here so first try to get the language from the 
            // mIso639_2 cache which contains the complete language entity and then use the Id
            // which is the alpha-3 code to check the macro-language dictionary
            if (iso639Code.Length == 2)
            {
                InitializeMacroLanguages();
                InitializeIso639_3();
                if (mIso639_2.TryGetValue(iso639Code, out var language))
                {
                    if (mIso639MacroLanguages.TryGetValue(language.Id.ToLower(), out macroLanguage))
                    {
                        return true;
                    }
                }
            }

            macroLanguage = null;
            return false;
        }

        /// <summary>
        /// Tries to get a language model from an ISO-639 alpha-2 code
        /// </summary>
        /// <param name="iso639_2">Language code, e.g. eng, fin, fre</param>
        /// <param name="language">Language model, if the code was found</param>
        public static bool TryGetLanguageFromTwoLetterCode(string iso639_2, [NotNullWhen(true)] out Language? language)
        {
            ArgumentNullException.ThrowIfNull(iso639_2);

            InitializeIso639_3();
            language = null;
            if (mIso639_2.TryGetValue(iso639_2.ToLower(), out var lang))
            {
                language = MapIso639ModelToLanguage(lang);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Tries to get a language model from an ISO-639 alpha-3 code
        /// </summary>
        /// <param name="iso639_3">Language code, e.g. eng, fin, fre</param>
        /// <param name="language">Language model, if the code was found</param>
        public static bool TryGetLanguageFromThreeLetterCode(string iso639_3, [NotNullWhen(true)] out Language? language)
        {
            ArgumentNullException.ThrowIfNull(iso639_3);

            InitializeIso639_3();
            language = null;
            if (mIso639_3.TryGetValue(iso639_3.ToLower(), out var lang))
            {
                language = MapIso639ModelToLanguage(lang);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to get a language model from an ISO-639 alpha-3 code
        /// </summary>
        /// <param name="iso639_3">Language code, e.g. eng, fin, fre</param>
        /// <param name="language">Language model, if the code was found</param>
        /// <returns></returns>
        public static bool TryGetLanguage(string iso639_3, [NotNullWhen(true)] out Language? language)
        {
            ArgumentNullException.ThrowIfNull(iso639_3);
            return TryGetLanguageFromThreeLetterCode(iso639_3, out language);
        }


        private static Language MapIso639ModelToLanguage(Iso639 lang)
        {
            return new Language
            {
                RefName = lang.Ref_Name,
                Alpha2 = lang.Part1,
                Alpha3 = lang.Id,
                IsoLanguageType = lang.Language_Type,
                Part2T = lang.Part2T,
                Part2B = lang.Part2B,

            };
        }

        #region Caching
        [MemberNotNull(nameof(mIso639_3), nameof(mIso639_2))]
        private static void InitializeIso639_3()
        {
            if (mIso639_3 is null || mIso639_2 is null)
            {
                using var jsonStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RedMaple.Internationalization.Data.iso-639-3.json");
                if (jsonStream is null)
                {
                    throw new InvalidDataException("Failed to load iso-639-3.json");
                }
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, AllowTrailingCommas = true };
                var items = JsonSerializer.Deserialize<List<Iso639>>(jsonStream, options);
                if (items is null)
                {
                    throw new InvalidDataException("Failed to load iso-639-3.json");
                }

                var alpha2 = new Dictionary<string, Iso639>();
                var alpha3 = new Dictionary<string, Iso639>();
                foreach (var item in items)
                {
                    if (item.Part1 is not null)
                    {
                        alpha2[item.Part1] = item;
                    }
                    alpha3[item.Id] = item;
                }
                mIso639_2 = alpha2.ToFrozenDictionary();
                mIso639_3 = alpha3.ToFrozenDictionary();
            }
        }

        [MemberNotNull(nameof(mIso639MacroLanguages))]
        private static void InitializeMacroLanguages()
        {
            if (mIso639MacroLanguages is null)
            {
                using var jsonStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RedMaple.Internationalization.Data.iso-639-3-macrolanguage.json");
                if (jsonStream is null)
                {
                    throw new InvalidDataException("Failed to load iso-639-3-macrolanguage.json");
                }
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, AllowTrailingCommas = true };
                var items = JsonSerializer.Deserialize<List<Iso639_MacroLanguage>>(jsonStream, options);
                if (items is null)
                {
                    throw new InvalidDataException("Failed to load iso-639-3-macrolanguage.json");
                }

                var alpha3 = new Dictionary<string, MacroLanguage>();
                foreach (var item in items)
                {
                    if (!alpha3.TryGetValue(item.M_Id, out MacroLanguage? macroLanguage))
                    {
                        macroLanguage = new MacroLanguage() { Iso639Code = item.M_Id };

                        // Get the complete language model based on the language code
                        if (Languages.TryGetLanguage(item.M_Id, out var language))
                        {
                            macroLanguage.RefName = language.RefName;
                        }

                        alpha3.Add(item.M_Id, macroLanguage);
                    }
                    if (item.I_Id is not null && !macroLanguage.IndividualLanguageCodes.Contains(item.I_Id))
                    {
                        if (Languages.TryGetLanguage(item.I_Id, out var language))
                        {
                            language.MacroLanguage = macroLanguage;
                            macroLanguage.IndividualLanguages.Add(language);
                        }

                        macroLanguage.IndividualLanguageCodes.Add(item.I_Id);
                    }
                }
                mIso639MacroLanguages = alpha3.ToFrozenDictionary();
            }
        }
        #endregion
    }
}
