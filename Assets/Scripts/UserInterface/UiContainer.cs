using ScenarioSceneParts;
using UnityEngine;

namespace UserInterface
{
    public class UiContainer
    {
        private bool found;

        public GameObject Canvas { get; } = GameObject.Find(nameof(Canvas));
        
        public GameObject OverBlack { get; } = GameObject.Find(nameof(OverBlack));

        public GameObject OverWhite { get; } = GameObject.Find(nameof(OverWhite));

        public GameObject LefFrame { get; } = GameObject.Find(nameof(LefFrame));

        public GameObject RightFrame { get; } = GameObject.Find(nameof(RightFrame));

        public GameObject MessageWindow { get; } = GameObject.Find(nameof(MessageWindow));

        public BGMPlayer BGMPlayer { get; set; }
    }
}