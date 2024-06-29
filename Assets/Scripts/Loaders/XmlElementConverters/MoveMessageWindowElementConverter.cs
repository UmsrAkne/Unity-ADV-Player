namespace Loaders.XmlElementConverters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;

    public class MoveMessageWindowElementConverter : IXMLElementConverter
    {
        public string TargetElementName => "moveMessageWindow";

        public List<string> Log { get; } = new();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName).ToList();

            if (!tags.Any())
            {
                return;
            }

            scenario.MoveMessageWindow = true;
        }
    }
}