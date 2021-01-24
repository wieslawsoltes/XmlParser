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
            XmlParser.Parse(_xml.AsSpan());
        }

        [Fact]
        public void XmlParser2_Parse()
        {
            XmlParser2.Parse(_xml.AsSpan());
        }

        [Fact]
        public void XmlTextReader_Parse()
        {
            XmlTextReaderParser.Parse(_xml);
        }
    }
}
