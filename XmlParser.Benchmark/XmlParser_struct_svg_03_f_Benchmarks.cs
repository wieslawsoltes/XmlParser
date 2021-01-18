using System;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class XmlParser_struct_svg_03_f_Benchmarks
    {
        [Benchmark(Baseline = true)]
        public void XmlParser_Parse()
        {
            XmlParser.Parse(Samples.struct_svg_03_f.AsSpan());
        }

        [Benchmark]
        public void XmlTextReader_Parse()
        {
            XmlTextReaderParser.Parse(Samples.struct_svg_03_f);
        }
    }
}