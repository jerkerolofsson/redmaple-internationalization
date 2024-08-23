namespace RedMaple.Internationalization.UnitTests
{
    public class ScriptTests
    {
        [Theory]
        [InlineData("Latn", "Latin")]
        [InlineData("Hang", "Hangul (Hangŭl, Hangeul)")]
        [InlineData("Hans", "Han (Simplified variant)")]
        [InlineData("Adlm", "Adlam")]                   // First entry in list
        [InlineData("Zzzz", "Code for uncoded script")] // Last entry in list
        public void TryGetScript_CorrectEnglishNameAndReturnsTrue(string code, string expectedName)
        {
            var res = Scripts.TryGetScript(code, out var script);
            Assert.True(res, $"Failed to get script for '{code}'");
            Assert.NotNull(script);
            Assert.Equal(expectedName, script.EnglishName);
        }
    }
}