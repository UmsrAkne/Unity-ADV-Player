using Loaders;
using UnityEngine;

namespace SceneLogics
{
    /// <summary>
    /// LoadScene で行う処理を記述するクラスです。
    /// このクラスでは、ScenarioScene で使用するファイルのロードのみを行います。
    /// </summary>
    public class LoadSceneLogic : MonoBehaviour
    {
        public string TargetDirectoryPath { get; set; } = string.Empty;

        // Start is called before the first frame update
        private void Start()
        {
            var loader = new Loader();
            loader.TextLoadCompleted += (_, _) =>
            {
                System.Diagnostics.Debug.WriteLine($"LoadSceneLogic (15) : {loader.Resource}");
            };

            loader.LoadTexts(TargetDirectoryPath);
        }
    }
}