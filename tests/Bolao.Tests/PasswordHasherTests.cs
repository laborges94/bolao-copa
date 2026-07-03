using Xunit;
using Bolao.Helpers;

namespace Bolao.Tests
{
    public class PasswordHasherTests
    {
        [Fact]
        public void Hash_WhenPasswordIsNull_ShouldReturnEmptyString()
        {
            // Act
            var result = PasswordHasher.Hash(null!);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Hash_WhenPasswordIsEmpty_ShouldReturnEmptyString()
        {
            // Act
            var result = PasswordHasher.Hash(string.Empty);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Hash_WhenPasswordIsValid_ShouldReturnCorrectSha256Hash()
        {
            // Arrange
            var password = "SenhaTeste123!";
            // SHA256 de "SenhaTeste123!" em formato hexadecimal minúsculo:
            // 91ad06287fa4fc8cdd9a4a1b6b9ebbff99fd22e507767fd3d3a862ac2a894125
            var expectedHash = "91ad06287fa4fc8cdd9a4a1b6b9ebbff99fd22e507767fd3d3a862ac2a894125";

            // Act
            var result = PasswordHasher.Hash(password);

            // Assert
            Assert.Equal(expectedHash, result);
        }

        [Fact]
        public void Hash_WhenDifferentPasswords_ShouldProduceDifferentHashes()
        {
            // Arrange
            var password1 = "senhaA";
            var password2 = "senhaB";

            // Act
            var hash1 = PasswordHasher.Hash(password1);
            var hash2 = PasswordHasher.Hash(password2);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
