using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLogics
{
    public class BootSceneLogic : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            // このメソッド内の処理はアプリ起動時に実行される。

            SceneManager.sceneLoaded += (_, _) =>
            {
                var loadingSceneLogic = GameObject.Find("Main Camera").GetComponent<LoadSceneLogic>();
                loadingSceneLogic.TargetDirectoryPath = @"scenes\sampleScn001";
            };

            // SceneManager.LoadScene("ScenarioScene");
            SceneManager.LoadScene("LoadScene");
        }

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}