using System;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class XmlParser_Factory_issue_134_01_Benchmarks
    {
        private static readonly string _xml;

        static XmlParser_Factory_issue_134_01_Benchmarks()
        {
            _xml = Util.ToString(Util.Open("__issue-134-01.svg"));
        }

        [Benchmark(Baseline = true)]
        public void XmlParser_Parse_Factory()
        {
            var factory = new XmlFactory();
            XmlParser.Parse(_xml.AsSpan(), factory);
        }

        [Benchmark]
        public void XmlTextReader_Parse_Factory()
        {
            var factory = new XmlFactory();
            XmlTextReaderParser.Parse(_xml, factory);
        }
    }
}
