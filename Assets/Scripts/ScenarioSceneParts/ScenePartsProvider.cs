using System.Collections.Generic;

namespace ScenarioSceneParts
{
    public static class ScenePartsProvider
    {
        private static List<ImageDrawer> ImageDrawers { get; } = new ();
        private static List<VoicePlayer> VoicePlayers { get; } = new ();
        private static List<BgvPlayer> BgvPlayers { get; } = new ();

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
            var vps = VoicePlayers;
            while (vps.Count <= channel)
            {
                vps.Add(null);
            }

            return vps[channel] ?? (vps[channel] = new VoicePlayer() { Channel = channel });
        }
        
        /// <summary>
        /// 指定したチャンネルの VoicePlayer　をセットした BgvPlayer を取得します。
        /// このメソッドでは、BgvPlayer を生成する際、内部で保持されている VoicePlayer をコンストラクタの引数にします。
        /// 指定したチャンネルの VoicePlayer が存在しない場合は、新たに生成してそれを引数とします。
        /// </summary>
        /// <param name="channel">BgvPlayer にセットする VoicePlayer のチャンネルをセットします</param>
        /// <returns>指定したチャンネルの VoicePlayer がセットされた BgvPlayer</returns>
        public static BgvPlayer GetBgvPlayer(int channel)
        {
            var vps = BgvPlayers;
            while (vps.Count <= channel)
            {
                vps.Add(null);
            }

            return vps[channel] ?? (vps[channel] = new BgvPlayer(GetVoicePlayer(channel)));
        }
    }
}