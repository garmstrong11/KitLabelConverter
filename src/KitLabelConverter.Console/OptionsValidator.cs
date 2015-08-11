namespace KitLabelConverter.Console
{
  using System.IO;
  using System.Text.RegularExpressions;
  using FluentValidation;

  public class OptionsValidator : AbstractValidator<Options>
  {
    public OptionsValidator()
    {
      RuleFor(p => p.InputPath).NotEmpty().WithMessage("Input path not specified.");

      RuleFor(p => p.InputPath).Must(BeExcelFile)
        .WithMessage("Input file is not Excel format.");

      RuleFor(p => p.OutputPath).NotEmpty().WithMessage("Output path not specified.");
    }

    private static bool BeExcelFile(string path)
    {
      var extension = Path.GetExtension(path);
      return extension != null && Regex.IsMatch(extension, @"\.xlsx?");
    }
  }
}