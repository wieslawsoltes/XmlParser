using System;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class XmlParser_NavBar_Benchmarks
    {
        private static readonly string _xml;

        static XmlParser_NavBar_Benchmarks()
        {
            _xml = Util.ToString(Util.Open("NavBar.axaml"));
        }

        [Benchmark(Baseline = true)]
        public void XmlParser_Parse()
        {
            XmlParser.Parse(_xml.AsSpan());
        }

        [Benchmark]
        public void XmlParser2_Parse()
        {
            XmlParser2.Parse(_xml.AsSpan());
        }

        [Benchmark]
        public void XmlTextReader_Parse()
        {
            XmlTextReaderParser.Parse(_xml);
        }
    }
}
