using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMaple.Internationalization.UnitTests
{
    public class LocalityTests
    {
        [Theory]
        [InlineData("US", "United States of America")]
        [InlineData("uS", "United States of America")]
        [InlineData("us", "United States of America")]
        [InlineData("Us", "United States of America")]
        public void TryGetByAlpha2_WithDifferentCase(string alpha2, string expectedName)
        {
            var res = Localities.TryGetByTwoLetterCode(alpha2, out var locality);
            Assert.True(res, $"Failed to get locality for '{alpha2}'");
            Assert.NotNull(locality);
            Assert.Equal(expectedName, locality.Name);
        }

        [Theory]
        [InlineData("USA", "United States of America")]
        [InlineData("uSa", "United States of America")]
        [InlineData("usA", "United States of America")]
        [InlineData("UsA", "United States of America")]
        [InlineData("usa", "United States of America")]
        public void TryGetByAlpha3_WithDifferentCase(string alpha3, string expectedName)
        {
            var res = Localities.TryGetByThreeLetterCode(alpha3, out var locality);
            Assert.True(res, $"Failed to get locality for '{alpha3}'");
            Assert.NotNull(locality);
            Assert.Equal(expectedName, locality.Name);
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\0")]
        [InlineData("AAAA")]
        [InlineData("A")]
        public void TryGetByAlpha3_WithInvalidData_ReturnsFalse(string alpha3)
        {
            var res = Localities.TryGetByThreeLetterCode(alpha3, out var locality);
            Assert.False(res, $"Expected TryGetByAlpha3 to return false");
            Assert.Null(locality);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\0")]
        [InlineData("AAA")]
        [InlineData("A")]
        public void TryGetByAlpha2_WithInvalidData_ReturnsFalse(string alpha2)
        {
            var res = Localities.TryGetByTwoLetterCode(alpha2, out var locality);
            Assert.False(res, $"Expected TryGetByAlpha2 to return false");
            Assert.Null(locality);
        }
    }
}
