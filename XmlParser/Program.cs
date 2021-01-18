#nullable enable
using System;
using System.Diagnostics;
using System.IO;

namespace XmlParser
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                var path = args[0];
                var svg = File.ReadAllText(path);
                var sw = Stopwatch.StartNew();
                XmlParser.Parse(svg.AsSpan());
                //XmlTextReaderParser.Parse(svg);
                sw.Stop();
                Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");
            }
            else
            {
                var sw = Stopwatch.StartNew();
                XmlParser.Parse(Samples.struct_svg_03_f.AsSpan());
                //XmlTextReaderParser.Parse(Samples.struct_svg_03_f);
                //XmlParser.Parse(Samples.Empty.AsSpan());
                //XmlTextReaderParser.Parse(Samples.Empty);
                Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");
            }
        }
    }
}