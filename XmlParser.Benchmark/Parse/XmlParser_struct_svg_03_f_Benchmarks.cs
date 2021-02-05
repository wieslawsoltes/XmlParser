using System;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class XmlParser_struct_svg_03_f_Benchmarks
    {
        private static readonly string _xml;

        static XmlParser_struct_svg_03_f_Benchmarks()
        {
            _xml = Util.ToString(Util.Open("struct-svg-03-f.svg"));
        }

        [Benchmark(Baseline = true)]
        public void XmlParser_Parse()
        {
            XmlParser.Parse(_xml.AsSpan());
        }

        [Benchmark]
        public void XmlTextReader_Parse()
        {
            XmlTextReaderParser.Parse(_xml);
        }
    }
}
