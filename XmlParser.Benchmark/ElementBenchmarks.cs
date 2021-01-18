using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class ElementBenchmarks
    {
        [Benchmark]
        public void Element_new()
        {
            var element = new Element();
        }

        [Benchmark]
        public void Elements_new()
        {
            var elements = new Stack<Element>();
        }
    }
}