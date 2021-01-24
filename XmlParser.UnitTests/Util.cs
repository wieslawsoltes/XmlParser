using System.IO;

namespace XmlParser.UnitTests
{
    internal static class Util
    {
        private const string Prefix = "XmlParser.UnitTests.Assets";

        public static Stream Open(string name)
        {
            return typeof(Util).Assembly.GetManifestResourceStream($"{Prefix}.{name}");
        }

        public static string ToString(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
