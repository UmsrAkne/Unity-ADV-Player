using NUnit.Framework;
using ScenarioSceneParts;

namespace Tests.ScenarioSceneParts
{
    public class ScenePartsRunnerTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ScenePartsRunnerTestSimplePasses()
        {
            // Use the Assert class to test conditions
            var s = new ScenePartsRunner();
        }

        // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // // `yield return null;` to skip a frame.
        // [UnityTest]
        // public IEnumerator ScenePartsRunnerTestWithEnumeratorPasses()
        // {
        //     // Use the Assert class to test conditions.
        //     // Use yield to skip a frame.
        //     yield return null;
        // }
    }
}