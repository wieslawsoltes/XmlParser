﻿#nullable enable
//#define DEBUG_ELEMENT_NAME
//#define DEBUG_CONTENT
//#define DEBUG_VALUE
//#define DEBUG_ATTRIBUTE
using System;

namespace XmlParser
{
    public static class XmlParser
    {
        public static void Parse(ReadOnlySpan<char> s, IXmlFactory? factory = null)
        {
#if DEBUG_ELEMENT_NAME
            var level = 0;
#endif
            var l = s.Length;
            var i = 0;
            var previousEnd = -1;

            for (; i < l; i++)
            {  
                switch (s[i])
                {
                    // Whitespace
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                    {
                        continue;
                    }
                    // Tag Start
                    case '<':
                    {
                        if (i + 1 >= l)
                        {
                           break;
                        }

                        var start = i;
                        var end = -1;
                        var slash = -1;
                        var lastWhitespace = -1;
                        for (i += 1; i < l; i++)
                        {
                            // Processing Instruction: <? ... ?>
                            if (s[i] == '?')
                            {
                                for (i += 1; i < l; i++)
                                {
                                    if (s[i] == '?' && s[i + 1] == '>')
                                    {
                                        i += 2;
                                        previousEnd = i;
                                        break;
                                    }
                                }
                                break;
                            }

                            // Comment: <!-- ... -->
                            if (l >= i + 2 && s[i] == '!' && s[i + 1] == '-' && s[i + 2] == '-')
                            {
                                i += 2;

                                for (i += 1; i < l; i++)
                                {
                                    if (s[i] == '-' && s[i + 1] == '-' && s[i + 2] == '>')
                                    {
                                        i += 3;
                                        previousEnd = i;
                                        break;
                                    }
                                }

                                break;
                            }

                            // CDATA: <![CDATA[ ... ]]>
                            if (l >= i + 7 && s[i] == '!' && s[i + 1] == '[' && s[i + 2] == 'C' && s[i + 3] == 'D' && s[i + 4] == 'A' && s[i + 5] == 'T' && s[i + 6] == 'A' && s[i + 7] == '[')
                            {
                                i += 7;

                                for (i += 1; i < l; i++)
                                {
                                    if (s[i] == ']' && s[i + 1] == ']' && s[i + 2] == '>')
                                    {
                                        i += 3;
                                        previousEnd = i;
                                        break;
                                    }
                                }

                                break;
                            }

                            // DOCTYPE: <!DOCTYPE ... >
                            if (l >= i + 7 && s[i] == '!' && s[i + 1] == 'D' && s[i + 2] == 'O' && s[i + 3] == 'C' && s[i + 4] == 'T' && s[i + 5] == 'Y' && s[i + 6] == 'P' && s[i + 7] == 'E')
                            {
                                i += 7;

                                for (i += 1; i < l; i++)
                                {
                                    if (s[i] == '>')
                                    {
                                        i += 1;
                                        previousEnd = i;
                                        break;
                                    }
                                }

                                break;
                            }            

                            if (i - 1 == start && s[i] != '/')
                            {
                                factory?.PushElement();
                            }

                            // Value: '...' or "..."
                            if (s[i] == '\"' || s[i] == '\'')
                            {
                                var skipValueEndChar = s[i];
                                var skipValueStart = i;

                                for (i += 1; i < l; i++)
                                {
                                    if (s[i] == skipValueEndChar)
                                    {
                                        break;
                                    }
                                }

                                var value = s.Slice(skipValueStart + 1, i - skipValueStart - 1);
#if DEBUG_VALUE
                                Console.WriteLine($"'{value.ToString()}'");
#endif
                                // Attribute
                                if (lastWhitespace >= 0 && s[skipValueStart - 1] == '=')
                                {
                                    var key = s.Slice(lastWhitespace + 1, skipValueStart - lastWhitespace - 2);
                                    factory?.AddElementAttribute(key, value);
#if DEBUG_ATTRIBUTE
                                    Console.WriteLine($"'{key.ToString()}'='{value.ToString()}'");
#endif
                                }

                                continue;
                            }

                            switch (s[i])
                            {
                                // Whitespace
                                case ' ':
                                case '\t':
                                case '\n':
                                case '\r':
                                {
                                    lastWhitespace = i;
                                    if (end < 0)
                                    {
                                        end = i;
                                    }
                                    continue;
                                }
                                // Tag Slash
                                case '/':
                                {
                                    slash = i;
                                    continue;
                                }
                                // Tag End
                                case '>':
                                {
                                    break;
                                }
                                // Skip
                                default:
                                {
                                    continue;
                                }
                            }

                            if (end < 0)
                            {
                                end = i;
                            }

                            // Tag Name
                            if (slash == start + 1)
                            {
                                // </tag>
                                var e = s.Slice(start + 2, end - start - 2);
                                factory?.PopElement();
                                factory?.SetElementName(e);
#if DEBUG_ELEMENT_NAME
                                level--;
                                Console.WriteLine($"[1] {new string(' ', level * 2)}'</{e.ToString()}>'");
#endif
                                if (previousEnd >= 0)
                                {
                                    var content = s.Slice(previousEnd + 1, start - previousEnd - 1);
                                    var trimmed = content.Trim();
                                    if (trimmed.Length > 0)
                                    {
                                        factory?.AddElementContent(trimmed);
#if DEBUG_CONTENT
                                        Console.WriteLine($"'{content.ToString()}'");
#endif
                                    }

                                }
                                previousEnd = i;
                                break;
                            }
                            else if (slash == i - 1)
                            {
                                // <tag/>
                                var e = s.Slice(start + 1, end - start - 1);
                                factory?.SetElementName(e);
                                factory?.PopElement();
#if DEBUG_ELEMENT_NAME
                                Console.WriteLine($"[2] {new string(' ', level * 2)}'<{e.ToString()}/>'"); 
#endif
                                previousEnd = i;
                                break;
                            }
                            else
                            {
                                // <tag>
                                var e = s.Slice(start + 1, end - start - 1);
                                factory?.SetElementName(e);
#if DEBUG_ELEMENT_NAME
                                Console.WriteLine($"[3] {new string(' ', level * 2)}'<{e.ToString()}>'");
                                level++;
#endif
                                previousEnd = i;
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
