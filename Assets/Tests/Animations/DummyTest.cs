using Animations;
using NUnit.Framework;

namespace Tests.Animations
{
    [TestFixture]
    public class DummyTest
    {
        [Test]
        public void ExecuteTest()
        {
            var dummy = new Dummy { Duration = 2, };

            Assert.That(dummy.IsWorking, Is.True);

            dummy.Execute();
            Assert.That(dummy.IsWorking, Is.True);

            dummy.Execute();
            Assert.That(dummy.IsWorking, Is.False, "2回実行したらアニメーションは停止しているはず");
        }

        [Test]
        public void ExecuteTest_Delayあり()
        {
            var dummy = new Dummy { Duration = 2, Delay = 2, };

            Assert.That(dummy.IsWorking, Is.True);

            dummy.Execute();
            Assert.That(dummy.IsWorking, Is.True, "e1");

            dummy.Execute();
            Assert.That(dummy.IsWorking, Is.True, "e2");

            dummy.Execute();
            Assert.That(dummy.IsWorking, Is.True, "e3");

            dummy.Execute();
            Assert.That(dummy.IsWorking, Is.False, "2 + 2 = 4回実行したらアニメーションは停止しているはず");
        }
    }
}