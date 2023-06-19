namespace Loaders.XmlElementConverters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;

    public class SeElementConverter : IXMLElementConverter
    {
        private readonly string fileNameAttribute = "fileName";
        private readonly string numberAttribute = "number";
        private readonly string repeatCountAttribute = "repeatCount";

        public string TargetElementName => "se";

        public List<string> Log { get; } = new();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName).ToList();

            if (!tags.Any())
            {
                return;
            }

            foreach (var seTag in tags)
            {
                var order = new SoundOrder();

                if (!seTag.Attributes().Any(x => x.Name == numberAttribute || x.Name == fileNameAttribute))
                {
                    Log.Add($"<se> には fileName か number 属性のどちらかが必須です。Index={scenario.Index}");
                }

                if (XElementHelper.HasAttribute(seTag, numberAttribute))
                {
                    order.Index = XElementHelper.GetIntFromAttribute(seTag, numberAttribute);
                }

                if (XElementHelper.HasAttribute(seTag, fileNameAttribute))
                {
                    order.FileName = XElementHelper.GetStringFromAttribute(seTag, fileNameAttribute);
                }

                if (XElementHelper.HasAttribute(seTag, repeatCountAttribute))
                {
                    order.RepeatCount = XElementHelper.GetIntFromAttribute(seTag, repeatCountAttribute);
                }

                scenario.SeOrders.Add(order);
            }
        }
    }
}