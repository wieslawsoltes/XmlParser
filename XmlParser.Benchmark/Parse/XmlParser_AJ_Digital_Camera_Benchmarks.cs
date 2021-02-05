using System;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class XmlParser_AJ_Digital_Camera_Benchmarks
    {
        private static readonly string _xml;

        static XmlParser_AJ_Digital_Camera_Benchmarks()
        {
            _xml = Util.ToString(Util.Open("__AJ_Digital_Camera.svg"));
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
