namespace Loaders.XmlElementConverters
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using SceneContents;

    public interface IXMLElementConverter
    {
        string TargetElementName { get; }

        List<string> Log { get; }

        void Convert(XElement xmlElement, Scenario scenario);
    }
}