using UnityEngine;
using UnityEngine.UI;

namespace SceneLogics
{
    public class ScenarioSceneLogic : MonoBehaviour
    {
        private Text TextField { get; set; }

        private void Awake()
        {
            var g = GameObject.Find("TextField");
            TextField = g.GetComponent<Text>();
        }
    }
}