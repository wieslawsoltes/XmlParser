#nullable enable
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

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

                if (factory.GetRootElement() is XmlElement root)
                {
                    var sb = new StringBuilder();
                    XmlElementWriter.Write(root, sb, 0, "  ");
                    Console.WriteLine(sb.ToString());
                }
            }
            else if (args.Length == 2)
            {
                if (args[0] != "-d")
                {
                    return;
                }

                var paths = Directory.GetFiles(args[1], "*.svg");
                //var swt = Stopwatch.StartNew();

                foreach (var path in paths)
                {
                    var svg = File.ReadAllText(path);
                    Console.WriteLine($"{path}");
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
                    }

                    sw.Stop();
                    Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");
                }

                //swt.Stop();
                //Console.WriteLine($"{swt.Elapsed.TotalMilliseconds}ms");
            }
        }
    }
}
