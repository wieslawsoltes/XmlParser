using System;
using BenchmarkDotNet.Attributes;
using XmlParser.Factory;

namespace XmlParser.Benchmark
{
    public class XmlParser_Factory_struct_svg_03_f_Benchmarks
    {
        [Benchmark(Baseline = true)]
        public void XmlParser_Parse_Factory()
        {
            var factory = new XmlFactory();
            XmlParser.Parse(Samples.struct_svg_03_f.AsSpan(), factory);
        }

        [Benchmark]
        public void XmlTextReader_Parse_Factory()
        {
            var factory = new XmlFactory();
            XmlTextReaderParser.Parse(Samples.struct_svg_03_f, factory);
        }
    }
}