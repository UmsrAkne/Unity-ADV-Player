using System.Collections.Generic;

namespace ScenarioSceneParts
{
    public class ScenePartsProvider
    {
        private static List<ImageDrawer> ImageDrawers { get; } = new ();

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
    }
}