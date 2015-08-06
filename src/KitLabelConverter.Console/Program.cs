namespace KitLabelConverter.Console
{
  using System;
  using System.IO;
  using System.IO.Abstractions;
  using System.Text;
  using FluentValidation;
  using KitLabelConverter.Abstract;
  using KitLabelConverter.Concrete;
  using KitLabelConverter.Concrete.Validators;
  using KitLabelConverter.Extractor;
  using SimpleInjector;
  using Console = System.Console;

  class Program
  {
    private static readonly Container Container;

    static Program()
    {
      Container = new Container();
      ConfigureContainer();
    }

    static int Main(string[] args)
    {
      var options = new Options();
      var exitCode = 0;

      if (!CommandLine.Parser.Default.ParseArguments(args, options)) {
        var err = GetParseErrorString(options);
        Console.Error.Write(err);
        exitCode = 1;
      }

      var extractor = Container.GetInstance<IExtractor<KitLabel>>();
      var settings = Container.GetInstance<ISettingsService>();

      var jobValidator = new JobValidator(settings);
      var columnMapValidator = new ColumnMapValidator();

      try {
        extractor.Initialize(options.InputPath);
        var columnMap = extractor.GetColumnMap(settings, 1, 1);
        columnMapValidator.ValidateAndThrow(columnMap);

        var kitLabels = extractor.Extract(columnMap, 1, 2);
        var job = new Job(columnMap.GetOutputHeaderString(), kitLabels);
        jobValidator.ValidateAndThrow(job);

        File.WriteAllLines(options.OutputPath, job.GetOutputList(), Encoding.Unicode);
      }
      catch (Exception exc) {
        exitCode = 2;
        Console.Error.Write(exc.Message);
      }

      return exitCode;
    }

    private static void ConfigureContainer()
    {
      Container.RegisterSingle<ISettingsService, KitLabelSettings>();
      Container.RegisterSingle<IFileSystem, FileSystem>();
      Container.Register<IDataSourceAdapter, FlexCelDataSourceAdapter>();
      Container.Register<IExtractor<KitLabel>, KitLabelExtractor>();
    }

    private static string GetParseErrorString(Options options)
    {
      var sb = new StringBuilder();
      if (options.InputPath == null) sb.AppendLine("Input path not specified.");
      if (options.OutputPath == null) sb.AppendLine("Output path not specified");

      return sb.ToString();
    }
  }
}
