using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLogics
{
    public class BootSceneLogic : MonoBehaviour
    {
        private static string currentSceneDirectoryPath = string.Empty;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            // このメソッド内の処理はアプリ起動時に実行される。
            
            DebugTools.Logger.Add($"--------------------------------------------------");
            DebugTools.Logger.Add($"BootSceneLogic : アプリを起動しました");

            SceneManager.sceneLoaded += NextSceneLoaded;

            SceneManager.LoadScene("SelectionScene");
            // SceneManager.LoadScene("ScenarioScene");
            // SceneManager.LoadScene("LoadScene");
        }

        private static void NextSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            var loadingSceneLogic = GameObject.Find("Main Camera")?.GetComponent<LoadSceneLogic>();
            if (loadingSceneLogic != null)
            {
                loadingSceneLogic.TargetDirectoryPath = currentSceneDirectoryPath;
                // loadingSceneLogic.TargetDirectoryPath = @"scenes\sampleScn001";
                SceneManager.sceneLoaded -= NextSceneLoaded;
            }

            // var scenarioSceneLogic = GameObject.Find("Main Camera").GetComponent<ScenarioSceneLogic>();
            // scenarioSceneLogic.ScenarioDirectoryPath = @"scenes\sampleScn001";
            // SceneManager.sceneLoaded -= NextSceneLoaded;

            var selectionSceneLogic = GameObject.Find("Canvas")?.GetComponent<SelectionSceneLogic>();
            if (selectionSceneLogic != null)
            {
                selectionSceneLogic.SceneSelected += SelectionSceneLogicOnSceneSelected;
            }
        }

        private static void SelectionSceneLogicOnSceneSelected(object sender, EventArgs e)
        {
            currentSceneDirectoryPath = ((SelectionSceneLogic)sender).SelectedScenePath;
            SceneManager.LoadScene("LoadScene");
        }
    }
}