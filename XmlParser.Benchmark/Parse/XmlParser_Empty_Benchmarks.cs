using System;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class XmlParser_Empty_Benchmarks
    {
        private static readonly string _xml;

        static XmlParser_Empty_Benchmarks()
        {
            _xml = Util.ToString(Util.Open("Empty.svg"));
        }

        [Benchmark]
        public void AsSpan_ForLoop()
        {
            var span = _xml.AsSpan();

            for (int position = 0; position < span.Length; position++)
            {
                var c = span[position];
            }
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
