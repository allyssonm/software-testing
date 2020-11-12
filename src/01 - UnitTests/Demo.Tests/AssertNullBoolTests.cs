using Xunit;

namespace Demo.Tests
{
    public class AssertNullBoolTests
    {
        [Fact]
        public void Employee_Name_ShouldNotBeNullOrEmpty()
        {
            // Arrange & Act
            var employee = new Employee("", 1000);

            // Assert
            Assert.False(string.IsNullOrEmpty(employee.Name));
        }

        [Fact]
        public void Employee_Nick_ShouldNotHaveNick()
        {
            // Arrange & Act
            var employee = new Employee("Michael", 1000);

            // Assert
            Assert.Null(employee.Nick);

            // Assert Bool
            Assert.True(string.IsNullOrEmpty(employee.Nick));
            Assert.False(employee.Nick?.Length > 0);
        }
    }
}
