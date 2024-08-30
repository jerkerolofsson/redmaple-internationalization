using RedMaple.Internationalization;

if (LanguageTag.TryParse("en-US", out var language1))
{
    Console.WriteLine(language1.Formatted);
}

if (LanguageTag.TryParse("eng-US", out var language2))
{
    Console.WriteLine(language2.FormattedText);
}

if (LanguageTag.TryParse("eng-Latn-US", out var language3))
{
    Console.WriteLine(language3.Script.EnglishName);
    Console.WriteLine(language3.Locality.Region);
}

if (LanguageTag.TryParse("zh-cmn-Hans-CN", out var language))
{
    Console.WriteLine(language.FormattedText);

    Console.WriteLine(language.MacroLanguage.Iso639Code);
    Console.WriteLine(language.MacroLanguage.RefName);

    Console.WriteLine(language.Language.RefName);
    Console.WriteLine(language.Language.Alpha2);
    Console.WriteLine(language.Language.Alpha3);
    Console.WriteLine(language.Language.Part2T);
    Console.WriteLine(language.Language.Part2B);

    Console.WriteLine(language.Script.Code);
    Console.WriteLine(language.Script.Number);
    Console.WriteLine(language.Script.EnglishName);

    Console.WriteLine(language.Locality.Name);
    Console.WriteLine(language.Locality.Alpha2);
    Console.WriteLine(language.Locality.Alpha3);
    Console.WriteLine(language.Locality.Region.Name);
    Console.WriteLine(language.Locality.Region.SubRegion);
}