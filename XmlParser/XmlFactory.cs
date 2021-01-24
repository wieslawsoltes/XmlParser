#nullable enable
using System;
using System.Collections.Generic;

namespace XmlParser
{
    public class XmlFactory : IXmlFactory
    {
        private readonly Stack<XmlElement> _elements;
        private XmlElement? _root;
        private XmlElement? _element;

        public XmlFactory()
        {
            _elements = new Stack<XmlElement>();
            _root = default;
            _element = default;
        }

        public void PushElement()
        {
            PushElement(default(string));
        }

        public void PushElement(ReadOnlySpan<char> elementName)
        {
            PushElement(elementName.ToString());
        }

        public void PushElement(string? elementName)
        {
            _element = new XmlElement()
            {
                ElementName = elementName
            };

            if (_elements.Count == 0)
            {
                _root = _element;
            }
            else
            {
                var parent = _elements.Peek();
                parent.AddChild(_element);
            }

            _elements.Push(_element);
        }

        public void PopElement()
        {
            _element = _elements.Pop();
        }

        public void SetElementName(ReadOnlySpan<char> elementName)
        {
            if (_element != null)
            {
                _element.ElementName = elementName.ToString();
            }
        }

        public void AddElementContent(ReadOnlySpan<char> content)
        {
            AddElementContent(content.ToString());
        }

        public void AddElementContent(string content)
        {
            if (_element is { })
            {
                _element.Content = content;
            }
        }

        public void AddElementAttribute(ReadOnlySpan<char> key, ReadOnlySpan<char> value)
        {
            AddElementAttribute(key.ToString(), value.ToString());
        }

        public void AddElementAttribute(string key, string value)
        {
            _element?.AddAttribute(key, value);
        }

        public object? GetRootElement()
        {
            return _root;
        }
    }
}
