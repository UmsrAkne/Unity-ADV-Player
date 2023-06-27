using System.Collections.Generic;
using System.Linq;
using Loaders;

namespace Tests.Loaders
{
    public class DummyPathListGen : IPathListGen
    {
        public List<string> ImageFilePaths { get; set; } = new();

        public List<string> SoundFilePaths { get; set; } = new();

        public List<string> GetImageFilePaths(string targetDirectoryPath)
        {
            return ImageFilePaths.Select(p => $@"{targetDirectoryPath}\{p}").ToList();
        }

        public List<string> GetSoundFilePaths(string targetDirectoryPath)
        {
            return SoundFilePaths.Select(p => $@"{targetDirectoryPath}\{p}").ToList();
        }
    }
}