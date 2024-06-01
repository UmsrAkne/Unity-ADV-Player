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
        private int cursorIndex;
        private List<string> selectableScenePaths = new ();
        public GameObject sceneDetailPrefab;

        private int CursorIndex
        {
            get => cursorIndex;
            set => cursorIndex = Math.Max(0, Math.Min(value, selectableScenePaths.Count - 1));
        }

        private void Start()
        {
            DebugTools.Logger.Add("SelectionScene に入りました");

            var mainCamera = Camera.main;

            // カメラのビューポート座標 (0, 0.5) は画面の左端の中央を指します
            var leftEdgePosition = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane));

            // Application.dataPath は Assets フォルダが取得される。
            // 各シナリオは、一段上のディレクトリの中の scenes に入っている。
            var scenesPath = new DirectoryInfo(Application.dataPath).Parent + @"\scenes";

            selectableScenePaths = GetDirectories(scenesPath);
            var materialLoader = new MaterialLoader();

            for (var i = 0; i < selectableScenePaths.Count; i++)
            {
                var p = selectableScenePaths[i];
                // UIテキストをインスタンス化し、リストの内容を表示する
                var sceneDetail = Instantiate(sceneDetailPrefab, gameObject.transform);
                sceneDetail.transform.position = new Vector3(0, 0, i);

                var t = sceneDetail.GetComponentInChildren<Text>();
                if (t != null)
                {
                    t.text = p;
                    var r = t.GetComponent<RectTransform>();
                    r.anchoredPosition = new Vector2(400, -40 * i);
                    r.sizeDelta = new Vector2(800, r.sizeDelta.y);
                }

                var ren = sceneDetail.GetComponentInChildren<SpriteRenderer>();
                var spw = materialLoader.LoadImage($@"{p}\images\A0101.png");
                ren.sprite = spw.Sprite;
                var scale = 320.0f / spw.Width;
                var transform1 = ren.transform;
                transform1.localScale = new Vector3(scale, scale, 1.0f);

                // オブジェクトの位置を設定
                var pos = new Vector2(leftEdgePosition.x + 160, leftEdgePosition.y - (180 * i));
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
    }
}