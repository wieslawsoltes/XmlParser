#nullable enable
using System.IO;
using System.Xml;
using XmlParser.Factory;

namespace XmlParser
{
    public static class XmlTextReaderParser
    {
        public static void Parse(string str, XmlFactory? factory = null)
        {
            using var stringReader = new StringReader(str);
            var xmlTextReader = new XmlTextReader(stringReader)
            {
                WhitespaceHandling = WhitespaceHandling.Significant
            };

            while (xmlTextReader.Read())
            {
                switch (xmlTextReader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            factory?.PushElement(xmlTextReader.LocalName);
                            while (xmlTextReader.MoveToNextAttribute())
                            {
                                factory?.AddElementAttribute(xmlTextReader.LocalName, xmlTextReader.Value);
                            }
                        }
                        break;
                    case XmlNodeType.EndElement:
                        {
                            factory?.PopElement();
                        }
                        break;
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text:
                    case XmlNodeType.SignificantWhitespace:
                        {
                            factory?.AddElementContent(xmlTextReader.Value);
                        }
                        break;
                    case XmlNodeType.EntityReference:
                        // xmlTextReader.Value
                        break;
                }
            }
        }   
    }
}