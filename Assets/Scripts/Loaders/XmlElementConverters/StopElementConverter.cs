using ScenarioSceneParts;

namespace Loaders.XmlElementConverters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;

    public class StopElementConverter : IXMLElementConverter
    {
        private string channelAttribute = "channel";
        private string layerIndexAttribute = "layerIndex";
        private string nameAttribute = "name";
        private string targetAttribute = "target";

        public string TargetElementName => "stop";

        public List<string> Log { get; } = new();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName);

            if (tags.Count() != 0)
            {
                foreach (var stopTag in tags)
                {
                    var stopOrder = new StopOrder();

                    if (!stopTag.Attributes().Any(x => x.Name == targetAttribute))
                    {
                        Log.Add($"<stop> には target 属性が必須です。Index={scenario.Index}");
                    }
                    else
                    {
                        var val = stopTag.Attribute(targetAttribute).Value;
                        var upperAttValue = val.Substring(0, 1).ToUpper() + val.Substring(1);
                        if (Enum.TryParse(upperAttValue, out StoppableSceneParts d))
                        {
                            stopOrder.Target = d;
                        }
                        else
                        {
                            Log.Add($"target 属性の変換に失敗。Index={scenario.Index}");
                        }
                    }

                    if (stopTag.Attribute(layerIndexAttribute) != null)
                    {
                        stopOrder.LayerIndex = int.Parse(stopTag.Attribute(layerIndexAttribute).Value);
                    }

                    if (stopTag.Attribute(channelAttribute) != null)
                    {
                        stopOrder.Channel = int.Parse(stopTag.Attribute(channelAttribute).Value);
                    }

                    if (stopTag.Attribute(nameAttribute) != null)
                    {
                        stopOrder.Name = stopTag.Attribute(nameAttribute).Value;
                    }

                    scenario.StopOrders.Add(stopOrder);
                }
            }
        }
    }
}