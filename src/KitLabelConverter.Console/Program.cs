namespace KitLabelConverter.Console
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.IO;
  using System.IO.Abstractions;
  using System.Text;
  using FluentValidation;
  using FluentValidation.Results;
  using KitLabelConverter.Abstract;
  using KitLabelConverter.Concrete;
  using KitLabelConverter.Concrete.Validators;
  using KitLabelConverter.Extractor;
  using SimpleInjector;
  using Console = System.Console;

  class Program
  {
    private static Container _container;

    static Program()
    {
      _container = new Container();
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

      //Console.WriteLine("Arguments parsed successfully!");
      var extractor = _container.GetInstance<IExtractor<KitLabel>>();
      var settings = _container.GetInstance<ISettingsService>();

      var jobValidator = new JobValidator();
      var columnMapValidator = new ColumnMapValidator();
      ValidationResult result;

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

      //Console.ReadLine();
      return exitCode;
    }

    private static void ConfigureContainer()
    {
      _container.RegisterSingle<ISettingsService, KitLabelSettings>();
      _container.RegisterSingle<IFileSystem, FileSystem>();
      _container.Register<IDataSourceAdapter, FlexCelDataSourceAdapter>();
      _container.Register<IExtractor<KitLabel>, KitLabelExtractor>();
    }

    private static string GetParseErrorString(Options options)
    {
      var sb = new StringBuilder();
      if (options.InputPath == null) sb.AppendLine("Input path not specified.");
      if (options.OutputPath == null) sb.AppendLine("Output path was not specified");

      return sb.ToString();
    }
  }
}
