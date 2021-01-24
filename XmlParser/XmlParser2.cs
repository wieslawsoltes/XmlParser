#nullable enable
using System;

namespace XmlParser
{
    public static class XmlParser2
    {
        public static void Parse(ReadOnlySpan<char> span, IXmlFactory? factory = null)
        {
            var level = 0;
            var line = 1;
            var column = 1;
            var position = 0;
            var previousEnd = -1;
            var length = span.Length;

            for (; position < length; position++)
            {
                switch (span[position])
                {
                    // Whitespace
                    case ' ':
                    {
                        column++;
                        break;
                    }
                    // Whitespace
                    case '\t':
                    {
                        column++;
                        break;
                    }
                    // Whitespace
                    case '\r':
                    {
                        column = 1;
                        break;
                    }
                    // Whitespace
                    case '\n':
                    {
                        column = 1;
                        line++;
                        break;
                    }
                    // Tag Start
                    case '<':
                    {
                        var start = position;
                        var startLine = line;
                        var startColumn = column;
                        var end = -1;
                        var slash = -1;
                        var skipValue = false;
                        var skipValueStart = -1;
                        var lastWhitespace = -1;

                        if (position + 1 >= length)
                        {
                           break;
                        }

                        var skipProcessingInstruction = false;
                        var skipComment = false;
                        var skipCdata = false;
                        var skipDoctype = false;

                        for (position += 1; position < length; position++)
                        {
                            // Processing Instruction End
                            if (skipProcessingInstruction)
                            {
                                if (span[position] == '?' && span[position + 1] == '>')
                                {
                                    position += 2;
                                    column += 2;
                                    previousEnd = position;
                                    break;
                                }
                                continue;
                            }

                            // Processing Instruction Start
                            if (span[position] == '?')
                            {
                                skipProcessingInstruction = true;
                                position += 1;
                                column += 1;
                                continue;
                            }

                            // Comment End
                            if (skipComment)
                            {
                                if (span[position] == '-' && span[position + 1] == '-' && span[position + 2] == '>')
                                {
                                    position += 3;
                                    column += 3;
                                    previousEnd = position;
                                    break;
                                }
                                continue;
                            }

                            // Comment Start
                            if (length >= position + 2
                                && span[position] == '!'
                                && span[position + 1] == '-' 
                                && span[position + 2] == '-')
                            {
                                skipComment = true;
                                position += 3;
                                column += 3;
                                continue;
                            }

                            // CDATA End
                            if (skipCdata)
                            {
                                if (span[position] == ']' && span[position + 1] == ']' && span[position + 2] == '>')
                                {
                                    position += 3;
                                    column += 3;
                                    previousEnd = position;
                                    break;
                                }
                                continue;
                            }

                            // CDATA Start
                            if (length >= position + 7
                                && span[position] == '!' 
                                && span[position + 1] == '[' 
                                && span[position + 2] == 'C' 
                                && span[position + 3] == 'D' 
                                && span[position + 4] == 'A' 
                                && span[position + 5] == 'T' 
                                && span[position + 6] == 'A' 
                                && span[position + 7] == '[')
                            {
                                skipCdata = true;
                                position += 8;
                                column += 8;
                                continue;
                            }

                            // DOCTYPE End
                            if (skipDoctype)
                            {
                                if (span[position] == ']' && span[position + 1] == '>')
                                {
                                    position += 2;
                                    column += 2;
                                    previousEnd = position;
                                    break;
                                }
                                continue;
                            }

                            // DOCTYPE Start
                            if (length >= position + 7 
                                && span[position] == '!'
                                && span[position + 1] == 'D' 
                                && span[position + 2] == 'O' 
                                && span[position + 3] == 'C' 
                                && span[position + 4] == 'T' 
                                && span[position + 5] == 'Y' 
                                && span[position + 6] == 'P' 
                                && span[position + 7] == 'E')
                            {
                                skipDoctype = true;
                                position += 8;
                                column += 8;
                                continue;
                            }            

                            // Skip Value
                            if (skipValue && span[position] != '\'' && span[position] != '\"')
                            {
                                continue;
                            }

                            // Start Value
                            if (span[position] == '\'' || span[position] == '\"')
                            {
                                if (skipValue)
                                {
                                    skipValue = false;

                                    var value = span.Slice(skipValueStart + 1, position - skipValueStart - 1);
                                    //Console.WriteLine($"'{value.ToString()}'");

                                    // Attribute
                                    if (lastWhitespace >= 0 && span[skipValueStart - 1] == '=')
                                    {
                                        var key = span.Slice(lastWhitespace + 1, skipValueStart - lastWhitespace - 2);
                                        //Console.WriteLine($"'{key.ToString()}'='{value.ToString()}'");
                                    }
                                }
                                else
                                {
                                    skipValueStart = position;
                                    skipValue = true;
                                    continue;
                                }
                            }

                            // Whitespace
                            switch (span[position])
                            {
                                case ' ':
                                {
                                    lastWhitespace = position;
                                    if (end < 0)
                                    {
                                        end = position;
                                    }
                                    continue;
                                }
                                case '\t':
                                {
                                    lastWhitespace = position;
                                    if (end < 0)
                                    {
                                        end = position;
                                    }
                                    continue;
                                }
                                case '\n':
                                {
                                    lastWhitespace = position;
                                    column = 1;
                                    line++;
                                    if (end < 0)
                                    {
                                        end = position;
                                    }
                                    continue;
                                }
                                case '\r':
                                {
                                    lastWhitespace = position;
                                    column = 1;
                                    if (end < 0)
                                    {
                                        end = position;
                                    }
                                    continue;
                                }
                                default:
                                {
                                    column++;
                                    break;
                                }
                            }

                            if (span[position] == '/')
                            {
                                slash = position;
                            }

                            // Tag End
                            if (span[position] != '>')
                            {
                                continue;
                            }

                            if (end < 0)
                            {
                                end = position;
                            }

                            // Tag Name
                            if (slash == start + 1)
                            {
                                if (previousEnd >= 0)
                                {
                                    var content = span.Slice(previousEnd + 1, start - previousEnd - 1);
                                    //var trimmed = content.Trim();
                                    //if (trimmed.Length > 0)
                                    //{
                                    //    Console.WriteLine($"'{content.ToString()}'");
                                    //}
                                }

                                // </tag>
                                var e = span.Slice(start + 2, end - start - 2);
                                //var e = span.Slice(start, end - start + 1);
                                level--;
                                //Console.WriteLine($"[1] {new string(' ', level * 2)}'</{e.ToString()}>' {startLine}:{startColumn}");
                                previousEnd = position;
                                break;
                            }
                            else if (slash == position - 1)
                            {
                                // <tag/>
                                var e = span.Slice(start + 1, end - start - 1);
                                //var e = span.Slice(start, end - start);
                                //Console.WriteLine($"[2] {new string(' ', level * 2)}'<{e.ToString()}/>' {startLine}:{startColumn}");
                                previousEnd = position;
                                break;
                            }
                            else
                            {
                                // <tag>
                                var e = span.Slice(start + 1, end - start - 1);
                                //var e = span.Slice(start, end - start + 1);
                                //Console.WriteLine($"[3] {new string(' ', level * 2)}'<{e.ToString()}>' {startLine}:{startColumn}");
                                level++;
                                previousEnd = position;
                                break;
                            }
                        }
                        break;
                    }
                    default:
                    {
                        column++;
                        break;
                    }
                }
            }
        }
    }
}
