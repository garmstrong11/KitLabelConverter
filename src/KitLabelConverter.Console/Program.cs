namespace KitLabelConverter.Console
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.IO.Abstractions;
  using System.Linq;
  using System.Text;
  using FluentValidation;
  using KitLabelConverter.Abstract;
  using KitLabelConverter.Concrete;
  using KitLabelConverter.Concrete.Validators;
  using KitLabelConverter.Extractor;
  using SimpleInjector;

  class Program
  {
    private static readonly Container Container;

    static Program()
    {
      Container = new Container();
      ConfigureContainer();
    }

    static void Main(string[] args)
    {
      var options = new Options();

      var extractor = Container.GetInstance<IExtractor<KitLabel>>();
      var settings = Container.GetInstance<ISettingsService>();

      var jobValidator = new JobValidator(settings);
      var columnMapValidator = new ColumnMapValidator();
      var optionsValidator = new OptionsValidator();
      var kitLabels = new List<KitLabel>();
      Job job;

      CommandLine.Parser.Default.ParseArguments(args, options);

      try {
        optionsValidator.ValidateAndThrow(options);

        extractor.Initialize(options.InputPath);
        var columnMap = extractor.GetColumnMap(settings, 1, 1);

        columnMapValidator.ValidateAndThrow(columnMap);

        kitLabels.AddRange(extractor.Extract(columnMap, 1, 2));
        job = new Job(settings.OutputHeaderString, kitLabels);

        jobValidator.ValidateAndThrow(job);
      }

      catch (ValidationException exc) {
        var messages = string.Join(", ", exc.Errors.Select(e => e.ErrorMessage));
        var messageText = string.Format("** Warning! Input data file validation failed: {0}", messages);

        kitLabels = new List<KitLabel>
        {
          new KitLabel(69) {Attn = "Data errors detected", Sbu = "Error", KitName = messageText}
        };
      }

      catch (Exception exc) {
        var message = string.Format("** Warning! Excel conversion errors occurred: {0}", exc.Message);
        kitLabels = new List<KitLabel>
        {
          new KitLabel(70) {Attn = "Unknown Error detected", Sbu = "Error",  KitName = message}
        };
      }

      finally {
        job = new Job(settings.OutputHeaderString, kitLabels);
        File.WriteAllLines(options.OutputPath, job.GetOutputList(), Encoding.Unicode);
      }
    }

    private static void ConfigureContainer()
    {
      Container.RegisterSingle<ISettingsService, KitLabelSettings>();
      Container.RegisterSingle<IFileSystem, FileSystem>();
      Container.Register<IDataSourceAdapter, FlexCelDataSourceAdapter>();
      Container.Register<IExtractor<KitLabel>, KitLabelExtractor>();
    }
  }
}
