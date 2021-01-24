#nullable enable
//#define DEBUG_ELEMENT_NAME
//#define DEBUG_CONTENT
//#define DEBUG_VALUE
//#define DEBUG_ATTRIBUTE
using System;

namespace XmlParser
{
    public static class XmlParser2
    {
        public static void Parse(ReadOnlySpan<char> span, IXmlFactory? factory = null)
        {
            var length = span.Length;
            var position = 0;
            var previousEnd = -1;
#if DEBUG_ELEMENT_NAME
            var level = 0;
#endif

            for (; position < length; position++)
            {  
                switch (span[position])
                {
                    // Whitespace
                    case ' ':
                    {
                        continue;
                    }
                    // Whitespace
                    case '\t':
                    {
                        continue;
                    }
                    // Whitespace
                    case '\r':
                    {
                        continue;
                    }
                    // Whitespace
                    case '\n':
                    {
                        continue;
                    }
                    // Tag Start
                    case '<':
                    {
                        if (position + 1 >= length)
                        {
                           break;
                        }

                        var start = position;
                        var end = -1;
                        var slash = -1;
                        var lastWhitespace = -1;

                        for (position += 1; position < length; position++)
                        {
                            // Processing Instruction: <? ... ?>
                            if (span[position] == '?')
                            {
                                for (position += 1; position < length; position++)
                                {
                                    if (span[position] == '?' && span[position + 1] == '>')
                                    {
                                        position += 2;
                                        previousEnd = position;
                                        break;
                                    }
                                }
                                break;
                            }

                            // Comment: <!-- ... -->
                            if (length >= position + 2
                                && span[position] == '!'
                                && span[position + 1] == '-' 
                                && span[position + 2] == '-')
                            {
                                position += 2;

                                for (position += 1; position < length; position++)
                                {
                                    if (span[position] == '-' && span[position + 1] == '-' && span[position + 2] == '>')
                                    {
                                        position += 3;
                                        previousEnd = position;
                                        break;
                                    }
                                }

                                break;
                            }

                            // CDATA: <![CDATA[ ... ]]>
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
                                position += 7;

                                for (position += 1; position < length; position++)
                                {
                                    if (span[position] == ']' && span[position + 1] == ']' && span[position + 2] == '>')
                                    {
                                        position += 3;
                                        previousEnd = position;
                                        break;
                                    }
                                }

                                break;
                            }

                            // DOCTYPE: <!DOCTYPE ... >
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
                                position += 7;

                                for (position += 1; position < length; position++)
                                {
                                    if (span[position] == '>')
                                    {
                                        position += 1;
                                        previousEnd = position;
                                        break;
                                    }
                                }

                                break;
                            }            

                            // Value: '...' or "..."
                            if (span[position] == '\"' || span[position] == '\'')
                            {
                                var skipValueEndChar = span[position];
                                var skipValueStart = position;

                                for (position += 1; position < length; position++)
                                {
                                    if (span[position] == skipValueEndChar)
                                    {
                                        break;
                                    }
                                }

                                var value = span.Slice(skipValueStart + 1, position - skipValueStart - 1);
#if DEBUG_VALUE
                                Console.WriteLine($"'{value.ToString()}'");
#endif
                                // Attribute
                                if (lastWhitespace >= 0 && span[skipValueStart - 1] == '=')
                                {
                                    var key = span.Slice(lastWhitespace + 1, skipValueStart - lastWhitespace - 2);
#if DEBUG_ATTRIBUTE
                                    Console.WriteLine($"'{key.ToString()}'='{value.ToString()}'");
#endif
                                }

                                continue;
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
                                    if (end < 0)
                                    {
                                        end = position;
                                    }
                                    continue;
                                }
                                case '\r':
                                {
                                    lastWhitespace = position;
                                    if (end < 0)
                                    {
                                        end = position;
                                    }
                                    continue;
                                }
                            }

                            // Tag Slash
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
#if DEBUG_CONTENT
                                    var trimmed = content.Trim();
                                    if (trimmed.Length > 0)
                                    {
                                        Console.WriteLine($"'{content.ToString()}'");
                                    }
#endif
                                }

                                // </tag>
                                var e = span.Slice(start + 2, end - start - 2);
                                // var e = span.Slice(start, end - start + 1);
#if DEBUG_ELEMENT_NAME
                                level--;
                                Console.WriteLine($"[1] {new string(' ', level * 2)}'</{e.ToString()}>'");
#endif
                                previousEnd = position;
                                break;
                            }
                            else if (slash == position - 1)
                            {
                                // <tag/>
                                var e = span.Slice(start + 1, end - start - 1);
                                // var e = span.Slice(start, end - start);
#if DEBUG_ELEMENT_NAME
                                Console.WriteLine($"[2] {new string(' ', level * 2)}'<{e.ToString()}/>'"); 
#endif
                                previousEnd = position;
                                break;
                            }
                            else
                            {
                                // <tag>
                                var e = span.Slice(start + 1, end - start - 1);
                                // var e = span.Slice(start, end - start + 1);
#if DEBUG_ELEMENT_NAME
                                Console.WriteLine($"[3] {new string(' ', level * 2)}'<{e.ToString()}>'");
                                level++; 
#endif
                                previousEnd = position;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}
