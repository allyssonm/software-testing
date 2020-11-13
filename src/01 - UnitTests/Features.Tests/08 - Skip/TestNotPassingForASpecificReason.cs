using Xunit;

namespace Features.Test
{
    public class TestNotPassingForASpecificReason
    {
        [Fact(DisplayName = "New Client 2.0", Skip = "New version")]
        [Trait("UnitTests", "Skipping test")]
        public void Test_NotPassing_NewVersionIsIncompatible()
        {
            Assert.True(false);
        }
    }
}
