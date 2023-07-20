using System.Collections.Generic;
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
                var names = XElementHelper.GetStringFromAttribute(imageTag, namesAttributeName.ToString());
                order.Names = names.Replace(" ", "").Split(',').ToList();
                return order;
            }).ToList();
        }
    }
}