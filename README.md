# RedMaple Internationalization

Parsing of language tags

- ISO-639 (Langages)
- ISO-3166 (Countries/Regions)
- ISO-15924 (Scipts)

## Example Uage

````csharp

if(LanguageTags.TryParse("en-US", out var language))
{
	Console.WriteLine(language.Formatted);
}

if(LanguageTags.TryParse("eng-US", out var language))
{
	Console.WriteLine(language.FormattedText);
}

if(LanguageTags.TryParse("eng-Latn-US", out var language))
{
	Console.WriteLine(language.Script.EnglishName);
	Console.WriteLine(language.Locality.Region);
}

if(LanguageTags.TryParse("zh-cmn-Hans-CN", out var language))
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
	Console.WriteLine(language.Script.PVA);

	Console.WriteLine(language.Locality.Name);
	Console.WriteLine(language.Locality.Alpha2);
	Console.WriteLine(language.Locality.Alpha3);
	Console.WriteLine(language.Locality.Region);
	Console.WriteLine(language.Locality.SubRegion);
}

````
