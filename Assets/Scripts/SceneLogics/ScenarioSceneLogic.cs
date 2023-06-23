using System.Collections.Generic;
using ScenarioSceneParts;
using SceneContents;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;

namespace SceneLogics
{
    public class ScenarioSceneLogic : MonoBehaviour
    {
        public Resource SceneResource { get; set; }

        private ScenePartsRunner ScenePartsRunner { get; } = new();

        private TextWriter TextWriter { get; } = new();

        private void Start()
        {
            TextWriter.SetUI(new TextField { Field = GameObject.Find("TextField").GetComponent<Text>() });
            var canvas = GameObject.Find(nameof(Canvas));

            var imageDrawer = new ImageDrawer()
            {
                ImageContainers = new List<IDisplayObjectContainer>()
                {
                    new ImageContainer(canvas) { Index = 0 },
                    new ImageContainer(canvas) { Index = 1 },
                    new ImageContainer(canvas) { Index = 2 },
                },
            };

            var list = new List<IScenarioSceneParts>
            {
                imageDrawer,
            };

            list.ForEach(s =>
            {
                s.SetResource(SceneResource);
                ScenePartsRunner.Add(s);
            });
        }
    }
}