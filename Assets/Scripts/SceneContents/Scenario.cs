using System.Collections.Generic;
using Animations;

namespace SceneContents
{
    public class Scenario
    {
        public int Index { get; set; }

        public string Text { get; set; } = string.Empty;

        public string ChapterName { get; set; } = string.Empty;

        public bool MoveMessageWindow { get; set; }

        public List<ImageOrder> ImageOrders { get; set; } = new List<ImageOrder>();

        public List<ImageOrder> DrawOrders { get; set; } = new List<ImageOrder>();

        public List<SoundOrder> VoiceOrders { get; set; } = new List<SoundOrder>();

        public List<BgvOrder> BgvOrders { get; set; } = new List<BgvOrder>();

        // public int VoiceIndex { get; set; }

        public List<SoundOrder> SeOrders { get; set; } = new List<SoundOrder>();

        public List<IAnimation> Animations { get; set; } = new List<IAnimation>();

        public List<StopOrder> StopOrders { get; set; } = new List<StopOrder>();
    }
}