using System;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class XmlParser_Empty_Benchmarks
    {
        [Benchmark(Baseline = true)]
        public void XmlParser_Parse()
        {
            XmlParser.Parse(Samples.Empty.AsSpan());
        }

        [Benchmark]
        public void XmlTextReader_Parse()
        {
            XmlTextReaderParser.Parse(Samples.Empty);
        }
    }
}