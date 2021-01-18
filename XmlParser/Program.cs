#nullable enable
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
            //XmlTextReaderParser.Parse(Samples.struct_svg_03_f);
            //XmlParser.Parse(Samples.Empty.AsSpan());
            //XmlTextReaderParser.Parse(Samples.Empty);
            sw.Stop();
            Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");
        }
    }
}