using Xunit;

namespace Demo.Tests
{
    public class AssertingCollectionsTests
    {
        [Fact]
        public void Employee_Skills_SkillsShouldNotBeEmpty()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Michael", 10000);

            // Assert
            Assert.All(employee.Skills, skill => Assert.False(string.IsNullOrWhiteSpace(skill)));
        }

        [Fact]
        public void Employee_Skills_JuniorShouldHaveBasicSkills()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Michael", 1000);

            // Assert
            Assert.Contains("OOP", employee.Skills);
        }


        [Fact]
        public void Employee_Skills_JuniorShouldNotHaveAdvancedSkills()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Michael", 1000);

            // Assert
            Assert.DoesNotContain("Microservices", employee.Skills);
        }


        [Fact]
        public void Employee_Skills_SeniorShouldHaveAllSkills()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Michael", 15000);

            var basicSkills = new[]
            {
                "Programming Logic",
                "OOP",
                "Tests",
                "Microservices"
            };

            // Assert
            Assert.Equal(basicSkills, employee.Skills);
        }
    }
}
