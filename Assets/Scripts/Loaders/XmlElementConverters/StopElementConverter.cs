using ScenarioSceneParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SceneContents;

namespace Loaders.XmlElementConverters
{
    public class StopElementConverter : IXMLElementConverter
    {
        private readonly string channelAttribute = "channel";
        private readonly string layerIndexAttribute = "layerIndex";
        private readonly string nameAttribute = "name";
        private readonly string targetAttribute = "target";

        public string TargetElementName => "stop";

        public List<string> Log { get; } = new();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName).ToList();

            if (!tags.Any())
            {
                return;
            }

            foreach (var stopTag in tags)
            {
                var stopOrder = GenerateStopOrder(stopTag, scenario);
                scenario.StopOrders.Add(stopOrder);
            }
        }

        private StopOrder GenerateStopOrder(XElement stopTag, Scenario scenario)
        {
            var stopOrder = new StopOrder();

            if (stopTag.Attributes().All(x => x.Name != targetAttribute))
            {
                Log.Add($"<stop> には target 属性が必須です。Index={scenario.Index}");
            }
            else
            {
                var val = XElementHelper.GetStringFromAttribute(stopTag, targetAttribute);
                var upperAttValue = val[..1].ToUpper() + val[1..];
                if (Enum.TryParse(upperAttValue, out StoppableSceneParts d))
                {
                    stopOrder.Target = d;
                }
                else
                {
                    Log.Add($"target 属性の変換に失敗。Index={scenario.Index}");
                }
            }

            if (XElementHelper.HasAttribute(stopTag, layerIndexAttribute))
            {
                stopOrder.LayerIndex = XElementHelper.GetIntFromAttribute(stopTag, layerIndexAttribute);
            }

            if (XElementHelper.HasAttribute(stopTag, channelAttribute))
            {
                stopOrder.Channel = XElementHelper.GetIntFromAttribute(stopTag, channelAttribute);
            }

            if (XElementHelper.HasAttribute(stopTag, nameAttribute))
            {
                stopOrder.Name = XElementHelper.GetStringFromAttribute(stopTag, nameAttribute);
            }

            return stopOrder;
        }
    }
}