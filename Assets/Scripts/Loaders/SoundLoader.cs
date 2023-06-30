using System;
using System.Collections;
using System.IO;
using SceneContents;
using UnityEngine;
using UnityEngine.Networking;

namespace Loaders
{
    public class SoundLoader : MonoBehaviour
    {
        private int loadCounter;

        public ISound Sound { get; set; }

        private AudioClip AudioClip { get; set; }

        public event EventHandler LoadCompleted;

        public void Load(string path)
        {
            Sound ??= new Sound() { AudioSource = new GameObject("AudioSource-Generator").AddComponent<AudioSource>() };

            if (!string.IsNullOrWhiteSpace(path))
            {
                StartCoroutine(LoadAudio(path));
            }
        }

        private IEnumerator LoadAudio(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                yield break;
            }

            using UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.OGGVORBIS);
            req.SendWebRequest();

            while (!req.isDone)
            {
                yield return null;
            }

            AudioClip = DownloadHandlerAudioClip.GetContent(req);

            if (AudioClip.loadState != AudioDataLoadState.Loaded)
            {
                yield break;
            }

            Sound.AudioSource.clip = AudioClip;
            Sound.Available = true;
            LoadCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}