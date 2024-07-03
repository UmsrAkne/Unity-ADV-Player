namespace Loaders.XmlElementConverters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;

    public class ImageElementConverter : IXMLElementConverter
    {
        private readonly List<string> charAttribute = new() { "a", "b", "c", "d" };

        private readonly string xAttributeName = nameof(ImageOrder.X).ToLower(0);
        private readonly string yAttributeName = nameof(ImageOrder.Y).ToLower(0);

        private readonly string scaleAttributeName = nameof(ImageOrder.Scale).ToLower(0);
        private readonly string angleAttributeName = nameof(ImageOrder.Angle).ToLower(0);
        private readonly string depthAttributeName = nameof(ImageOrder.Depth).ToLower(0);
        private readonly string delayAttributeName = nameof(ImageOrder.Delay).ToLower(0);
        private readonly string durationAttributeName = nameof(ImageOrder.Duration).ToLower(0);
        private readonly string inheritStatusAttributeName = nameof(ImageOrder.InheritStatus).ToLower(0);
        private readonly string targetLayerIndexAttribute = nameof(ImageOrder.TargetLayerIndex).ToLower(0);

        public string TargetElementName => "image";

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
                var order = new ImageOrder();

                if (imageTag.Attributes()
                    .Any(x => x.Name == "a" || x.Name == "b" || x.Name == "c" || x.Name == "d"))
                {
                    charAttribute.ForEach(s =>
                    {
                        var name = imageTag.Attribute(s) != null
                            ? XElementHelper.GetStringFromAttribute(imageTag, s)
                            : string.Empty;

                        order.Names.Add(name);
                    });
                }
                else
                {
                    Log.Add($"image要素に a, b. c, d 属性が含まれていません。Index={scenario.Index}");
                }

                if (XElementHelper.HasAttribute(imageTag, targetLayerIndexAttribute))
                {
                    order.TargetLayerIndex = XElementHelper.GetIntFromAttribute(imageTag, targetLayerIndexAttribute);
                }

                if (XElementHelper.HasAttribute(imageTag, scaleAttributeName))
                {
                    order.Scale = XElementHelper.GetDoubleFromAttribute(imageTag, scaleAttributeName);
                }
                
                if (XElementHelper.HasAttribute(imageTag, xAttributeName))
                {
                    order.X = XElementHelper.GetIntFromAttribute(imageTag, xAttributeName);
                }
                
                if (XElementHelper.HasAttribute(imageTag, yAttributeName))
                {
                    order.Y = XElementHelper.GetIntFromAttribute(imageTag, yAttributeName);
                }
                
                if (XElementHelper.HasAttribute(imageTag, angleAttributeName))
                {
                    order.Angle = XElementHelper.GetIntFromAttribute(imageTag, angleAttributeName);
                }
                
                if (XElementHelper.HasAttribute(imageTag, depthAttributeName))
                {
                    order.Depth = XElementHelper.GetDoubleFromAttribute(imageTag, depthAttributeName);
                }

                if (XElementHelper.HasAttribute(imageTag, durationAttributeName))
                {
                    order.Duration = XElementHelper.GetIntFromAttribute(imageTag, durationAttributeName);
                }

                if (XElementHelper.HasAttribute(imageTag, delayAttributeName))
                {
                    order.Delay = XElementHelper.GetIntFromAttribute(imageTag, delayAttributeName);
                }
                
                if (XElementHelper.HasAttribute(imageTag, inheritStatusAttributeName))
                {
                    order.InheritStatus = bool.Parse(XElementHelper.GetStringFromAttribute(imageTag, inheritStatusAttributeName));
                }
                
                if (XElementHelper.HasAttribute(imageTag, "mask"))
                {
                    order.MaskImageName = XElementHelper.GetStringFromAttribute(imageTag, "mask");
                }
                
                scenario.ImageOrders.Add(order);
            }
        }
    }
}