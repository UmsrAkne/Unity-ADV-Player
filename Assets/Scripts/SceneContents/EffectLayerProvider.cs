using System.Collections.Generic;
using UnityEngine;
using UserInterface;

namespace SceneContents
{
    public class EffectLayerProvider : IEffectLayerGettable
    {
        private static IDisplayObject overWhite;
        private IDisplayObject overBlack;

        public IDisplayObject GetWhiteLayer(int containerLayerIndex)
        {
            if (overWhite != null)
            {
                return overWhite;
            }

            var whiteImage = new ImageSet();
            var oveWhiteSprite = GameObject.Find(nameof(UiContainer.OverWhite)).GetComponent<SpriteRenderer>().sprite;
            var spw = new SpriteWrapper() { Sprite = oveWhiteSprite, };
            whiteImage.Draw(new List<SpriteWrapper>() { spw, }, "EffectLayer");
            overWhite = whiteImage;
            return whiteImage;
        }

        public IDisplayObject GetBlackLayer(int containerLayerIndex)
        {
            if (overBlack != null)
            {
                return overBlack;
            }

            var blackImage = new ImageSet();
            var oveWhiteSprite = GameObject.Find(nameof(UiContainer.OverBlack)).GetComponent<SpriteRenderer>().sprite;
            var spw = new SpriteWrapper() { Sprite = oveWhiteSprite, };
            blackImage.Draw(new List<SpriteWrapper>() { spw, }, "EffectLayer");
            overBlack = blackImage;
            return blackImage;
        }
    }
}