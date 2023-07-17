using System;
using System.Collections.Generic;
using Animations;
using Loaders;
using ScenarioSceneParts;
using SceneContents;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;

namespace SceneLogics
{
    public class ScenarioSceneLogic : MonoBehaviour
    {
        private int counter;
        private Scenario currentScenario;
        private bool initialized;

        public Resource SceneResource { get; set; }

        public string ScenarioDirectoryPath { get; set; }

        private ScenePartsRunner ScenePartsRunner { get; } = new();

        private TextWriter TextWriter { get; } = new();

        private ChapterManager ChapterManager { get; } = new();

        private UiContainer UiContainer { get; set; }

        private void Start()
        {
            DebugTools.Logger.Add($"ScenarioSceneLogic : Start() が実行されます");
            LoadResource();
        }

        public void Update()
        {
            if (!initialized)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Forward();

                // if (logWindowObject != null)
                // {
                //     Destroy(logWindowObject);
                //     logWindowObject = null;
                // }
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                TextWriter.SetScenarioIndex(ChapterManager.GetNextChapterIndex());
                Forward();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    TextWriter.SetScenarioIndex(ChapterManager.GetLastChapterIndex());
                    Forward();
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    LoadResource();
                }
            }
        }

        private void Init()
        {
            TextWriter.SetUI(new TextField { Field = GameObject.Find("TextField").GetComponent<Text>() });
            TextWriter.Reload(SceneResource);
            TextWriter.Execute();

            if (initialized)
            {
                ScenePartsRunner.ScenePartsList.ForEach(s => s.Reload(SceneResource));
                return;
            }

            var canvas = GameObject.Find(nameof(Canvas));

            var imageContainers = new List<IDisplayObjectContainer>
            {
                new ImageContainer(canvas) { Index = 0 },
                new ImageContainer(canvas) { Index = 1 },
                new ImageContainer(canvas) { Index = 2 },
            };

            var imageDrawers = new List<ImageDrawer>()
            {
                ScenePartsProvider.GetImageDrawer(0),
                ScenePartsProvider.GetImageDrawer(1),
                ScenePartsProvider.GetImageDrawer(2),
            };

            imageDrawers[0].ImageContainer = imageContainers[0];
            imageDrawers[1].ImageContainer = imageContainers[1];
            imageDrawers[2].ImageContainer = imageContainers[2];

            var list = new List<IScenarioSceneParts>
            {
                imageDrawers[0],
                imageDrawers[1],
                imageDrawers[2],
                ChapterManager,
                new AnimationsManager((ImageContainer)imageContainers[0]),
                new AnimationsManager((ImageContainer)imageContainers[1]),
                new AnimationsManager((ImageContainer)imageContainers[2]),
                new SePlayer(),
                ScenePartsProvider.GetVoicePlayer(0),
                ScenePartsProvider.GetVoicePlayer(1),
                ScenePartsProvider.GetVoicePlayer(2),
                ScenePartsProvider.GetBgvPlayer(0),
                ScenePartsProvider.GetBgvPlayer(1),
                ScenePartsProvider.GetBgvPlayer(2),
            };

            list.ForEach(s =>
            {
                s.SetResource(SceneResource);
                ScenePartsRunner.Add(s);
            });

            UiContainer = new UiContainer()
            {
                BGMPlayer = new BGMPlayer(),
            };

            UiContainer.OverWhite.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            // UiContainer.OverWhite.SetActive(false);
            UiContainer.BGMPlayer.SetResource(SceneResource);
            UiContainer.BGMPlayer.Execute();
            UiContainer.SetScreenWidth(SceneResource.SceneSetting.DefaultImageWidth);

            InvokeRepeating(nameof(EraseOverImage), 0, 0.025f);
            InvokeRepeating(nameof(ExecuteEveryFrames), 0, 0.025f);
        }

        private void LoadResource()
        {
            currentScenario = null;

            var loader = new Loader();
            loader.LoadCompleted += LoaderOnLoadCompleted;
            loader.Load(ScenarioDirectoryPath);
        }

        private void LoaderOnLoadCompleted(object sender, EventArgs e)
        {
            var loader = sender as Loader;
            if (loader == null)
            {
                return;
            }

            SceneResource = loader.Resource;
            Init();
            loader.LoadCompleted -= LoaderOnLoadCompleted;
        }

        private void Forward()
        {
            TextWriter.Execute();

            if (!TextWriter.Writing)
            {
                return;
            }

            if (currentScenario == null || currentScenario != SceneResource.GetScenario(TextWriter.ScenarioIndex))
            {
                currentScenario = SceneResource.GetScenario(TextWriter.ScenarioIndex);
                ScenePartsRunner.Run(currentScenario);
            }
        }

        private void ExecuteEveryFrames()
        {
            if (!initialized)
            {
                return;
            }

            TextWriter.ExecuteEveryFrame();
            ScenePartsRunner.RunEveryFrame();
        }

        private void EraseOverImage()
        {
            if (UiContainer.OverBlack == null)
            {
                initialized = true;
                CancelInvoke(nameof(EraseOverImage));
            }

            counter++;
            if (counter < 40)
            {
                return;
            }

            var r = UiContainer.OverBlack.GetComponent<SpriteRenderer>();
            var a = r.color.a - 0.03f;
            r.color = new Color(0, 0, 0, a);
            if (a <= 0)
            {
                initialized = true;
                CancelInvoke(nameof(EraseOverImage));
            }
        }
    }
}