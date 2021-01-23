
dotnet run -c Release -f net5.0 -- -f '*'

dotnet run -c Release -f net5.0 -- -f '*ElementBenchmarks*'
dotnet run -c Release -f net5.0 -- -f '*SplitCharsBenchmarks*'

dotnet run -c Release -f net5.0 -- -f '*XmlParser_Factory_AJ_Digital_Camera_Benchmarks*'
dotnet run -c Release -f net5.0 -- -f '*XmlParser_Factory_Empty_Benchmarks*'
dotnet run -c Release -f net5.0 -- -f '*XmlParser_Factory_issue_134_01_Benchmarks*'
dotnet run -c Release -f net5.0 -- -f '*XmlParser_Factory_NavBar_Benchmarks*'
dotnet run -c Release -f net5.0 -- -f '*XmlParser_Factory_paths_data_02_t_Benchmarks*'
dotnet run -c Release -f net5.0 -- -f '*XmlParser_Factory_struct_svg_03_f_Benchmarks*'
dotnet run -c Release -f net5.0 -- -f '*XmlParser_Factory_tiger_Benchmarks*'

dotnet run -c Release -f net5.0 -- -f '*XmlParser_AJ_Digital_Camera_Benchmarks*'
dotnet run -c Release -f net5.0 -- -f '*XmlParser_Empty_Benchmarks*'
dotnet run -c Release -f net5.0 -- -f '*XmlParser_issue_134_01_Benchmarks*'
dotnet run -c Release -f net5.0 -- -f '*XmlParser_NavBar_Benchmarks*'
dotnet run -c Release -f net5.0 -- -f '*XmlParser_paths_data_02_t_Benchmarks*'
dotnet run -c Release -f net5.0 -- -f '*XmlParser_struct_svg_03_f_Benchmarks*'
dotnet run -c Release -f net5.0 -- -f '*XmlParser_tiger_Benchmarks*'
