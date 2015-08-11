namespace KitLabelConverter.Tests
{
  using System;
  using FluentAssertions;
  using FluentValidation;
  using KitLabelConverter.Console;
  using NUnit.Framework;

  [TestFixture]
  public class OptionsValidatorTests
  {
    [Test]
    public void InputFile_NotExcelFile_Throws()
    {
      var options = new Options {InputPath = @"C:\TestDir\TestFile.pdf", OutputPath = @"C:\TestDir\TestFile.txt"};
      var validator = new OptionsValidator();
      Action act = () => validator.ValidateAndThrow(options);

      act.ShouldThrow<ValidationException>().WithMessage("*not Excel*");
    }
  }
}