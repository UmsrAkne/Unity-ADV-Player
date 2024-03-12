using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace Loaders.XmlElementConverters
{
    public class XElementHelper
    {
        /// <summary>
        /// 指定した XElement に含まれる attribute の値を文字列で取得します
        /// </summary>
        /// <param name="x">指定する属性名を持つ XElement</param>
        /// <param name="attributeName">XElement に含まれる属性名</param>
        /// <returns>
        /// 指定した属性名が x の中にあれば、その値を取得。それ以外の場合は string.Empty を返します。
        /// </returns>
        public static string GetStringFromAttribute([NotNull] XElement x, string attributeName)
        {
            var att = x.Attribute(attributeName);
            return att == null ? string.Empty : att.Value;
        }

        /// <summary>
        /// 指定した XElement に含まれる attribute の値を整数で取得します。
        /// </summary>
        /// <param name="x">整数に変換可能な属性を持つ XElement</param>
        /// <param name="attributeName">整数に変換可能な属性名</param>
        /// <returns>
        /// 指定した属性名で、変換可能な属性値が XElement に含まれる場合は、その値を返します。
        /// 指定した属性名が見つからない場合、見つかっても値を変換できない場合は 0 を返します。
        /// </returns>
        public static int GetIntFromAttribute([NotNull] XElement x, string attributeName)
        {
            var att = x.Attribute(attributeName);

            if (att == null)
            {
                return 0;
            }

            return int.TryParse(att.Value, out var n) ? n : 0;
        }

        /// <summary>
        /// 指定した XElement に含まれる attribute の値を double で取得します。
        /// </summary>
        /// <param name="x">double に変換可能な属性を持つ XElement</param>
        /// <param name="attributeName">double に変換可能な属性名</param>
        /// <returns>
        /// 指定した属性名で、変換可能な属性値が XElement に含まれる場合は、その値を返します。
        /// 指定した属性名が見つからない場合、見つかっても値を変換できない場合は 0 を返します。
        /// </returns>
        public static double GetDoubleFromAttribute([NotNull] XElement x, string attributeName)
        {
            var att = x.Attribute(attributeName);

            if (att == null)
            {
                return 0;
            }

            return double.TryParse(att.Value, out var n) ? n : 0;
        }

        /// <summary>
        /// 指定した XElement に含まれる attribute の値を float で取得します。
        /// </summary>
        /// <param name="x">float に変換可能な属性を持つ XElement</param>
        /// <param name="attributeName">float に変換可能な属性名</param>
        /// <returns>
        /// 指定した属性名で、変換可能な属性値が XElement に含まれる場合は、その値を返します。
        /// 指定した属性名が見つからない場合、見つかっても値を変換できない場合は 0 を返します。
        /// </returns>
        public static float GetFloatFromAttribute([NotNull] XElement x, string attributeName)
        {
            var att = x.Attribute(attributeName);

            if (att == null)
            {
                return 0;
            }

            return float.TryParse(att.Value, out var n) ? n : 0;
        }

        /// <summary>
        /// 指定した XElement が、指定した属性名の属性を持つかどうかを調べます。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="attributeName"></param>
        /// <returns>指定した属性を持っているかどうかを返します</returns>
        public static bool HasAttribute([NotNull] XElement x, string attributeName)
        {
            return x.Attribute(attributeName) != null;
        }
    }
}