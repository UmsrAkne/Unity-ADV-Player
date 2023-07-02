namespace SceneContents
{
    public interface IEffectLayerGettable
    {
        IDisplayObject GetWhiteLayer(int containerLayerIndex);

        IDisplayObject GetBlackLayer(int containerLayerIndex);
    }
}