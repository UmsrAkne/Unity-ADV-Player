using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            // Application.dataPath は Assets フォルダが取得される。
            // 各シナリオは、一段上のディレクトリの中の scenes に入っている。
            var scenesPath = new DirectoryInfo(Application.dataPath).Parent + @"\scenes";

            selectableScenePaths = GetDirectories(scenesPath);

            var cnt = 1;
            foreach (var p in selectableScenePaths)
            {
                // UIテキストをインスタンス化し、リストの内容を表示する
                var sceneDetail = Instantiate(sceneDetailPrefab, gameObject.transform);
                var t = sceneDetail.GetComponentInChildren<Text>();
                if (t != null)
                {
                    t.text = p;
                    var r = t.GetComponent<RectTransform>();
                    r.anchoredPosition = new Vector2(400, -40 * cnt);
                    r.sizeDelta = new Vector2(800, r.sizeDelta.y);
                }

                cnt++;
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