#nullable enable
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace XmlParser.Sample
{
    internal static class Program
    {
        public static void Indent(StringBuilder sb, int level, string indent)
        {
            for (int i = 0; i < level; i++)
            {
                sb.Append(indent);
            }
        }

        public static void Print(XmlElement element, StringBuilder sb, int level, string indent)
        {
            Indent(sb, level, indent);
            sb.Append($"<{element.ElementName}");

            if (element.Attributes is not null && element.Attributes.Count > 0)
            {
                foreach (var attribute in element.Attributes)
                {
                    sb.Append($" {attribute.Key}=\"{attribute.Value}\"");
                }
            }

            if (element.Children is not null && element.Children.Count > 0 || !string.IsNullOrEmpty(element.Content))
            {
                sb.AppendLine(">");

                if (element.Children is not null && element.Children.Count > 0)
                {
                    foreach (var child in element.Children)
                    {
                        Print(child, sb, level + 1, indent);
                    }
                }

                if (!string.IsNullOrEmpty(element.Content))
                {
                    Indent(sb, level, indent);
                    sb.AppendLine(element.Content);
                }

                Indent(sb, level, indent);
                sb.AppendLine($"</{element.ElementName}>");
            }
            else
            {
                sb.AppendLine("/>");
            }
        }

        private static void Main(string[] args)
        {
            {
                //var path = @"c:\DOWNLOADS\GitHub-Forks\SVG\Tests\W3CTestSuite\svg\paths-data-02-t.svg";
                //var path = @"c:\DOWNLOADS\GitHub-Forks\WalletWasabi\WalletWasabi.Fluent\Views\NavBar\NavBar.axaml";
                var path = @"c:\DOWNLOADS\GitHub-Forks\SVG\Tests\W3CTestSuite\svg\__AJ_Digital_Camera.svg";
                var svg = File.ReadAllText(path);
                var factory = new XmlFactory();
                //XmlParser.Parse(svg.AsSpan(), factory);
                XmlParser2.Parse(svg.AsSpan(), factory);
                if (factory.GetRootElement() is XmlElement root)
                {
                    var sb = new StringBuilder();
                    Print(root, sb, 0, "  ");
                    Console.WriteLine(sb.ToString());
                }
                return;
            }
/*
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
*/
        }
    }
}
