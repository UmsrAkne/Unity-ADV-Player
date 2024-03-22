using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SceneLogics
{
    public class SelectionSceneLogic : MonoBehaviour
    {
        private int cursorIndex;
        private List<string> selectableScenePaths = new ();

        private int CursorIndex
        {
            get => cursorIndex;
            set => cursorIndex = Math.Max(0, Math.Min(value, selectableScenePaths.Count - 1));
        }

        private void Start()
        {
            Debug.Log("SelectionScene に入りました");

            // Application.dataPath は Assets フォルダが取得される。
            // 各シナリオは、一段上のディレクトリの中の scenes に入っている。
            var scenesPath = new DirectoryInfo(Application.dataPath).Parent + @"\scenes";

            selectableScenePaths = GetDirectories(scenesPath);

            foreach (var p in selectableScenePaths)
            {
                Debug.Log(p);
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
            catch (System.Exception e)
            {
                // エラーが発生した場合はエラーメッセージをログに出力
                Debug.LogError("Error getting directories: " + e.Message);
            }

            return directories;
        }
    }
}