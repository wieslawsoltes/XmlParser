#nullable enable
using System;

namespace XmlParser.Factory
{
    public interface IXmlFactory
    {
        void PushElement(ReadOnlySpan<char> elementName);
        void PushElement(string elementName);
        void PopElement();
        void AddElementContent(ReadOnlySpan<char> content);
        void AddElementContent(string content);
        void AddElementAttribute(ReadOnlySpan<char> key, ReadOnlySpan<char> value);
        void AddElementAttribute(string key, string value);
        object? GetRootElement();
    }
}