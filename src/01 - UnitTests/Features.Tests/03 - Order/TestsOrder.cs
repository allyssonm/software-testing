using Xunit;

namespace Features.Tests
{
    [TestCaseOrderer("Features.Tests.PriorityOrderer", "Features.Tests")]
    public class TestsOrder
    {
        public static bool TestCall1;
        public static bool TestCall2;
        public static bool TestCall3;
        public static bool TestCall4;

        [Fact(DisplayName = "Test 04"), TestPriority(3)]
        [Trait("UnitTests", "Tests orderer")]
        public void Test04()
        {
            TestCall4 = true;

            Assert.True(TestCall3);
            Assert.True(TestCall1);
            Assert.False(TestCall2);
        }

        [Fact(DisplayName = "Test 01"), TestPriority(2)]
        [Trait("UnitTests", "Tests orderer")]
        public void Test01()
        {
            TestCall1 = true;

            Assert.True(TestCall3);
            Assert.False(TestCall4);
            Assert.False(TestCall2);
        }

        [Fact(DisplayName = "Test 03"), TestPriority(1)]
        [Trait("UnitTests", "Tests orderer")]
        public void Test03()
        {
            TestCall3 = true;

            Assert.False(TestCall1);
            Assert.False(TestCall2);
            Assert.False(TestCall4);
        }

        [Fact(DisplayName = "Test 02"), TestPriority(4)]
        [Trait("UnitTests", "Tests orderer")]
        public void Test02()
        {
            TestCall2 = true;

            Assert.True(TestCall3);
            Assert.True(TestCall4);
            Assert.True(TestCall1);
        }
    }
}
