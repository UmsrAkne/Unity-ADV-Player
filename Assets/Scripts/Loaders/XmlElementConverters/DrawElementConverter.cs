using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SceneContents;

namespace Loaders.XmlElementConverters
{
    public class DrawElementConverter : IXMLElementConverter
    {
        private readonly List<string> charaAttribute = new() { "a", "b", "c", "d" };
        private readonly string depthAttribute = "depth";
        private readonly string targetLayerIndexAttribute = "targetLayerIndex";

        public string TargetElementName => "draw";

        public List<string> Log { get; } = new();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName).ToList();

            if (!tags.Any())
            {
                return;
            }

            foreach (var imageTag in tags)
            {
                var order = new ImageOrder() { IsDrawOrder = true };

                if (imageTag.Attributes()
                    .Any(x => x.Name == "a" || x.Name == "b" || x.Name == "c" || x.Name == "d"))
                {
                    charaAttribute.ForEach(s =>
                    {
                        var imageName = XElementHelper.GetStringFromAttribute(imageTag, s);
                        order.Names.Add(imageName);
                    });
                }

                if (XElementHelper.HasAttribute(imageTag, depthAttribute))
                {
                    order.Depth = XElementHelper.GetDoubleFromAttribute(imageTag, depthAttribute);
                }

                if (XElementHelper.HasAttribute(imageTag, targetLayerIndexAttribute))
                {
                    order.TargetLayerIndex = XElementHelper.GetIntFromAttribute(imageTag, targetLayerIndexAttribute);
                }

                scenario.DrawOrders.Add(order);
            }
        }
    }
}