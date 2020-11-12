using System;
using Xunit;

namespace Demo.Tests
{
    public class AssertingExceptionsTests
    {
        [Fact]
        public void Calculator_Divide_ShouldReturnsDivideByZeroError()
        {
            // Arrange
            var calc = new Calculator();

            // Act & Assert
            Assert.Throws<DivideByZeroException>(() => calc.Divide(10, 0));
        }


        [Fact]
        public void Employee_Salary_ShouldReturnsNotAllowedSalary()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<Exception>(() => EmployeeFactory.Create("Michael", 250));

            Assert.Equal("Salary is lower than allowed", exception.Message);
        }
    }
}
