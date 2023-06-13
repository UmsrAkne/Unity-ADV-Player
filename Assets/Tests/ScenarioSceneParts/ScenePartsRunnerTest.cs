using NUnit.Framework;
using ScenarioSceneParts;
using SceneContents;

namespace Tests.ScenarioSceneParts
{
    public class ScenePartsRunnerTest
    {
        [Test]
        public void AddTest()
        {
            var sr = new ScenePartsRunner();
            var ma = new ScenePartsMock { Priority = ExecutionPriority.Low, };
            var mb = new ScenePartsMock { Priority = ExecutionPriority.Middle, };
            var mc = new ScenePartsMock { Priority = ExecutionPriority.High, };

            sr.Add(mc);
            sr.Add(ma);
            sr.Add(mb);

            // 優先順位 High -> Low の順番で入力されているか
            Assert.AreEqual(sr.ScenePartsList[0], mc);
            Assert.AreEqual(sr.ScenePartsList[1], mb);
            Assert.AreEqual(sr.ScenePartsList[2], ma);
        }

        [Test]
        public void RunTest()
        {
            var sr = new ScenePartsRunner();
            var ma = new ScenePartsMock();
            var mb = new ScenePartsMock();

            sr.Add(ma);
            sr.Add(mb);

            // 実行する度にカウントが増えるか確認する
            Assert.AreEqual(0, ma.ExecutionCounter);
            Assert.AreEqual(0, mb.ExecutionCounter);

            var scr1 = new Scenario();
            sr.Run(scr1);
            Assert.AreEqual(1, ma.ExecutionCounter);
            Assert.AreEqual(scr1, ma.Scenario);
            Assert.AreEqual(1, mb.ExecutionCounter);
            Assert.AreEqual(scr1, mb.Scenario);

            var scr2 = new Scenario();
            sr.Run(scr2);
            Assert.AreEqual(2, ma.ExecutionCounter);
            Assert.AreEqual(scr2, ma.Scenario);
            Assert.AreEqual(2, mb.ExecutionCounter);
            Assert.AreEqual(scr2, mb.Scenario);
        }

        [Test]
        public void RunEveryFrameTest()
        {
            var sr = new ScenePartsRunner();
            var ma = new ScenePartsMock();
            var mb = new ScenePartsMock();

            sr.Add(ma);
            sr.Add(mb);

            Assert.AreEqual(0, ma.ExecutionEveryFrameCounter);
            Assert.AreEqual(0, mb.ExecutionEveryFrameCounter);

            sr.RunEveryFrame();
            Assert.AreEqual(1, ma.ExecutionEveryFrameCounter);
            Assert.AreEqual(1, mb.ExecutionEveryFrameCounter);

            sr.RunEveryFrame();
            Assert.AreEqual(2, ma.ExecutionEveryFrameCounter);
            Assert.AreEqual(2, mb.ExecutionEveryFrameCounter);
        }
    }
}