using System.Collections.Generic;
using Loaders;

namespace Tests.Loaders
{
    public class DummyPathListGen : IPathListGen
    {
        public List<string> ImageFilePaths { get; set; } = new();

        public List<string> SoundFilePaths { get; set; } = new();

        public List<string> GetImageFilePaths(string targetDirectoryPath)
        {
            return ImageFilePaths;
        }

        public List<string> GetSoundFilePaths(string targetDirectoryPath)
        {
            return SoundFilePaths;
        }
    }
}