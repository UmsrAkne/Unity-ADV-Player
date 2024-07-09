using System.Collections.Generic;
using UnityEngine;
using UserInterface;

namespace SceneContents
{
    public class EffectLayerProvider : IEffectLayerGettable
    {
        private static IDisplayObject overWhite;
        private IDisplayObject overBlack;

        private static List<IDisplayObject> masks = new List<IDisplayObject>(3) {null, null, null,};

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

        public IDisplayObject GetMask(int containerLayerIndex)
        {
            var m = masks[containerLayerIndex];
            if (m != null)
            {
                return m;
            }

            var d = new DisplayObject(GameObject.Find($"MaskObject-{containerLayerIndex}"));
            masks[containerLayerIndex] = d;
            return d;
        }
    }
}