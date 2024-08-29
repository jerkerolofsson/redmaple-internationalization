namespace RedMaple.Internationalization
{
    /// <summary>
    /// Class that parses and represents language tags such as:
    /// 
    /// eng-US              (American English)
    /// en-GB               (British English)
    /// en-Latn-CA          (Canadian English, Latin script)
    /// zh-cmn-Hans-CN      (Chinese macro language, Mandarin, Simplified Chinese, Mainland China
    /// 
    /// </summary>
    public class LanguageTag
    {
        private static Options _defaultParsingOptions = new Options();

        /// <summary>
        /// Parsing options
        /// </summary>
        public class Options
        {
            /// <summary>
            /// Tries to read macro language
            /// </summary>
            public bool ReadMacroLanguage { get; set; } = true;

            /// <summary>
            /// Tries to read individual language
            /// </summary>
            public bool ReadIndividualLanguage { get; set; } = true;

            /// <summary>
            /// Tries to parse a language tag as ISO639-2
            /// </summary>
            public bool Iso639_Alpha2 { get; set; } = true;

            /// <summary>
            /// Tries to parse a language tag as ISO639-3
            /// </summary>
            public bool Iso639_Alpha3 { get; set; } = true;

            /// <summary>
            /// Tries to read the script
            /// </summary>
            public bool ReadScript { get; set; } = true;

            /// <summary>
            /// Tries to read country/region information
            /// </summary>
            public bool ReadLocality { get; set; } = true;

            /// <summary>
            /// Tries to parse a locality tag as ISO3166 alpha2
            /// </summary>
            public bool Iso3166_Alpha2 { get; set; } = true;

            /// <summary>
            /// Tries to parse a locality tag as ISO3166 alpha3
            /// 
            /// Note:
            /// There is some overlap between ISO3166 and ISO639
            /// Example: ISO3166 CHN=China, ISO639 CHN=Chinook jargon
            /// </summary>
            public bool Iso3166_Alpha3 { get; set; } = false;

            /// <summary>
            /// Allows extending with additional information
            /// </summary>
            public List<ITagParser> ExtendedParsers { get; set; } = new();
        }

        /// <summary>
        /// List of tags that are not parsed as Macro Language, Language, Script or Locality etc.
        /// </summary>

        private List<string> mExtraSubTags = new List<string>();

        /// <summary>
        /// List of tags that are not parsed as Macro Language, Language, Script or Locality etc.
        /// </summary>
        public IReadOnlyList<string> ExtraSubTags => mExtraSubTags;

        /// <summary>
        /// The country/region, e.g. US, GB
        /// </summary>
        public Locality? Locality { get; set; }

        /// <summary>
        /// Macro language (language group)
        /// e.g. Chinese (zh)
        /// </summary>
        public MacroLanguage? MacroLanguage { get; set; }

        /// <summary>
        /// Individual language
        /// e.g. English (eng), Mandarin Chinese (cmn)
        /// </summary>
        public Language? Language { get; set; }

        /// <summary>
        /// Writing script
        /// e.g. Hans (simplified chinese) or Latn (Latin alphabet)
        /// </summary>
        public Script? Script { get; set; }

        /// <summary>
        /// The original language tag, as text
        /// </summary>
        public required string Text { get; init; }

        /// <summary>
        /// Returns a formatted language tag, as a string
        /// 
        /// The order may not be the same as the original, parsed, language tag and some representation
        /// may be different according to the following:
        /// - Macro language is formatted as a alpha-2 tag (if available)
        /// - Language is formatted as a alpha-3 tag
        /// </summary>
        public string Formatted
        {
            get
            {
                var items = new List<string>();
                if (MacroLanguage?.Iso639Code is not null)
                {
                    if (Languages.TryGetLanguage(MacroLanguage.Iso639Code, out var language) && language.Alpha2 is not null)
                    {
                        items.Add(language.Alpha2.ToLower());
                    }
                    else
                    {
                        items.Add(MacroLanguage.Iso639Code.ToLower());
                    }
                }
                if (Language?.Alpha3 is not null)
                {
                    items.Add(Language.Alpha3);
                }
                if (Script?.Code is not null)
                {
                    items.Add(Script.Code);
                }
                if (Locality?.Alpha2 is not null)
                {
                    items.Add(Locality.Alpha2.ToUpperInvariant());
                }

                return string.Join("-", items);
            }
        }

        /// <summary>
        /// Formats the langauge tag as a human readable string
        /// </summary>
        public string FormattedText
        {
            get
            {
                var items = new List<string>();

                // Only add macro language if no individual language was set
                if (Language?.RefName is null)
                {
                    if (MacroLanguage?.RefName is not null)
                    {
                        items.Add(MacroLanguage.RefName);
                    }
                }
                if (Language?.RefName is not null)
                {
                    items.Add(Language.RefName);
                }
                if (Script?.EnglishName is not null)
                {
                    items.Add(Script.EnglishName);
                }
                if (Locality?.Name is not null)
                {
                    items.Add(Locality.Name);
                }

                return string.Join(", ", items);
            }
        }

        /// <summary>
        /// Returns the formatted language tag
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Formatted;
        }

        /// <summary>
        /// Parses and returns a language tag, as any text will be parsed as supplementary tags
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the language tag could not be parsed</exception>
        public static LanguageTag Parse(string text)
        {
            if(TryParse(text, out var languageTag))
            {
                return languageTag;
            }
            throw new ArgumentException($"Failed to parse language tag '{text}'", nameof(text));
        }

        /// <summary>
        /// Tries to parse a language tag with default options
        /// </summary>
        /// <param name="text"></param>
        /// <param name="languageTag"></param>
        /// <returns></returns>
        public static bool TryParse(string text, [NotNullWhen(true)] out LanguageTag? languageTag)
        {
            return TryParse(text, _defaultParsingOptions, out languageTag);
        }

        /// <summary>
        /// Tries to parse a language tag with the specified options
        /// </summary>
        /// <param name="text"></param>
        /// <param name="options">Control which components of the language tag to parse</param>
        /// <param name="languageTag"></param>
        /// <returns></returns>
        public static bool TryParse(string text, Options options, [NotNullWhen(true)] out LanguageTag? languageTag)
        {
            languageTag = null;
            if (text.Contains(' ') || 
                text.Contains('\t') ||
                text.Contains('\r') ||
                text.Contains('\n'))
            {
                return false;
            }

            languageTag = new() { Text = text };

            var tags = text.Split('-');

            foreach (var tag in tags)
            {
                bool success = false;
                if (options.ReadMacroLanguage &&
                    languageTag.MacroLanguage is null &&
                    Languages.TryGetMacroLanguage(tag, out var macroLanguage))
                {
                    languageTag.MacroLanguage = macroLanguage;
                }
                else if (options.ReadIndividualLanguage && options.Iso639_Alpha3 &&
                    tag.Length == 3 &&
                    languageTag.Language is null &&
                    Languages.TryGetLanguageFromThreeLetterCode(tag, out var language))
                {
                    languageTag.Language = language;
                }
                else if (options.ReadIndividualLanguage && options.Iso639_Alpha2 &&
                    tag.Length == 2 &&
                    languageTag.Language is null &&
                    Languages.TryGetLanguageFromTwoLetterCode(tag, out var language2))
                {
                    languageTag.Language = language2;
                }

                else if (options.ReadScript &&
                    languageTag.Script is null &&
                    Scripts.TryGetScript(tag, out var script))
                {
                    languageTag.Script = script;
                }
                else if (options.ReadLocality &&
                    languageTag.Language is not null &&
                    languageTag.Locality is null &&
                    tag.ToUpper().Equals(tag))
                {
                    if (tag.Length == 3 && options.Iso3166_Alpha3)
                    {
                        if (Localities.TryGetByThreeLetterCode(tag, out var localityFromAlpha3))
                        {
                            languageTag.Locality = localityFromAlpha3;
                        }
                    }
                    if (tag.Length == 2 && options.Iso3166_Alpha2)
                    {
                        if (Localities.TryGetByTwoLetterCode(tag, out var localityFromAlpha2))
                        {
                            languageTag.Locality = localityFromAlpha2;
                        }
                    }
                }

                if(!success && options.ExtendedParsers.Count > 0)
                {
                    foreach (var parser in options.ExtendedParsers)
                    {
                        if (parser.Parse(tag, languageTag))
                        {
                            success = true;
                            break;
                        }
                    }
                }

                if (!success)
                {
                    languageTag.mExtraSubTags.Add(tag);
                }
            }

            return true;
        }
    }
}
