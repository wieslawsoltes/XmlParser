#nullable enable
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace XmlParser.Sample
{
    internal static class XmlElementWriter
    {
        private static void Indent(StringBuilder sb, int level, string indent)
        {
            for (int i = 0; i < level; i++)
            {
                sb.Append(indent);
            }
        }

        public static void Write(XmlElement element, StringBuilder sb, int level, string indent)
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
                        Write(child, sb, level + 1, indent);
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
    }

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

                //XmlParser.Parse(svg.AsSpan(), factory);
                XmlParser2.Parse(svg.AsSpan(), factory);

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

                        //XmlParser.Parse(svg.AsSpan(), factory);
                        XmlParser2.Parse(svg.AsSpan(), factory);

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
