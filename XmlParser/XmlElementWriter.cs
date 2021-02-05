#nullable enable
using System.Text;

namespace XmlParser
{
    public static class XmlElementWriter
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
}
