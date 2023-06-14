using System.Collections.Generic;
using NUnit.Framework;
using ScenarioSceneParts;
using SceneContents;

namespace Tests.ScenarioSceneParts
{
    [TestFixture]
    public class TextWriterTest
    {
        [Test]
        public void TestExecute()
        {
            var res = new Resource
            {
                Scenarios = new List<Scenario>()
                {
                    new() { Text = "one" },
                    new() { Text = "two" },
                    new() { Text = "three" }
                }
            };

            var writer = new TextWriter();
            writer.SetResource(res);

            // ここからテスト

            writer.Execute();
            Assert.AreEqual(writer.CurrentText, string.Empty);

            writer.ExecuteEveryFrame();
            Assert.AreEqual(writer.CurrentText, "o");

            for (var i = 0; i < 10; i++)
            {
                writer.ExecuteEveryFrame();
            }

            Assert.AreEqual(writer.CurrentText, "one", "11frame 経過時の状態。テキストの全てが過不足なく入力済みか");

            writer.Execute();
            Assert.AreEqual(writer.CurrentText, string.Empty);

            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();
            writer.Execute();

            Assert.AreEqual(writer.CurrentText, "two", "2フレーム経過でテキスト描画を切り上げ。テキストが全て入力されているか");

            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();
            Assert.AreEqual(writer.CurrentText, "two", "executeEveryFrame() を余分に実行しても問題ないか？");
        }

        [Test]
        public void シナリオのカウンターのテスト()
        {
            var res = new Resource
            {
                Scenarios = new List<Scenario>()
                {
                    new() { Text = "one" },
                    new() { Text = "two" },
                    new() { Text = "three" }
                }
            };

            var writer = new TextWriter();
            writer.SetResource(res);

            Assert.AreEqual(writer.ScenarioIndex, 0);
            writer.Execute();

            Assert.AreEqual(writer.ScenarioIndex, 0);
            writer.Execute();
            writer.Execute();

            Assert.AreEqual(writer.ScenarioIndex, 1);

            writer.Execute();
            writer.Execute();
            Assert.AreEqual(writer.ScenarioIndex, 2);
        }

        [Test]
        public void 特定のインデックスまでジャンプするテスト()
        {
            var res = new Resource
            {
                Scenarios = new List<Scenario>()
                {
                    new() { Text = "one" },
                    new() { Text = "two" },
                    new() { Text = "three" },
                    new() { Text = "four" }
                }
            };

            var writer = new TextWriter();
            writer.SetResource(res);

            // ここからテスト

            writer.SetScenarioIndex(2);
            Assert.AreEqual(writer.CurrentText, string.Empty);

            writer.ExecuteEveryFrame();
            Assert.AreEqual(writer.CurrentText, string.Empty, "ExecuteEveryFrame を呼び出してもテキストは入力されない");

            writer.Execute();
            Assert.AreEqual(writer.CurrentText, string.Empty);

            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();

            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();
            writer.ExecuteEveryFrame();

            Assert.AreEqual(writer.CurrentText, "three", "2フレーム経過でテキスト描画を切り上げ。テキストが全て入力されているか");
        }
    }
}