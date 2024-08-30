namespace RedMaple.Internationalization.UnitTests
{
    public class LanguageTests
    {
        [Theory]
        [InlineData("en", "English")]
        [InlineData("sv", "Swedish")]
        public void TryGetLanguageFromTwoLetterCode_WithValidCode_ReturnsTrueAndCorrectLanguage(string code, string expectedName)
        {
            var res = Languages.TryGetLanguageFromTwoLetterCode(code, out var language);
            Assert.True(res, $"Failed to get language for '{code}'");
            Assert.NotNull(language);
            Assert.Equal(expectedName, language.RefName);
        }

        [Theory]
        [InlineData("eng", "English")]
        [InlineData("swe", "Swedish")]
        public void TryGetLanguageFromThreeLetterCode_WithValidCode_ReturnsTrueAndCorrectLanguage(string code, string expectedName)
        {
            var res = Languages.TryGetLanguageFromThreeLetterCode(code, out var language);
            Assert.True(res, $"Failed to get language for '{code}'");
            Assert.NotNull(language);
            Assert.Equal(expectedName, language.RefName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("e")]
        [InlineData("engl")]
        [InlineData("\r")]
        [InlineData("\0")]
        public void TryGetLanguageFromThreeLetterCode_WithInvalidCode_ReturnsFalse(string invalidData)
        {
            var res = Languages.TryGetLanguageFromThreeLetterCode(invalidData, out var language);
            Assert.False(res, $"Expected '{invalidData}' to return false");
            Assert.Null(language);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("z")]
        [InlineData("engl")]
        [InlineData("\r")]
        [InlineData("\0")]
        public void TryGetMacroLanguage_WithInvalidCode_ReturnsFalse(string invalidData)
        {
            var res = Languages.TryGetMacroLanguage(invalidData, out var language);
            Assert.False(res, $"Expected '{invalidData}' to return false");
            Assert.Null(language);
        }

        [Theory]
        [InlineData("zh", "Chinese")]
        [InlineData("zho", "Chinese")]
        [InlineData("chi", "Chinese")] // 2B
        public void TryGetMacroLanguage_ReturnsTrueAndCorrectLanguage(string code, string expectedName)
        {
            var res = Languages.TryGetMacroLanguage(code, out var language);
            Assert.True(res, $"Expected '{code}' to return true");
            Assert.NotNull(language);
            Assert.Equal(expectedName, language.RefName);
        }
    }
}