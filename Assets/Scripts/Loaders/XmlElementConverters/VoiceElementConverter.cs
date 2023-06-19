using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SceneContents;

namespace Loaders.XmlElementConverters
{
    public class VoiceElementConverter : IXMLElementConverter
    {
        private readonly string numberAttribute = "number";
        private readonly string fileNameAttribute = "fileName";
        private readonly string channelAttribute = "channel";

        public string TargetElementName => "voice";

        public List<string> Log { get; } = new List<string>();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName).ToList();

            if (!tags.Any())
            {
                return;
            }

            foreach (XElement voiceTag in tags)
            {
                var order = new SoundOrder();

                if (!voiceTag.Attributes().Any(x => x.Name == numberAttribute || x.Name == fileNameAttribute))
                {
                    Log.Add($"<voice> には fileName か number 属性のどちらかが必須です。Index={scenario.Index}");
                }

                if (XElementHelper.HasAttribute(voiceTag, numberAttribute))
                {
                    order.Index = XElementHelper.GetIntFromAttribute(voiceTag, numberAttribute);
                }

                if (XElementHelper.HasAttribute(voiceTag, fileNameAttribute))
                {
                    order.FileName = XElementHelper.GetStringFromAttribute(voiceTag, fileNameAttribute);
                }

                if (XElementHelper.HasAttribute(voiceTag, channelAttribute))
                {
                    order.Channel = XElementHelper.GetIntFromAttribute(voiceTag, channelAttribute);
                }

                scenario.VoiceOrders.Add(order);
            }
        }
    }
}