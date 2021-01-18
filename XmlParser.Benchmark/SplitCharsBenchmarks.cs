using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace XmlParser.Benchmark
{
    public class SplitCharsBenchmarks
    {
        public static ReadOnlySpan<char> SplitCharsReadOnlySpan => new[] {' ', '\t', '\n', '\r'};

        public static char[] SplitCharsArray = new[] {' ', '\t', '\n', '\r'};

        [Benchmark]
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