#nullable enable
//#define CONSOLE_DEBUG
#define CREATE_ELEMENTS
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace XmlParser
{
    public static class XmlTextReaderParser
    {
        public static void Parse(string str)
        {
            using var stringReader = new StringReader(str);
            var xmlTextReader = new XmlTextReader(stringReader)
            {
                WhitespaceHandling = WhitespaceHandling.Significant
            };
#if CREATE_ELEMENTS
            var elements = new Stack<Element>();
            var root = default(Element);
            var element = default(Element);
#endif
            while (xmlTextReader.Read())
            {
                switch (xmlTextReader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
#if CONSOLE_DEBUG
                            Console.WriteLine($"<Element> '{xmlTextReader.LocalName}");
#endif
#if CREATE_ELEMENTS
                            element = new Element()
                            {
                                ElementName = xmlTextReader.LocalName
                            };

                            if (elements.Count == 0)
                            {
                                root = element;
                            }

                            if (elements.Count > 0)
                            {
                                var parent = elements.Peek();
                                parent.AddChild(element);
                            }

                            elements.Push(element);
#endif
#if CONSOLE_DEBUG
                            Console.WriteLine($"    <Attributes>");
#endif
                            while (xmlTextReader.MoveToNextAttribute())
                            {
#if CONSOLE_DEBUG
                                Console.WriteLine($"    [\"{xmlTextReader.LocalName}\"] = \"{xmlTextReader.Value}\"");
#endif
#if CREATE_ELEMENTS
                                element?.AddAttribute(xmlTextReader.LocalName, xmlTextReader.Value);
#endif
                            }
                        }
                        break;
                    case XmlNodeType.EndElement:
                        {
#if CREATE_ELEMENTS
                            element = elements.Pop();
#endif
                        }
                        break;
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text:
                    case XmlNodeType.SignificantWhitespace:
                        {
#if CREATE_ELEMENTS
                            element = elements.Peek();
                            element.Content = xmlTextReader.Value;
#endif
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