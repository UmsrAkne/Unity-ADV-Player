using System;

namespace SceneContents
{
    public class ImageAddedEventArgs : EventArgs
    {
        public IDisplayObject CurrentImageSet { get; set; }

        public ImageOrder CurrentOrder { get; set; }
    }
}