using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMaple.Internationalization.UnitTests
{
    public class LanguageTagTests
    {
        [Theory]
        [InlineData("Hans",             null,       null,               "Han (Simplified variant)", null)]
        [InlineData("sv",               null,       "Swedish",          null, null)]
        [InlineData("en-US",            null,       "English",          null, "United States of America")]
        [InlineData("cmn-Hans-CN",      null,       "Mandarin Chinese", "Han (Simplified variant)", "China")]
        [InlineData("zh-cmn-Hans-CN",   "Chinese",  "Mandarin Chinese", "Han (Simplified variant)", "China")]
        public void TryParse_WithCorrectValues(
            string tag, 
            string? expectedMacroLanguage,
            string? expectedLanguage,
            string? expectedScript,
            string? expectedLocality)
        {
            var res = LanguageTag.TryParse(tag, out var languageTag);
            Assert.True(res, $"Expected '{tag}' to return true");
            Assert.NotNull(languageTag);
            Assert.Equal(expectedMacroLanguage, languageTag?.MacroLanguage?.RefName);
            Assert.Equal(expectedLanguage,      languageTag?.Language?.RefName);
            Assert.Equal(expectedScript,        languageTag?.Script?.EnglishName);
            Assert.Equal(expectedLocality,      languageTag?.Locality?.Name);
        }
    }
}
