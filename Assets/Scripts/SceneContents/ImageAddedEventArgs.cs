using System;

namespace SceneContents
{
    public class ImageAddedEventArgs : EventArgs
    {
        public IDisplayObject CurrentImageSet { get; set; }
    }
}