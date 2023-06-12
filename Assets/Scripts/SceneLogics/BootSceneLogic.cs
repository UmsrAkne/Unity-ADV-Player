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
            SceneManager.LoadScene("ScenarioScene");
        }

        void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}