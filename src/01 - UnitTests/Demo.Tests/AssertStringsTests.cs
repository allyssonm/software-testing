using Xunit;

namespace Demo.Tests
{
    public class AssertStringsTests
    {
        [Fact]
        public void StringsTools_JoinNames_ReturnsFullName()
        {
            // Arrange
            var sut = new StringTools();

            // Act
            var fullName = sut.Join("Michael", "Scott");

            // Assert
            Assert.Equal("Michael Scott", fullName);
        }

        [Fact]
        public void StringsTools_JoinNames_ShouldIgnoreCase()
        {
            // Arrange
            var sut = new StringTools();

            // Act
            var fullName = sut.Join("Michael", "Scott");

            // Assert
            Assert.Equal("MICHAEL SCOTT", fullName, true);
        }

        [Fact]
        public void StringsTools_JoinNames_ShouldContainsSnippet()
        {
            // Arrange
            var sut = new StringTools();

            // Act
            var fullName = sut.Join("Michael", "Scott");

            // Assert
            Assert.Contains("ael", fullName);
        }

        [Fact]
        public void StringsTools_JoinNames_ShouldStartsWith()
        {
            // Arrange
            var sut = new StringTools();

            // Act
            var fullName = sut.Join("Michael", "Scott");

            // Assert
            Assert.StartsWith("Mich", fullName);
        }

        [Fact]
        public void StringsTools_JoinNames_ShouldEndsWith()
        {
            // Arrange
            var sut = new StringTools();

            // Act
            var fullName = sut.Join("Michael", "Scott");

            // Assert
            Assert.EndsWith("ott", fullName);
        }

        [Fact]
        public void StringsTools_JoinNames_EvaluateRegex()
        {
            // Arrange
            var sut = new StringTools();

            // Act
            var fullName = sut.Join("Michael", "Scott");

            // Assert
            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", fullName);
        }
    }
}
