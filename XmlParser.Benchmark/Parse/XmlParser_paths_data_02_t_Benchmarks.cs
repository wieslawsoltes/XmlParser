using System;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class XmlParser_paths_data_02_t_Benchmarks
    {
        private static readonly string _xml;

        static XmlParser_paths_data_02_t_Benchmarks()
        {
            _xml = Util.ToString(Util.Open("paths-data-02-t.svg"));
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
