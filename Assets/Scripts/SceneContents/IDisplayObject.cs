namespace SceneContents
{
    public interface IDisplayObject
    {
        float Alpha { get; set; }

        double Scale { get; set; }

        float X { get; set; }

        float Y { get; set; }

        int Angle { get; set; }
    }
}