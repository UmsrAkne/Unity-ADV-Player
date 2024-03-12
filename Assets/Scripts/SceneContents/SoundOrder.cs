namespace SceneContents
{
    public class SoundOrder
    {
        public int Index { get; set; }

        public string FileName { get; set; } = string.Empty;

        public int RepeatCount { get; set; }

        /// <summary>
        /// オブジェクトの音量を表すプロパティです。
        /// デフォルトの値は null であり、この場合、音量が設定されていないことを示します。
        /// </summary>
        public double? Volume { get; set; }

        public bool ChangedVolume { get; set; }

        public int Channel { get; set; }

        public bool StopRequest { get; set; }
    }
}