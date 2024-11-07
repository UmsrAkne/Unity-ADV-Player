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
        private readonly string volumeAttribute = "volume";
        private readonly string repeatCountAttribute = "repeatCount";
        private readonly string delayAttribute = "delay";
        private readonly string channelAttribute = "channel";

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

                if (XElementHelper.HasAttribute(seTag, channelAttribute))
                {
                    order.Channel = XElementHelper.GetIntFromAttribute(seTag, channelAttribute);
                }

                if (XElementHelper.HasAttribute(seTag, repeatCountAttribute))
                {
                    order.RepeatCount = XElementHelper.GetIntFromAttribute(seTag, repeatCountAttribute);
                }

                if (XElementHelper.HasAttribute(seTag, volumeAttribute))
                {
                    var v = XElementHelper.GetFloatFromAttribute(seTag, volumeAttribute);
                    order.Volume = v != 0 ? XElementHelper.GetFloatFromAttribute(seTag, volumeAttribute) : order.Volume;
                }

                if (XElementHelper.HasAttribute(seTag, delayAttribute))
                {
                    var d = XElementHelper.GetFloatFromAttribute(seTag, delayAttribute);
                    order.Delay = d != 0 ? d : order.Delay;
                }

                scenario.SeOrders.Add(order);
            }
        }
    }
}