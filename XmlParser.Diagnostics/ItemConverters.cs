using Avalonia;
using Avalonia.Data.Converters;

namespace XmlParser.Diagnostics
{
    public static class ItemConverters
    {
        public static IValueConverter AttributesConverter = new FuncValueConverter<object, object>(x =>
        {
            return x switch
            {
                Item item => item.Root.Attributes,
                XmlElement element => element.Attributes,
                _ => AvaloniaProperty.UnsetValue
            };
        });

        public static IValueConverter ContentConverter = new FuncValueConverter<object, object>(x =>
        {
            return x switch
            {
                Item item => item.Root.Content,
                XmlElement element => element.Content,
                _ => AvaloniaProperty.UnsetValue
            };
        });
    }
}

