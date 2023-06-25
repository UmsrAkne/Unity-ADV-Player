using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Loaders
{
    public class FilePathListGen : IPathListGen
    {
        public List<string> GetImageFilePaths(string targetDirectoryPath)
        {
            var allFilePaths = new List<string>(Directory.GetFiles(targetDirectoryPath));
            return allFilePaths.Where(f => Path.GetExtension(f) == ".png" || Path.GetExtension(f) == ".jpg").ToList();
        }

        public List<string> GetSoundFilePaths(string targetDirectoryPath)
        {
            var allFilePaths = new List<string>(Directory.GetFiles(targetDirectoryPath));
            return allFilePaths.Where(f => Path.GetExtension(f) == ".ogg").ToList();
        }
    }
}