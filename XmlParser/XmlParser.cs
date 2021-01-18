#nullable enable
using System;

namespace XmlParser
{
    public static class XmlParser
    {
        private const char LessThanSign = '<';
        private const char GreaterThanSign = '>';
        private const char Apostrophe = '\'';
        private const char QuotationMark = '\"';
        private const char Solidus = '/';
        private const char QuestionMark = '?';
        private const char ExclamationMark = '!';
        private const char EqualsSign = '=';
        private const char LeftSquareBracket = '[';
        private const char RightSquareBracket = ']';
        private static readonly char[] WhitespaceChars = new[] {' ', '\t', '\n', '\r'};
        private static readonly char[] CommentStart = new[] {'<', '!', '-', '-'};
        private static readonly char[] CommentEnd = new[] {'-', '-', '>'};
        private static readonly char[] DoctypeStart = new[] {'<', '!', 'D', 'O', 'C', 'T', 'Y', 'P', 'E'};
        private static readonly char[] DoctypeEnd = new[] {']', '>'};
        private static readonly char[] CdataStart = new[] {'<', '!', '[', 'C', 'D', 'A', 'T', 'A', '['};
        private static readonly char[] CdataEnd = new[] {']', ']', '>'};

        public static void Parse(ReadOnlySpan<char> span, IXmlFactory? factory = null)
        {
            var whitespaceChars = WhitespaceChars.AsSpan();

            while (true)
            {
                // Next Tag

                var startIndex = span.IndexOf(LessThanSign);
                if (startIndex < 0)
                {
                    break;
                }

                if (span.Length < 1)
                {
                    break;
                }

                // Unsupported (processing instruction, comment, cdata etc.)

                if (span.Length >= startIndex + 1 
                    && (span[startIndex + 1] == QuestionMark || span[startIndex + 1] == ExclamationMark))
                {
                    // Comment

                    var isComment = span.Length >= startIndex + 4 
                                    && span[startIndex + 1] == CommentStart[1] 
                                    && span[startIndex + 2] == CommentStart[2] 
                                    && span[startIndex + 3] == CommentStart[3];
                    if (isComment)
                    {
                        var commentEnd = span.IndexOf(CommentEnd.AsSpan()) + 3;
                        if (commentEnd < 0)
                        {
                            break;
                        }

                        var commentStart = startIndex + 4;
                        var comment = span.Slice(commentStart, commentEnd - commentStart - 3);
  
                        span = span.Slice(commentEnd);
                        if (span.Length <= 0)
                        {
                            break;
                        }
                        continue;
                    }

                    // Processing Instructions

                    if (span[startIndex + 1] == QuestionMark)
                    {
                        var endInstruction = span.IndexOf(GreaterThanSign);
                        if (endInstruction < 0)
                        {
                            break;
                        }

                        var contentStart = startIndex + 2;
                        var contentLength = endInstruction - 3 - startIndex;
                        var instruction = span.Slice(contentStart, contentLength);

                        span = span.Slice(endInstruction + 1);
                        if (span.Length <= 0)
                        {
                            break;
                        }
                        continue;
                    }

                    // DOCTYPE
                    
                    var isDoctype = span.Length >= startIndex + 8 
                                    && span[startIndex + 1] == DoctypeStart[1] 
                                    && span[startIndex + 2] == DoctypeStart[2] 
                                    && span[startIndex + 3] == DoctypeStart[3] 
                                    && span[startIndex + 4] == DoctypeStart[4] 
                                    && span[startIndex + 5] == DoctypeStart[5] 
                                    && span[startIndex + 6] == DoctypeStart[6] 
                                    && span[startIndex + 7] == DoctypeStart[7]
                                    && span[startIndex + 8] == DoctypeStart[8];
                    if (isDoctype)
                    {
                        var doctypeEnd = span.IndexOf(DoctypeEnd.AsSpan());
                        if (doctypeEnd > 0)
                        {
                            span = span.Slice(doctypeEnd + DoctypeEnd.Length);
                        }
                        else
                        {
                            var doctypeEndGreaterThanSign = span.IndexOf(GreaterThanSign);
                            span = span.Slice(doctypeEndGreaterThanSign + 1);
                        }

                        if (span.Length <= 0)
                        {
                            break;
                        }
                        continue;
                    }

                    // CDATA
                    
                    var isCdata = span.Length >= startIndex + 8
                                  && span[startIndex + 1] == CdataStart[1] 
                                  && span[startIndex + 2] == CdataStart[2] 
                                  && span[startIndex + 3] == CdataStart[3] 
                                  && span[startIndex + 4] == CdataStart[4] 
                                  && span[startIndex + 5] == CdataStart[5] 
                                  && span[startIndex + 6] == CdataStart[6] 
                                  && span[startIndex + 7] == CdataStart[7]
                                  && span[startIndex + 8] == CdataStart[8];
                    if (isCdata)
                    {
                        var cdataEnd = span.IndexOf(CdataEnd.AsSpan());
                        if (cdataEnd > 0)
                        {
                            span = span.Slice(cdataEnd + CdataEnd.Length);
                        }
                        else
                        {
                            break;
                        }

                        if (span.Length <= 0)
                        {
                            break;
                        }
                        continue;
                    }

                    throw new Exception("Not supported Xml content.");
                }

                // Element

                span = span.Slice(startIndex + 1);

                var nextWhiteSpaceIndex = span.IndexOfAny(whitespaceChars);
                if (nextWhiteSpaceIndex <= 0)
                {
                    break;
                }

                var endIndex = span.IndexOf(GreaterThanSign);
                if (endIndex < 0)
                {
                    break;
                }

                var isSelfEnd = (endIndex > 0 && span[endIndex - 1] == Solidus);
                var isEnd = span[0] == Solidus;
                var hasAttributes = nextWhiteSpaceIndex < endIndex;
                var endOffset = isEnd && !isSelfEnd ? 1 : 0;
                var elementEnd = hasAttributes ? 
                                 nextWhiteSpaceIndex - endOffset 
                                 : (nextWhiteSpaceIndex > endIndex ? endIndex - endOffset : nextWhiteSpaceIndex - endOffset - 1);

                var elementName = span.Slice(endOffset, elementEnd);

                if (!isEnd)
                {
                    factory?.PushElement(elementName);

                    // Content

                    var nextIndex = span.IndexOf(LessThanSign);
                    if (nextIndex > 0)
                    {
                        var contentStart = endIndex + 1;
                        var contentLength = nextIndex - endIndex - 1;
                        var content = span.Slice(contentStart, contentLength).Trim();
                        if (content.Length > 0)
                        {
                            factory?.AddElementContent(content);
                        }
                    }
                }

                if (isEnd || isSelfEnd)
                {
                    factory?.PopElement();
                }

                // Attributes

                if (hasAttributes)
                {
                    var attributesStart = nextWhiteSpaceIndex;
                    var attributesLength = endIndex - nextWhiteSpaceIndex;
                    var attributes = span.Slice(attributesStart, attributesLength);

                    while (true)
                    {
                        var attributeSplitIndex = attributes.IndexOf(EqualsSign);
                        if (attributeSplitIndex < 0)
                        {
                            break;
                        }

                        var attributeKey = attributes.Slice(0, attributeSplitIndex);

                        attributes = attributes.Slice(attributeSplitIndex + 1);
                        var attributeStartValueIndex1 = attributes.IndexOf(QuotationMark);
                        var attributeStartValueIndex2 = attributes.IndexOf(Apostrophe);
                        if (attributeStartValueIndex1 < 0 && attributeStartValueIndex2 < 0)
                        {
                            break;
                        }
                        attributes = attributes.Slice(attributeStartValueIndex1 >= 0 ? attributeStartValueIndex1 + 1 : attributeStartValueIndex2 + 1);
                        var attributeEndValueIndex1 = attributes.IndexOf(QuotationMark);
                        var attributeEndValueIndex2 = attributes.IndexOf(Apostrophe);
                        if (attributeEndValueIndex1 < 0 && attributeEndValueIndex2 < 0)
                        {
                            break;
                        }
                        var attributeValue = attributes.Slice(0, attributeEndValueIndex1 >= 0 ? attributeEndValueIndex1 : attributeEndValueIndex2);

                        // Filter Attributes
#if false
                        if (elementName.SequenceEqual("path".AsSpan()))
                        {
                            if (attributeKey.Trim().SequenceEqual("d".AsSpan()))
                            {
                                Console.WriteLine($"d={attributeValue.ToString()}");
                            }
                        }
#endif
                        factory?.AddElementAttribute(attributeKey.Trim().ToString(), attributeValue.ToString());

                        attributes = attributes.Slice(attributeEndValueIndex1 >= 0 ? attributeEndValueIndex1 + 1 : attributeEndValueIndex2 + 1);
                    }

                    span = span.Slice(endIndex + 1);
                    if (span.Length == 0)
                    {
                        break;
                    }
                }
                else
                {
                    var nextTagEndIndex = span.IndexOf(GreaterThanSign);
                    span = span.Slice(nextTagEndIndex + 1);
                    if (span.Length == 0)
                    {
                        break;
                    }
                }
            }
        }   
    }
}