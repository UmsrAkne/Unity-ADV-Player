using System;
using Animations;
using NUnit.Framework;
using Tests.ScenarioSceneParts;

namespace Tests.Animations
{
    [TestFixture]
    public class TestScaleChange
    {
        [Test]
        public void 生成テスト()
        {
            var _ = new ScaleChange();
        }

        [Test]
        public void 遅延実行通常動作()
        {
            var dummy = new DisplayObjectMock();
            var scaleChanger = new ScaleChange
            {
                Target = dummy,
                To = 1.3,
                Duration = 3,
                Delay = 3,
            };

            Assert.IsTrue(scaleChanger.IsWorking);
            Assert.Less(Math.Abs(dummy.Scale) - 1.0,  0.01);

            scaleChanger.Execute();
            scaleChanger.Execute();
            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.0,  0.01);

            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.1,  0.01);

            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.2,  0.01);

            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.3,  0.01);

            Assert.IsFalse(scaleChanger.IsWorking);

            // 停止後に実行しても動作しないことを確認する
            scaleChanger.Execute();
            scaleChanger.Execute();
            scaleChanger.Execute();
            Assert.Less(Math.Abs(dummy.Scale) - 1.3,  0.01);
            Assert.IsFalse(scaleChanger.IsWorking);
        }

        [Test]
        public void 通常実行のテスト()
        {
            var dummy = new DisplayObjectMock();
            var scaleChanger = new ScaleChange
            {
                Target = dummy,
                To = 1.3,
                Duration = 5,
            };

            Assert.IsTrue(scaleChanger.IsWorking);
            Assert.Less(Math.Abs(dummy.Scale) - 1.0,  0.01);

            scaleChanger.Execute();
            scaleChanger.Execute();
            scaleChanger.Execute();
            scaleChanger.Execute();
            scaleChanger.Execute();
            scaleChanger.Execute();

            Assert.Less(Math.Abs(dummy.Scale) - 1.3,  0.01);

            Assert.IsFalse(scaleChanger.IsWorking);
        }
    }
}