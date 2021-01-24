using System;
using Xunit;

namespace XmlParser.UnitTests
{
    public class XmlParser_tiger_UnitTest
    {
        private static readonly string _xml;

        static XmlParser_tiger_UnitTest()
        {
            _xml = Util.ToString(Util.Open("__tiger.svg"));
        }

        [Fact]
        public void XmlParser_Parse()
        {
            for (int i = 0; i < 1_000; i++)
            {
                XmlParser.Parse(_xml.AsSpan());
            }
        }

        [Fact]
        public void XmlParser2_Parse()
        {
            for (int i = 0; i < 1_000; i++)
            {
                XmlParser2.Parse(_xml.AsSpan());
            }
        }

        [Fact]
        public void XmlTextReader_Parse()
        {
            for (int i = 0; i < 1_000; i++)
            {
                XmlTextReaderParser.Parse(_xml);
            }
        }
    }
}
