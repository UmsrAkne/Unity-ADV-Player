namespace SceneContents
{
    public interface IEffectLayerGettable
    {
        IDisplayObject GetWhiteLayer(int containerLayerIndex);

        IDisplayObject GetBlackLayer(int containerLayerIndex);

        IDisplayObject GetMask(int containerLayerIndex);
    }
}