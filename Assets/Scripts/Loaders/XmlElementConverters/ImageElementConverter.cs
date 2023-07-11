namespace Loaders.XmlElementConverters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;

    public class ImageElementConverter : IXMLElementConverter
    {
        private readonly List<string> charAttribute = new() { "a", "b", "c", "d" };

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

                if (imageTag.Attribute("scale") != null)
                {
                    order.Scale = XElementHelper.GetDoubleFromAttribute(imageTag, "scale");
                }

                if (imageTag.Attribute("x") != null)
                {
                    order.X = XElementHelper.GetIntFromAttribute(imageTag, "x");
                }

                if (imageTag.Attribute("y") != null)
                {
                    order.Y = XElementHelper.GetIntFromAttribute(imageTag, "y");
                }

                if (imageTag.Attribute("angle") != null)
                {
                    order.Angle = XElementHelper.GetIntFromAttribute(imageTag, "angle");
                }
                
                if (imageTag.Attribute("depth") != null)
                {
                    order.Depth = XElementHelper.GetDoubleFromAttribute(imageTag, "depth");
                }

                if (imageTag.Attribute("mask") != null)
                {
                    order.MaskImageName = XElementHelper.GetStringFromAttribute(imageTag, "mask");
                }

                if (imageTag.Attribute("inheritStatus") != null)
                {
                    order.InheritStatus = bool.Parse(XElementHelper.GetStringFromAttribute(imageTag, "inheritStatus"));
                }

                scenario.ImageOrders.Add(order);
            }
        }
    }
}