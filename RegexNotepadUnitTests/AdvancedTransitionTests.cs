using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegexNotepad.ApplicationLogic;

namespace RegexNotepadUnitTests
{
    [TestClass]
    public class AdvancedTransitionTests
    {
        [TestMethod]
        public void TestEquals()
        {
            var t1 = new AdvancedTransition<int>(0, 'A', 1);
            var t2 = new AdvancedTransition<int>(0, 'A', 1);
            var t3 = new AdvancedTransition<int>(0, 'B', 1);
            Assert.IsTrue(t1.Equals(t2));
            Assert.IsFalse(t1.Equals(t3));
        }

        [TestMethod]
        public void TestCompareTo()
        {
            var t1 = new AdvancedTransition<int>(0, 'A', 1);
            var t2 = new AdvancedTransition<int>(0, 'A', 1);
            var t3 = new AdvancedTransition<int>(0, 'B', 1);
            Assert.IsTrue(t1.CompareTo(t2) == 0);
            Assert.IsTrue(t1.CompareTo(t3) < 0);
            Assert.IsTrue(t3.CompareTo(t1) > 0);
        }
    }
}
