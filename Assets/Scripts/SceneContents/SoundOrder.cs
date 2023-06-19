namespace SceneContents
{
    public class SoundOrder
    {
        public int Index { get; set; }

        public string FileName { get; set; } = string.Empty;

        public int RepeatCount { get; set; }

        public double Volume { get; set; } = 1.0;

        public int Channel { get; set; }

        public bool StopRequest { get; set; }
    }
}