using System.Collections.Generic;
using SceneContents;
using UnityEngine;

namespace Loaders
{
    public class SceneSetting
    {
        public int DefaultImageWidth { get; set; } = 1280;

        public int DefaultImageHeight { get; set; } = 720;

        public int BGMNumber { get; internal set; }

        public string BGMFileName { get; set; } = string.Empty;

        public float BGMVolume { get; set; } = 1.0f;

        public float SeVolume { get; set; } = 1.0f;

        public float VoiceVolume { get; set; } = 1.0f;

        public float BgvVolume { get; set; } = 1.0f;

        public float MessageWindowAlpha { get; set; } = 0.6f;

        public Vector2 MessageWindowPos { get; set; } = new (0, 0);

        public List<ImageLocation> ImageLocations { get; set; } = new();

        public List<BlinkOrder> BlinkOrders { get; set; } = new ();

        public override string ToString()
        {
            return $"BGMNumber={BGMNumber}, BGMFileName={BGMFileName}, BGMVolume={BGMVolume}, VcVolume={VoiceVolume}, SeVolume={SeVolume}, ImageWidth={DefaultImageWidth}";
        }
    }
}