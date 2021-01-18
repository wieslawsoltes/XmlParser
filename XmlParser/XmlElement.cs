#nullable enable
using System.Collections.Generic;

namespace XmlParser
{
    public class XmlElement
    {
        public string? ElementName { get; set; }
        public List<XmlElement>? Children { get; set; }
        public Dictionary<string, string>? Attributes { get; set; }
        public string? Content { get; set; }

        public void AddChild(XmlElement child)
        {
            Children ??= new List<XmlElement>();
            Children.Add(child);
        }

        public void AddAttribute(string key, string value)
        {
            Attributes ??= new Dictionary<string, string>();
            Attributes[key] = value;
        }

        public override string? ToString() => ElementName;
    }
}