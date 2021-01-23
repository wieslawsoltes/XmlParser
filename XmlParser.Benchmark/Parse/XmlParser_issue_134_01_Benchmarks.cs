using System;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class XmlParser_issue_134_01_Benchmarks
    {
        private static readonly string _xml;

        static XmlParser_issue_134_01_Benchmarks()
        {
            _xml = Util.ToString(Util.Open("__issue-134-01.svg"));
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
