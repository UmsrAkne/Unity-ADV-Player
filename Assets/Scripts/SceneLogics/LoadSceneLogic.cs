using System.Linq;
using Loaders;
using SceneContents;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneLogics
{
    /// <summary>
    /// LoadScene で行う処理を記述するクラスです。
    /// このクラスでは、ScenarioScene で使用するファイルのロードのみを行います。
    /// </summary>
    public class LoadSceneLogic : MonoBehaviour
    {
        public string TargetDirectoryPath { get; set; } = string.Empty;

        public Resource SceneResource { private get; set; }

        // Start is called before the first frame update
        private void Start()
        {
            var loader = new Loader();
            loader.TextLoadCompleted += (_, _) =>
            {
                loader.MediaLoadCompleted += (_, _) =>
                {
                    System.Diagnostics.Debug.WriteLine($"LoadSceneLogic (26) : load completed");
                    SceneManager.sceneLoaded += (_, _) =>
                    {
                        var scenarioSceneLogic = GameObject.Find("Main Camera").GetComponent<ScenarioSceneLogic>();
                        scenarioSceneLogic.SceneResource = loader.Resource;
                    };

                    SceneManager.LoadScene("ScenarioScene");
                };

                loader.LoadMedias(TargetDirectoryPath);
            };

            if (SceneResource != null)
            {
                loader.Recycle(SceneResource);
            }

            loader.LoadTexts(TargetDirectoryPath);

            if (DebugTools.Logger.ErrorMessages.Count != 0)
            {
                GameObject.Find("MessageWindowObject").GetComponent<Text>().text =
                    string.Join(string.Empty, DebugTools.Logger.ErrorMessages);
            }
        }
    }
}