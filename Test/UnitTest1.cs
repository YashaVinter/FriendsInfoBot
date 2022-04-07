using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityFrameworkApp;

namespace EntityFrameworkApp.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod()]
        public void test14Test()
        {
            Assert.AreEqual(30, EntityFrameworkApp.ClassForTest.Add(10, 20));
            //Assert.Fail();
        }

        [TestMethod()]
        public void AddTest()
        {
            Assert.Fail();
        }
    }
}

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        private const int Expceted = 30;
        [TestMethod]
        public void TestMethod1()
        {
            var v = EntityFrameworkApp.Test.test14;
            int a=10;
            int b=20;
            var result = EntityFrameworkApp.Test.test14(a, b);
            Assert.AreEqual(Expceted,result);

        }
    }
}