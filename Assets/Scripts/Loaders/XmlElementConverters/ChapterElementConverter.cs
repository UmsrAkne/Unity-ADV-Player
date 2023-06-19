namespace Loaders.XmlElementConverters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;

    public class ChapterElementConverter : IXMLElementConverter
    {
        private readonly string nameAttribute = "name";

        public string TargetElementName => "chapter";

        public List<string> Log { get; } = new();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName).ToList();

            if (!tags.Any())
            {
                return;
            }

            foreach (var cTag in tags.Where(c => XElementHelper.HasAttribute(c, nameAttribute)))
            {
                scenario.ChapterName = XElementHelper.GetStringFromAttribute(cTag, nameAttribute);
            }
        }
    }
}