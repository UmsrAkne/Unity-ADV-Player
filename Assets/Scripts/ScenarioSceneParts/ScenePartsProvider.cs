using System.Collections.Generic;

namespace ScenarioSceneParts
{
    public static class ScenePartsProvider
    {
        private static List<ImageDrawer> ImageDrawers { get; } = new ();
        private static List<VoicePlayer> VoicePlayers { get; } = new ();

        public static ImageDrawer GetImageDrawer(int index)
        {
            if (index < ImageDrawers.Count)
            {
                return ImageDrawers[index];
            }

            while (ImageDrawers.Count <= index)
            {
                ImageDrawers.Add(new ImageDrawer());
            }

            return ImageDrawers[index];
        }
        
        /// <summary>
        /// 指定のチャンネルを設定した VoicePlayer インスタンスを生成、取得します。
        /// オブジェクトの生成は、一つのチャンネルにつき一回のみ行われます。
        /// 生成したオブジェクトは内部で保存され、２回目以降の呼び出し時には既成のインスタンスを返します。
        /// </summary>
        /// <param name="channel">VoicePlayer にセットされるチャンネルを指定します。</param>
        /// <returns>指定したチャンネルが設定された VoicePlayer</returns>
        public static VoicePlayer GetVoicePlayer(int channel)
        {
            if (channel < VoicePlayers.Count)
            {
                return VoicePlayers[channel];
            }

            while (VoicePlayers.Count <= channel)
            {
                VoicePlayers.Add(new VoicePlayer() { Channel = channel });
            }

            return VoicePlayers[channel];
        }
    }
}