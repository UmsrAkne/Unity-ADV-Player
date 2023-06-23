using System.Collections.Generic;
using Animations;
using Loaders;
using ScenarioSceneParts;
using SceneContents;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UserInterface;

namespace SceneLogics
{
    public class ScenarioSceneLogic : MonoBehaviour
    {
        private Scenario currentScenario;
        private bool initialized;

        public Resource SceneResource { get; set; }

        private ScenePartsRunner ScenePartsRunner { get; } = new();

        private TextWriter TextWriter { get; } = new();

        private ChapterManager ChapterManager { get; } = new();

        private BGMPlayer BgmPlayer { get; } = new();

        private void Start()
        {
            TextWriter.SetUI(new TextField { Field = GameObject.Find("TextField").GetComponent<Text>() });
            var canvas = GameObject.Find(nameof(Canvas));

            var imageContainers = new List<IDisplayObjectContainer>
            {
                new ImageContainer(canvas) { Index = 0 },
                new ImageContainer(canvas) { Index = 1 },
                new ImageContainer(canvas) { Index = 2 },
            };

            var imageDrawer = new ImageDrawer() { ImageContainers = imageContainers };

            var voicePlayers = new List<VoicePlayer>()
            {
                new() { Channel = 0 },
                new() { Channel = 1 },
                new() { Channel = 2 },
            };

            var list = new List<IScenarioSceneParts>
            {
                imageDrawer,
                ChapterManager,
                new AnimationsManager((ImageContainer)imageContainers[0]),
                new AnimationsManager((ImageContainer)imageContainers[1]),
                new AnimationsManager((ImageContainer)imageContainers[2]),
                new SePlayer(),
                voicePlayers[0],
                voicePlayers[1],
                voicePlayers[2],
                new BgvPlayer(voicePlayers[0]),
                new BgvPlayer(voicePlayers[1]),
                new BgvPlayer(voicePlayers[2]),
            };

            list.ForEach(s =>
            {
                s.SetResource(SceneResource);
                ScenePartsRunner.Add(s);
            });

            BgmPlayer.Execute();
            initialized = true;
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
                    SceneManager.sceneLoaded += LoadSceneResource;
                    SceneManager.LoadScene("LoadScene");
                }
            }
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

        private void LoadSceneResource(Scene next, LoadSceneMode mode)
        {
            var loadSceneLogic = GameObject.Find("Main Camera").GetComponent<LoadSceneLogic>();
            loadSceneLogic.SceneResource = SceneResource;
            SceneManager.sceneLoaded -= LoadSceneResource;
        }
    }
}