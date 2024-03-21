using Animations;
using NUnit.Framework;
using Tests.ScenarioSceneParts;

namespace Tests.Animations
{
    public class FlashTest
    {
        [Test]
        public void ExecuteTest_通常実行()
        {
            var imageSet = new DisplayObjectMock();
            var flash = new Flash()
            {
                Duration = 6, EffectImageSet = imageSet,
            };

            // 実行前のデフォルトでは 0
            Assert.Zero(imageSet.Alpha);

            var a = imageSet.Alpha;

            flash.Execute();
            Assert.Greater(imageSet.Alpha, a, "前回の実行時から値が上昇");
            a = imageSet.Alpha;

            flash.Execute();
            Assert.Greater(imageSet.Alpha, a, "前回の実行時から値が上昇");
            a = imageSet.Alpha;

            flash.Execute();
            Assert.Greater(imageSet.Alpha, a, "前回の実行時から値が上昇");
            Assert.AreEqual(imageSet.Alpha, 1.0, "ここがピークで 1.0 に達する。");
            a = imageSet.Alpha;

            flash.Execute();
            Assert.Less(imageSet.Alpha, a, "折り返しを過ぎたので、値が減少し始める");
            a = imageSet.Alpha;

            flash.Execute();
            Assert.Less(imageSet.Alpha, a, "減少");
            a = imageSet.Alpha;

            flash.Execute();
            Assert.Less(imageSet.Alpha, a, "減少");
            Assert.Zero(imageSet.Alpha, "値が最低値の 0 に戻る");

            flash.Execute();
            Assert.Zero(imageSet.Alpha, "７回目以降の実行では変化しない");

            flash.Execute();
            Assert.Zero(imageSet.Alpha, "変化なし");
        }
    }
}