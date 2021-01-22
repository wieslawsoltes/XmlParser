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
            var skipComment = false;
            var lastWhitespace = -1;

            for (int position = 0; position < span.Length; position++)
            {
                if (span[position] == '\r')
                {
                    lastWhitespace = position;
                    column = 1;
                    continue;
                }
                else if (span[position] == '\n')
                {
                    lastWhitespace = position;
                    column = 1;
                    line++;
                    continue;
                }

                // Tag Start
                if (span[position] == '<')
                {
                    var start = position;
                    var startLine = line;
                    var startColumn = column;
                    var end = -1;
                    var slash = -1;
                    var skipValue = false;
                    var skipValueStart = -1;
                    var skipValueEnd = -1;

                    if (position + 1 >= span.Length)
                    {
                        break;
                    }
                    var slice = span.Slice(position + 1);

                    for (int slicePosition = 0; slicePosition < slice.Length; slicePosition++)
                    {
                        position++;

                        if (slice[slicePosition] == '\r')
                        {
                            lastWhitespace = position;
                            column = 1;
                        }
                        else if (slice[slicePosition] == '\n')
                        {
                            lastWhitespace = position;
                            column = 1;
                            line++;
                        }
                        else
                        {
                            column++;
                        }

                        // Comment End
                        if (skipComment)
                        {
                            if (slice[slicePosition] == '-' && slice[slicePosition + 1] == '-' && slice[slicePosition + 2] == '>')
                            {
                                skipComment = false;
                                position += 3;
                                column += 3;
                                break;
                            }
                            continue;
                        }

                        // Comment Start
                        if (slice[slicePosition] == '!' && slice[slicePosition + 1] == '-' && slice[slicePosition + 2] == '-')
                        {
                            skipComment = true;
                            slicePosition += 3;
                            position += 3;
                            column += 3;
                            continue;
                        }

                        // Skip Value
                        if (skipValue && slice[slicePosition] != '\'' && slice[slicePosition] != '\"')
                        {
                            continue;
                        }
 
                        // Start Value
                        if (slice[slicePosition] == '\'' || slice[slicePosition] == '\"')
                        {
                            if (skipValue)
                            {
                                skipValueEnd = position;
                                skipValue = false;

                                var value = span.Slice(skipValueStart, skipValueEnd - skipValueStart + 1);
                                // Console.WriteLine($"{value.ToString()}");
                            }
                            else
                            {
                                skipValueStart = position;
                                skipValueEnd = -1;
                                skipValue = true;
                                continue;
                            }
                        }

                        if (slice[slicePosition] == '/')
                        {
                            slash = position;
                        }

                        // Whitespace
                        if (slice[slicePosition] == ' ' || slice[slicePosition] == '\t' || slice[slicePosition] == '\n' || slice[slicePosition] == '\r')
                        {
                            lastWhitespace = position;
                            if (end < 0)
                            {
                                end = position;
                            }
                            continue;
                        }

                        // Tag End
                        if (slice[slicePosition] == '>')
                        {
                            if (end < 0)
                            {
                                end = position;
                            }

                            if (slash == start + 1)
                            {
                                // </tag>
                                var e = span.Slice(start + 2, end - start - 2);
                                level--;
                                Console.WriteLine($"[1] {new string(' ', level * 2)}'</{e.ToString()}>' {startLine}:{startColumn}");
                            }
                            else if (slash == position - 1)
                            {
                                // <tag/>
                                var e = span.Slice(start + 1, end - start - 1);
                                Console.WriteLine($"[2] {new string(' ', level * 2)}'<{e.ToString()}/>' {startLine}:{startColumn}");
                            }
                            else
                            {
                                // <tag>
                                var e = span.Slice(start + 1, end - start - 1);
                                Console.WriteLine($"[3] {new string(' ', level * 2)}'<{e.ToString()}>' {startLine}:{startColumn}");
                                level++;
                            }

                            break;
                        }
                    }
                }

                column++;
            }
        }
    }
}
