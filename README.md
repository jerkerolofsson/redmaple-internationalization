# RedMaple Internationalization

Parsing of language tags

- ISO-639 (Langages)
- ISO-3166 (Countries/Regions)
- ISO-15924 (Scipts)

## Example Uage

````csharp

if(LanguageTags.TryParse("en-US", out var language))
{
	Console.WriteLine(language);
}

if(LanguageTags.TryParse("eng-US", out var language))
{
	Console.WriteLine(language);
}

if(LanguageTags.TryParse("eng-Latn-US", out var language))
{
	Console.WriteLine(language);
}

if(LanguageTags.TryParse("zh-cmn-Hans-CN", out var language))
{
	Console.WriteLine(language);
}

````