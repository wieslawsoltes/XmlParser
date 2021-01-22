using System;
using BenchmarkDotNet.Attributes;
using XmlParser.Sample;

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
        public void XmlParser2_Parse()
        {
            XmlParser2.Parse(Samples.Empty.AsSpan());
        }

        [Benchmark]
        public void XmlTextReader_Parse()
        {
            XmlTextReaderParser.Parse(Samples.Empty);
        }
    }
}
