namespace SceneContents
{
    public class VoiceOrder
    {
        public int Index { get; set; }

        public string FileName { get; set; } = string.Empty;

        public int Channel { get; set; }

        public bool StopRequest { get; set; }
    }
}