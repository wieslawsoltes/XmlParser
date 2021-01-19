using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace XmlParser.Diagnostics
{
    public class MainWindow : Window
    {
        public ObservableCollection<Item> Items { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Items = new ObservableCollection<Item>();

            DataContext = this;

            AddHandler(DragDrop.DropEvent, Drop);
            AddHandler(DragDrop.DragOverEvent, DragOver);

            void Drop(object sender, DragEventArgs e)
            {
                if (!e.Data.Contains(DataFormats.FileNames))
                {
                    return;
                }
                var paths = e.Data.GetFileNames();
                if (paths is null)
                {
                    return;
                }

                if (e.KeyModifiers == KeyModifiers.Alt)
                {
                    Items.Clear();
                }

                foreach (var path in paths)
                {
                    Console.WriteLine($"{path}");
                    var svg = File.ReadAllText(path);
                    var sw = Stopwatch.StartNew();

                    try
                    {
                        var factory = new XmlFactory();
                        XmlParser.Parse(svg.AsSpan(), factory);
                        var item = new Item()
                        {
                            Name = Path.GetFileName(path),
                            Path = path,
                            Svg = svg,
                            Root = factory.GetRootElement() as XmlElement
                        };
                        Items.Add(item);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }

                    sw.Stop();
                    Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");
                }
            }

            void DragOver(object sender, DragEventArgs e)
            {
                e.DragEffects &= (DragDropEffects.Copy | DragDropEffects.Link);

                if (!e.Data.Contains(DataFormats.FileNames))
                {
                    e.DragEffects = DragDropEffects.None;
                }
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
