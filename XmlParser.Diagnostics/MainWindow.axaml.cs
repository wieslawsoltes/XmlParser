using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace XmlParser.Diagnostics
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var vm = new MainWindowViewModel();

            AddHandler(DragDrop.DropEvent, vm.Drop);
            AddHandler(DragDrop.DragOverEvent, vm.DragOver);

            DataContext = vm;
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
