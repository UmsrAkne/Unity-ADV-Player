using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Loaders
{
    public class FilePathListGen : IPathListGen
    {
        private List<string> soundFilePaths;
        private List<string> imageFilePaths;

        public List<string> GetImageFilePaths(string targetDirectoryPath)
        {
            imageFilePaths ??= new List<string>(Directory.GetFiles(targetDirectoryPath))
                .Where(f => Path.GetExtension(f) == ".png" || Path.GetExtension(f) == ".jpg")
                .ToList();

            return imageFilePaths;
        }

        public List<string> GetSoundFilePaths(string targetDirectoryPath)
        {
            soundFilePaths ??= new List<string>(Directory.GetFiles(targetDirectoryPath))
                .Where(f => Path.GetExtension(f) == ".ogg")
                .Select(f => new FileInfo(f).FullName)
                .ToList();

            return soundFilePaths;
        }
    }
}