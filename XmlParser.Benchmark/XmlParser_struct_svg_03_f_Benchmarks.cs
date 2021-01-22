using System;
using BenchmarkDotNet.Attributes;
using XmlParser.Sample;

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
        public void XmlParser2_Parse()
        {
            XmlParser2.Parse(Samples.struct_svg_03_f.AsSpan());
        }

        [Benchmark]
        public void XmlTextReader_Parse()
        {
            XmlTextReaderParser.Parse(Samples.struct_svg_03_f);
        }
    }
}
