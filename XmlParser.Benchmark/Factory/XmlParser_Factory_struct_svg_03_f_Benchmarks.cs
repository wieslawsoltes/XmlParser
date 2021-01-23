using System;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class XmlParser_Factory_struct_svg_03_f_Benchmarks
    {
        private static readonly string _xml;

        static XmlParser_Factory_struct_svg_03_f_Benchmarks()
        {
            _xml = Util.ToString(Util.Open("struct-svg-03-f.svg"));
        }

        [Benchmark(Baseline = true)]
        public void XmlParser_Parse_Factory()
        {
            var factory = new XmlFactory();
            XmlParser.Parse(_xml.AsSpan(), factory);
        }

        [Benchmark]
        public void XmlParser2_Parse_Factory()
        {
            var factory = new XmlFactory();
            XmlParser2.Parse(_xml.AsSpan(), factory);
        }

        [Benchmark]
        public void XmlTextReader_Parse_Factory()
        {
            var factory = new XmlFactory();
            XmlTextReaderParser.Parse(_xml, factory);
        }
    }
}
