#nullable enable
using System.Collections.Generic;

namespace XmlParser
{
    public class Element
    {
        public string? ElementName { get; set; }
        public List<Element>? Children { get; set; }
        public Dictionary<string, string>? Attributes { get; set; }
        public string? Content { get; set; }

        public void AddChild(Element child)
        {
            if (Children == null)
            {
                Children = new List<Element>();
            }
            Children.Add(child);
        }

        public void AddAttribute(string key, string value)
        {
            if (Attributes == null)
            {
                Attributes = new Dictionary<string, string>();
            }
            Attributes[key] = value;
        }

        public override string? ToString() => ElementName;
    }
}