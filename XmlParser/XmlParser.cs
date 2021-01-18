#nullable enable
//#define CONSOLE_DEBUG
//#define CREATE_ELEMENTS
using System;
using System.Collections.Generic;

namespace XmlParser
{
    public static class XmlParser
    {
        public const char TagStart = '<';
        public const char TagEnd = '>';
        public const char TagEndSlash = '/';
        public const char AttributeSeparator = '=';
        public const char DelimiterApostrophe = '\'';
        public const char DelimiterQuotationMark = '\"';
        public readonly static char[] CommentStart = new[] {'<', '!', '-', '-'};
        public readonly static char[] CommentEnd = new[] {'-', '-', '>'};
        public readonly static char[] WhitespaceChars = new[] {' ', '\t', '\n', '\r'};
        public const string UriString = "http://www.w3.org/2000/svg";

        public static void Parse(ReadOnlySpan<char> span)
        {
#if CONSOLE_DEBUG
            int indent = 0;
            int indentSize = 4;
#endif
#if CREATE_ELEMENTS
            var elements = new Stack<Element>();
            var root = default(Element);
            var element = default(Element);
#endif
            var whitespaceChars = WhitespaceChars.AsSpan();

            while (true)
            {
                // Next Tag

                var start = span.IndexOf(TagStart);
                if (start < 0)
                {
                    break;
                }

                // Comment

                var isComment = span.Length >= start + 4 && span[start + 1] == CommentStart[1] && span[start + 2] == CommentStart[2] && span[start + 3] == CommentStart[3];
                if (isComment)
                {
                    var commentEnd = span.IndexOf(CommentEnd.AsSpan()) + 3;
#if CONSOLE_DEBUG
                    var commentStart = start + 4;
                    var comment = span.Slice(commentStart, commentEnd - commentStart - 3);
                    Console.WriteLine($"{new string(' ', indent)}<Comment>");
                    Console.WriteLine($"{new string(' ', indent)}{comment.ToString()}");
#endif
                    span = span.Slice(commentEnd);
                    continue;
                }
    
                span = span.Slice(start + 1);
                var splitIndex = span.IndexOfAny(whitespaceChars);
                if (splitIndex <= 0)
                {
                    break;
                }

                var endIndex = span.IndexOf(TagEnd);
                if (endIndex < 0)
                {
                    break;
                }
                bool isSelfEnd = (endIndex > 0 && span[endIndex - 1] == TagEndSlash);
                bool isEnd = span[0] == TagEndSlash;
                bool hasAttributes = span[splitIndex - 1] != TagEnd;
                var elementStart = isEnd && !isSelfEnd ? 1 : 0;
                var elementEnd = hasAttributes ? splitIndex - elementStart : splitIndex - elementStart - 1;
                var elementName = span.Slice(elementStart, elementEnd);

                if (!isEnd)
                {
#if CONSOLE_DEBUG
                    Console.WriteLine($"{new string(' ', indent)}<Element> '{elementName.ToString()}'");
                    indent += indentSize;
#endif
#if CREATE_ELEMENTS
                    element = new Element()
                    {
                        ElementName = elementName.ToString()
                    };
#endif
                    var nextIndex = span.IndexOf(TagStart);
                    if (nextIndex > 0)
                    {
                        // Content
                        var contentStart = endIndex + 1;
                        var contentLength = nextIndex - endIndex - 1;
                        var content = span.Slice(contentStart, contentLength).Trim();
#if CONSOLE_DEBUG
                        if (content.Length > 0)
                        {
                            Console.WriteLine($"{new string(' ', indent)}<Content>");
                            Console.WriteLine($"{new string(' ', indent)}'{content.ToString()}'");
                        }
#endif
#if CREATE_ELEMENTS
                        if (content.Length > 0)
                        {
                            element.Content = content.ToString();
                        }
#endif
                    }
#if CREATE_ELEMENTS
                    if (elements.Count == 0)
                    {
                        root = element;
                    }

                    if (elements.Count > 0)
                    {
                        var parent = elements.Peek();
                        parent.AddChild(element);
                    }

                    elements.Push(element);
#endif
                }
#if CONSOLE_DEBUG
                if (isEnd || isSelfEnd)
                {
                    indent -= indentSize;
                }
#endif
#if CREATE_ELEMENTS
                if (isEnd || isSelfEnd)
                {
                    element = elements.Pop();
                }
#endif
                if (hasAttributes)
                {
                    var attributes = span.Slice(splitIndex, endIndex - splitIndex);
#if CONSOLE_DEBUG
                    Console.WriteLine($"{new string(' ', indent)}<Attributes>");
#endif
                    while (true)
                    {
                        var attributeSplitIndex = attributes.IndexOf(AttributeSeparator);
                        if (attributeSplitIndex < 0)
                        {
                            // ERROR
                            break;
                        }
                        
                        var attributeKey = attributes.Slice(0, attributeSplitIndex);

                        attributes = attributes.Slice(attributeSplitIndex + 1);
                        var attributeStartValueIndex1 = attributes.IndexOf(DelimiterQuotationMark);
                        var attributeStartValueIndex2 = attributes.IndexOf(DelimiterApostrophe);
                        if (attributeStartValueIndex1 < 0 && attributeStartValueIndex2 < 0)
                        {
                            // ERROR
                            break;
                        }
                        attributes = attributes.Slice(attributeStartValueIndex1 >= 0 ? attributeStartValueIndex1 + 1 : attributeStartValueIndex2 + 1);
                        var attributeEndValueIndex1 = attributes.IndexOf(DelimiterQuotationMark);
                        var attributeEndValueIndex2 = attributes.IndexOf(DelimiterApostrophe);
                        if (attributeEndValueIndex1 < 0 && attributeEndValueIndex2 < 0)
                        {
                            // ERROR
                            break;
                        }
                        var attributeValue = attributes.Slice(0, attributeEndValueIndex1 >= 0 ? attributeEndValueIndex1 : attributeEndValueIndex2);
#if CONSOLE_DEBUG
                        Console.WriteLine($"{new string(' ', indent)}[\"{attributeKey.Trim().ToString()}\"] = \"{attributeValue.ToString()}\"");
#endif
#if CREATE_ELEMENTS
                        element?.AddAttribute(attributeKey.Trim().ToString(), attributeValue.ToString());
#endif
                        attributes = attributes.Slice(attributeEndValueIndex1 >= 0 ? attributeEndValueIndex1 + 1 : attributeEndValueIndex2 + 1);
                    }

                    span = span.Slice(endIndex + 1);
                    if (span.Length == 0)
                    {
                        break;
                    }
                    continue;
                }

                //if (isEnd)
                else
                {
#if CONSOLE_DEBUG
                    if (isEnd)
                    {
                        Console.WriteLine($"{new string(' ', indent)}<EndElement> '{elementName.ToString()}'");
                    }
#endif
                    var tagEndIndex = span.IndexOf(TagEnd);
                    span = span.Slice(tagEndIndex + 1);
                    if (span.Length == 0)
                    {
                        break;
                    }
                }
            }
        }   
    }
}