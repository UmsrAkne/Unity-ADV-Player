using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Editor
{
    public class PostBuildProcessor : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            var documentFilePath = "Assets/Documents/Document.md";
            var updateHistoryFilePath = "Assets/Documents/UpdateHistory.md";
            var createScenesDirScriptPath = "createScenesDir.sh";

            // summary.outputPath から .exe が取得される。
            var outputPath = Path.GetDirectoryName(report.summary.outputPath) ?? string.Empty;

            var updateHistoryFileDestinationPath = Path.Combine(outputPath, "UpdateHistory.md");
            var documentFileDestinationPath = Path.Combine(outputPath, "Document.md");
            var createScenesDirScriptDestinationPath = Path.Combine(outputPath, "createScenesDir.sh");

            if (File.Exists(updateHistoryFilePath))
            {
                File.Copy(updateHistoryFilePath, updateHistoryFileDestinationPath, true);
            }

            if (File.Exists(documentFilePath))
            {
                File.Copy(documentFilePath, documentFileDestinationPath, true);
            }

            if (File.Exists(createScenesDirScriptPath))
            {
                File.Copy(createScenesDirScriptPath, createScenesDirScriptDestinationPath, true);
            }
        }
    }
}