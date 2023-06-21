using System;
using System.Collections.Generic;
using SceneContents;

namespace Loaders
{
    public interface IContentsLoader
    {
        List<string> Log { get; }

        Resource Resource { get; set; }

        event EventHandler LoadCompleted;

        void Load(string sceneDirectoryPath);
    }
}