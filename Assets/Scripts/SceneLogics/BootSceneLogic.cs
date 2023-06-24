using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLogics
{
    public class BootSceneLogic : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            // このメソッド内の処理はアプリ起動時に実行される。

            SceneManager.sceneLoaded += NextSceneLoaded;

            SceneManager.LoadScene("ScenarioScene");
            // SceneManager.LoadScene("LoadScene");
        }

        private static void NextSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            // var loadingSceneLogic = GameObject.Find("Main Camera").GetComponent<LoadSceneLogic>();
            // loadingSceneLogic.TargetDirectoryPath = @"scenes\sampleScn001";
            // SceneManager.sceneLoaded -= NextSceneLoaded;

            var scenarioSceneLogic = GameObject.Find("Main Camera").GetComponent<ScenarioSceneLogic>();
            scenarioSceneLogic.ScenarioDirectoryPath = @"scenes\sampleScn001";
            SceneManager.sceneLoaded -= NextSceneLoaded;
        }
    }
}