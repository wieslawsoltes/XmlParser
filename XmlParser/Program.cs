#nullable enable
using System;
using System.Diagnostics;
using System.IO;
using XmlParser.Factory;

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

                var factory = new XmlFactory();
                XmlParser.Parse(svg.AsSpan(), factory);

                //XmlParser.Parse(svg.AsSpan());

                //XmlTextReaderParser.Parse(svg);
                
                sw.Stop();
                Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");
            }
            else
            {
                var sw = Stopwatch.StartNew();
                
                //var factory = new XmlFactory();
                //XmlParser.Parse(Samples.Empty.AsSpan(), factory);
                //var root = factory.GetRootElement();

                //XmlParser.Parse(Samples.Empty.AsSpan());
 
                //XmlTextReaderParser.Parse(Samples.Empty);

                var factory = new XmlFactory();
                XmlParser.Parse(Samples.struct_svg_03_f.AsSpan(), factory);
                var root = factory.GetRootElement();

                //XmlParser.Parse(Samples.struct_svg_03_f.AsSpan());

                //XmlTextReaderParser.Parse(Samples.struct_svg_03_f);

                Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");
            }
        }
    }
}