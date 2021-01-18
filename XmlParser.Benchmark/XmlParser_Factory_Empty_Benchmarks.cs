using System;
using BenchmarkDotNet.Attributes;
using XmlParser.Sample;

namespace XmlParser.Benchmark
{
    public class XmlParser_Factory_Empty_Benchmarks
    {
        [Benchmark(Baseline = true)]
        public void XmlParser_Parse_Factory()
        {
            var factory = new XmlFactory();
            XmlParser.Parse(Samples.Empty.AsSpan(), factory);
        }

        [Benchmark]
        public void XmlTextReader_Parse_Factory()
        {
            var factory = new XmlFactory();
            XmlTextReaderParser.Parse(Samples.Empty, factory);
        }
    }
}