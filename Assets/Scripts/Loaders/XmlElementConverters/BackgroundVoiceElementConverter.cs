using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SceneContents;

namespace Loaders.XmlElementConverters
{
    public class BackgroundVoiceElementConverter : IXMLElementConverter
    {
        private readonly string channelAttribute = "channel";
        private readonly string namesAttribute = "names";
        private readonly string panStereoAttribute = "panStereo";


        public string TargetElementName => "backgroundVoice";

        public List<string> Log => new();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName).ToList();

            if (!tags.Any())
            {
                return;
            }

            foreach (var bgvTag in tags)
            {
                var order = new BgvOrder();

                if (bgvTag.Attribute(namesAttribute) != null)
                {
                    var fileNames = XElementHelper.GetStringFromAttribute(bgvTag, namesAttribute);
                    order.FileNames = new List<string>(fileNames.Split(','));
                    order.FileNames = order.FileNames.Select(name => name.Trim()).ToList();
                }

                if (XElementHelper.HasAttribute(bgvTag, panStereoAttribute))
                {
                    order.PanStereo = XElementHelper.GetFloatFromAttribute(bgvTag, panStereoAttribute);
                }

                if (XElementHelper.HasAttribute(bgvTag, channelAttribute))
                {
                    order.Channel = XElementHelper.GetIntFromAttribute(bgvTag, channelAttribute);
                }

                scenario.BgvOrders.Add(order);
            }
        }
    }
}