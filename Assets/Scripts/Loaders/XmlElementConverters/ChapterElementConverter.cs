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
            var tags = xmlElement.Elements(TargetElementName);

            if (tags.Count() != 0)
            {
                foreach (var chapterTag in tags)
                {
                    if (chapterTag.Attribute(nameAttribute) != null)
                    {
                        scenario.ChapterName = chapterTag.Attribute(nameAttribute).Value;
                    }
                }
            }
        }
    }
}