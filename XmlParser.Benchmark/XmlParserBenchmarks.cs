using System;
using System.IO;
using System.Xml;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class SvgDocumentBenchmarks
    {
        [Benchmark(Baseline = true)]
        public void XmlParser_Parse()
        {
            XmlParser.Parse(Samples.struct_svg_03_f.AsSpan());
        }

        [Benchmark]
        public void XmlTextReader_Read()
        {
            using var stringReader = new StringReader(Samples.struct_svg_03_f);
            var xmlTextReader = new XmlTextReader(stringReader)
            {
                WhitespaceHandling = WhitespaceHandling.Significant
            };
            while (xmlTextReader.Read())
            {
                switch (xmlTextReader.NodeType)
                {
                    case XmlNodeType.Element:
                        // TODO: 
                        // string elementName = xmlTextReader.LocalName;
                        break;
                    case XmlNodeType.EndElement:
                        // TODO:
                        while (xmlTextReader.MoveToNextAttribute())
                        {
                            // var localName = xmlTextReader.LocalName;
                            // var localValue = xmlTextReader.Value;
                        }
                        break;
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text:
                    case XmlNodeType.SignificantWhitespace:
                        // xmlTextReader.Value
                        break;
                    case XmlNodeType.EntityReference:
                        // xmlTextReader.Value
                        break;
                }
            }
        }
    }
}