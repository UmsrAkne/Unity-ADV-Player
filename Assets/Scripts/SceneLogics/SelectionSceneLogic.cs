using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Loaders;
using UnityEngine;
using UnityEngine.UI;

namespace SceneLogics
{
    public class SelectionSceneLogic : MonoBehaviour
    {
        private const int LoadThumbnailCountPerOne = 5;
        private int cursorIndex;
        private List<string> selectableScenePaths = new ();
        public GameObject sceneDetailPrefab;
        public List<GameObject> sceneDetails;
        public event EventHandler SceneSelected;

        public string SelectedScenePath { get; set; } = string.Empty;

        private int CursorIndex
        {
            get => cursorIndex;
            set => cursorIndex = Math.Max(0, Math.Min(value, selectableScenePaths.Count - 1));
        }

        private void Start()
        {
            DebugTools.Logger.Add("SelectionScene に入りました");
            sceneDetails = new List<GameObject>(LoadThumbnailCountPerOne)
            {
                Instantiate(sceneDetailPrefab, gameObject.transform),
                Instantiate(sceneDetailPrefab, gameObject.transform),
                Instantiate(sceneDetailPrefab, gameObject.transform),
                Instantiate(sceneDetailPrefab, gameObject.transform),
                Instantiate(sceneDetailPrefab, gameObject.transform),
            };

            // Application.dataPath は Assets フォルダが取得される。
            // 各シナリオは、一段上のディレクトリの中の scenes に入っている。
            var scenesPath = new DirectoryInfo(Application.dataPath).Parent + @"\scenes";
            selectableScenePaths = GetDirectories(scenesPath);

            ReloadThumbnails();
        }

        private void ReloadThumbnails()
        {
            var mainCamera = Camera.main;

            // カメラのビューポート座標 (0, 0.5) は画面の左端の中央を指します
            var leftEdgePosition = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane));
            var materialLoader = new MaterialLoader();

            var loadStartIndex = CursorIndex - (LoadThumbnailCountPerOne / 2);

            for (var i = 0; i < LoadThumbnailCountPerOne; i++)
            {
                var index = loadStartIndex + i;
                if (index < 0 || index >= selectableScenePaths.Count)
                {
                    sceneDetails[i].gameObject.SetActive(false);
                    continue;
                }

                sceneDetails[i].gameObject.SetActive(true);
                var p = selectableScenePaths[index];
                var sceneDetail = sceneDetails[i];

                var t = sceneDetail.GetComponentInChildren<Text>();
                if (t != null)
                {
                    t.text = i == 2 ? p : string.Empty;
                }

                var ren = sceneDetail.GetComponentInChildren<SpriteRenderer>();

                var path = $@"{p}\images\A0101.png";
                var size = materialLoader.GetImageSizeFrom(path);
                var scale = 320.0f / size.Width;
                var spw = materialLoader.LoadImage(path,scale);
                ren.sprite = spw.Sprite;
                var transform1 = ren.transform;

                // オブジェクトの位置を設定
                var pos = new Vector2(leftEdgePosition.x + 160, leftEdgePosition.y - (185 * i));
                transform1.position = new Vector3(pos.x, pos.y, 1.0f);
            }
        }

        private List<string> GetDirectories(string path)
        {
            var directories = new List<string>();

            try
            {
                // 指定したディレクトリ内のディレクトリ一覧を取得
                directories = Directory.GetDirectories(path).ToList();
            }
            catch (Exception e)
            {
                // エラーが発生した場合はエラーメッセージをログに出力
                Debug.LogError("Error getting directories: " + e.Message);
            }

            return directories;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                var c = CursorIndex;
                CursorIndex++;
                if (c != CursorIndex)
                {
                    ReloadThumbnails();
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                var c = CursorIndex;
                CursorIndex--;
                if (c != CursorIndex)
                {
                    ReloadThumbnails();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SelectedScenePath = selectableScenePaths[CursorIndex];
                SceneSelected?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}