using Xunit;

namespace Demo.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Calculator_Sum_ReturnsSumValue()
        {
            // Arrange
            var calc = new Calculator();

            // Act
            var result = calc.Sum(2, 2);

            // Assert
            Assert.Equal(4, result);
        }

        [Theory]
        [InlineData(1,1,2)]
        [InlineData(7,3,10)]
        [InlineData(9,10,19)]
        public void Calculator_Sum_ReturnsSumCorrectValues(double v1, double v2, double total)
        {
            // Arrange
            var calc = new Calculator();

            // Act
            var result = calc.Sum(v1, v2);

            // Assert
            Assert.Equal(total, result);
        }
    }
}
