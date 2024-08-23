# RedMaple Internationalization

Parsing of language tags

- ISO-639 (Langages)
- ISO-3166 (Countries/Regions)
- ISO-15924 (Scipts)

## Example Uage

````csharp

if(LanguageTag.TryParse("en-US", out var language))
{
	// eng-US
	Console.WriteLine(language.Formatted);
}

if(LanguageTag.TryParse("eng-US", out var language))
{
	// English, United States of America
	Console.WriteLine(language.FormattedText);
}

if(LanguageTag.TryParse("eng-Latn-US", out var language))
{
	// Latin
	Console.WriteLine(language.Script.EnglishName);

	// Americas
	Console.WriteLine(language.Locality.Region);
}

if(LanguageTag.TryParse("zh-cmn-Hans-CN", out var language))
{
	// Mandarin Chinese, Han (Simplified variant), China
        Console.WriteLine(language.FormattedText);
        
        // zho (parsed from zh)
	Console.WriteLine(language.MacroLanguage.Iso639Code);
        
        // Chinese
	Console.WriteLine(language.MacroLanguage.RefName);

        // Mandarin Chinese
	Console.WriteLine(language.Language.RefName);
 
        // cmn
	Console.WriteLine(language.Language.Alpha3);

        // Hans
	Console.WriteLine(language.Script.Code);
       
        // 501
        Console.WriteLine(language.Script.Number);

        // Han (Simplified variant)
	Console.WriteLine(language.Script.EnglishName);

        // China
	Console.WriteLine(language.Locality.Name);
 
        // CN
	Console.WriteLine(language.Locality.Alpha2);
        
        // CHN
	Console.WriteLine(language.Locality.Alpha3);

        // Asia
	Console.WriteLine(language.Locality.Region);

        // Eastern Asia
	Console.WriteLine(language.Locality.SubRegion);
}

````
