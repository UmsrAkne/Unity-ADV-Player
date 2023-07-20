using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using SceneContents;

namespace Loaders.XmlElementConverters
{
    public class BlinkElementConverter
    {
        private readonly XName childElementName = "image";
        private readonly XName baseImageNameAttributeName = "baseImageName";
        private readonly XName namesAttributeName = "names";

        public string TargetElementName => "blink";

        /// <summary>
        /// image 要素を含む　blink 要素から BlinkOrder を生成します。
        /// image 要素は以下の属性を記述します。
        /// baseImageName : 単体の画像ファイル名を入力
        /// baseImageName : カンマ区切りで複数の画像ファイル名を入力
        /// いずれの属性も、拡張子付き、拡張子なしの両方の記述に対応します。
        /// 但し、生成される BlinkOrder に入力されるのはいずれの場合も拡張子なしのファイル名のみです。
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <returns></returns>
        public List<BlinkOrder> Convert(XElement xmlElement)
        {
            var tags = xmlElement.Elements(childElementName).ToList();
            var orders = new List<BlinkOrder>();

            if (xmlElement.Name != TargetElementName || !tags.Any())
            {
                return orders;
            }
            
            return tags.Select(imageTag =>
            {
                var order = new BlinkOrder();
                order.BaseImageName = XElementHelper.GetStringFromAttribute(imageTag, baseImageNameAttributeName.ToString());
                order.BaseImageName = Path.GetFileNameWithoutExtension(order.BaseImageName);
                
                var names = XElementHelper.GetStringFromAttribute(imageTag, namesAttributeName.ToString());
                order.Names = names.Replace(" ", "")
                    .Split(',')
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToList();
                
                return order;
            }).ToList();
        }
    }
}