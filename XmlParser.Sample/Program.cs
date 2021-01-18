#nullable enable
using System;
using System.Diagnostics;
using System.IO;

namespace XmlParser.Sample
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
            else if (args.Length == 2)
            {
                if (args[1] != "-d")
                {
                    return;
                }

                var paths = Directory.GetFiles(args[2], "*.svg");

                foreach (var path in paths)
                {
                    Console.WriteLine($"{path}");
                    var svg = File.ReadAllText(path);
                    var sw = Stopwatch.StartNew();

                    try
                    {
                        var factory = new XmlFactory();
                        XmlParser.Parse(svg.AsSpan(), factory);
                        //XmlParser.Parse(svg.AsSpan());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);
                        //return;
                    }

                    sw.Stop();
                    Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");
                }
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

                sw.Stop();
                Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");
            }
        }
    }
}