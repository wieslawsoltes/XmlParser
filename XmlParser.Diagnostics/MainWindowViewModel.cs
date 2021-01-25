using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Avalonia.Input;

namespace XmlParser.Diagnostics
{
    public class MainWindowViewModel
    {
        public ObservableCollection<Item> Items { get; set; }

        public MainWindowViewModel()
        {
            Items = new ObservableCollection<Item>();
        }

        public void Drop(object sender, DragEventArgs e)
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

            Load(paths);
        }

        public void DragOver(object sender, DragEventArgs e)
        {
            e.DragEffects &= (DragDropEffects.Copy | DragDropEffects.Link);

            if (!e.Data.Contains(DataFormats.FileNames))
            {
                e.DragEffects = DragDropEffects.None;
            }
        }

        public void Load(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                Console.WriteLine($"{path}");
                var svg = File.ReadAllText(path);
                var sw = Stopwatch.StartNew();

                try
                {
                    var factory = new XmlFactory();
                    //XmlParser.Parse(svg.AsSpan(), factory);
                    XmlParser2.Parse(svg.AsSpan(), factory);
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
    }
}
