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
loop:
            for (; position < span.Length; position++)
            {
                switch (span[position])
                {
                    // Whitespace
                    case ' ':
                    {
                        column++;
                        continue;
                    }
                    // Whitespace
                    case '\t':
                    {
                        column++;
                        continue;
                    }
                    // Whitespace
                    case '\r':
                    {
                        column = 1;
                        continue;
                    }
                    // Whitespace
                    case '\n':
                    {
                        column = 1;
                        line++;
                        continue;
                    }
                    // Tag Start
                    case '<':
                        goto tag;
                    default:
                        column++;
                        continue;
                }
            }
            goto end;
tag:
            var start = position;
            var startLine = line;
            var startColumn = column;
            var end = -1;
            var slash = -1;
            var skipValue = false;
            var skipValueStart = -1;
            var lastWhitespace = -1;

            if (position + 1 >= span.Length)
            {
                goto loop;
            }

            var skipComment = false;

            for (position += 1; position < span.Length; position++)
            {
                switch (span[position])
                {
                    // Whitespace
                    case '\r':
                        lastWhitespace = position;
                        column = 1;
                        break;
                    // Whitespace
                    case '\n':
                        lastWhitespace = position;
                        column = 1;
                        line++;
                        break;
                    // Next Char
                    default:
                        column++;
                        break;
                }

                // Comment End
                if (skipComment)
                {
                    if (span[position] == '-' && span[position + 1] == '-' && span[position + 2] == '>')
                    {
                        position += 3;
                        column += 3;
                        break;
                    }
                    continue;
                }

                // Comment Start
                if (span[position] == '!' && span[position + 1] == '-' && span[position + 2] == '-')
                {
                    skipComment = true;
                    position += 3;
                    column += 3;
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

                if (span[position] == '/')
                {
                    slash = position;
                }

                // Whitespace
                if (span[position] == ' ' || span[position] == '\t' || span[position] == '\n' || span[position] == '\r')
                {
                    lastWhitespace = position;
                    if (end < 0)
                    {
                        end = position;
                    }
                    continue;
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

                if (slash == start + 1)
                {
                    // </tag>
                    var e = span.Slice(start + 2, end - start - 2);
                    level--;
                    //Console.WriteLine($"[1] {new string(' ', level * 2)}'</{e.ToString()}>' {startLine}:{startColumn}");
                }
                else if (slash == position - 1)
                {
                    // <tag/>
                    var e = span.Slice(start + 1, end - start - 1);
                    //Console.WriteLine($"[2] {new string(' ', level * 2)}'<{e.ToString()}/>' {startLine}:{startColumn}");
                }
                else
                {
                    // <tag>
                    var e = span.Slice(start + 1, end - start - 1);
                    //Console.WriteLine($"[3] {new string(' ', level * 2)}'<{e.ToString()}>' {startLine}:{startColumn}");
                    level++;
                }

                break;
            }
            goto loop;
end:
            return;
        }
    }
}
