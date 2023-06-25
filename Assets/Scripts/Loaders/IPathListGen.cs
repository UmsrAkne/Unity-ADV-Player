using System.Collections.Generic;

namespace Loaders
{
    public interface IPathListGen
    {
        /// <summary>
        /// 指定ディレクトリに存在する画像ファイルのパスのリストを返します
        /// 画像ファイルと認識されるのは、拡張子が png, jpg のいずれかの時のみです。
        /// </summary>
        /// <param name="targetDirectoryPath">ファイルリストを列挙したいディレクトリのパス</param>
        /// <returns>画像ファイルのパスリスト</returns>
        List<string> GetImageFilePaths(string targetDirectoryPath);

        /// <summary>
        /// 指定ディレクトリに存在するサウンドファイルのパスのリストを返します。
        /// サウンドファイルと認識されるのは ogg のみです。
        /// </summary>
        /// <param name="targetDirectoryPath">ファイルのリストを列挙したいディレクトリのパス</param>
        /// <returns>サウンドファイルのリスト</returns>
        List<string> GetSoundFilePaths(string targetDirectoryPath);
    }
}