namespace Loaders.XmlElementConverters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;

    public class VoiceElementConverter : IXMLElementConverter
    {
        private readonly string numberAttribute = "number";
        private readonly string fileNameAttribute = "fileName";
        private readonly string channelAttribute = "channel";

        public string TargetElementName => "voice";

        public List<string> Log { get; } = new List<string>();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName);

            // if (tags.Count() != 0)
            // {
            //     foreach (XElement voiceTag in tags)
            //     {
            //         var order = new VoiceOrder();

            //         if (!voiceTag.Attributes().Any(x => x.Name == numberAttribute || x.Name == fileNameAttribute))
            //         {
            //             Log.Add($"<voice> には fileName か number 属性のどちらかが必須です。Index={scenario.Index}");
            //         }

            //         if (voiceTag.Attribute(numberAttribute) != null)
            //         {
            //             order.Index = int.Parse(voiceTag.Attribute(numberAttribute).Value);
            //         }

            //         if (voiceTag.Attribute(fileNameAttribute) != null)
            //         {
            //             order.FileName = voiceTag.Attribute(fileNameAttribute).Value;
            //         }

            //         if (voiceTag.Attribute(channelAttribute) != null)
            //         {
            //             order.Channel = int.Parse(voiceTag.Attribute(channelAttribute).Value);
            //         }

            //         scenario.VoiceOrders.Add(order);
            //     }
            // }
        }
    }
}