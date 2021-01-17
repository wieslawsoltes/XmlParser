#nullable enable
//#define CREATE_ELEMENTS
using System;
using System.Diagnostics;

namespace XmlParser
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            XmlParser.Parse(Samples.struct_svg_03_f.AsSpan());
            sw.Stop();
            Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");
        }
    }
}