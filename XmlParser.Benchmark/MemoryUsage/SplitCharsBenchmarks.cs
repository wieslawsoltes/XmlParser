using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class SplitCharsBenchmarks
    {
        private static ReadOnlySpan<char> SplitCharsReadOnlySpan => new[] {' ', '\t', '\n', '\r'};

        private static readonly char[] SplitCharsArray = new[] {' ', '\t', '\n', '\r'};

        [Benchmark(Baseline = true)]
        public void SplitChars_Array_AsSpan()
        {
           var splitChars = SplitCharsArray.AsSpan();
        }

        [Benchmark]
        public void SplitChars_ReadOnlySpan()
        {
            var splitChars = SplitCharsReadOnlySpan;
        }
    }
}